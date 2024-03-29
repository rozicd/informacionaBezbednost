﻿using IB_projekat.Certificates.Model;
using IB_projekat.Certificates.Repository;
using IB_projekat.Requests.Model.Repository;
using IB_projekat.Users.Model;
using IB_projekat.Users.Repository;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace IB_projekat.Certificates.Service
{
    public class CertificateService : ICertificateService
    {
        private readonly ICertificateRepository _certificateRepository;
        private readonly IRequestRepository _requestRepository;
        private readonly IUserRepository<User> _userRepository;
        private X509Certificate2 issuerCertificate;
        private DateTime validTo;
        private User subject;
        private bool isAuthority;
        private X509KeyUsageFlags flags;
        private string certDir = "certs";
        private string keyDir = "keys";
        private RSA currentRSA;
        private Certificate issuer;
        private Serilog.ILogger _logger;

        public CertificateService(ICertificateRepository certificateRepository, IUserRepository<User> userRepository, IRequestRepository requestRepository, Serilog.ILogger logger)
        {
            _userRepository = userRepository;
            _requestRepository = requestRepository;
            _certificateRepository = certificateRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<Certificate>> GetAll()
        {
            _logger.Information("Getting all certificates");
            return await _certificateRepository.GetAll();
        }

        public async Task<Certificate> IssueCertificate(string? issuerSN, User user, string keyUsageFlags, DateTime validTo, CertificateType type)
        {
            _logger.Information("Issuing certificate");
            await Validate(issuerSN, user, keyUsageFlags, validTo);
            var cert = GenerateCertificate();

            var returnedCertificate = await ExportGeneratedCertificate(cert, type);
            return returnedCertificate;
        }

        private async Task Validate(string? issuerSN, User user, string keyUsageFlags, DateTime validTo)
        {
            _logger.Information("Validating certificate request");
            if (!string.IsNullOrEmpty(issuerSN))
            {
                issuer = await _certificateRepository.GetBySerialNumber(issuerSN);
                issuerCertificate = new X509Certificate2($"{certDir}/{issuerSN}.crt");
                using (RSA rsa = RSA.Create())
                {
                    rsa.ImportRSAPrivateKey(File.ReadAllBytes($"{keyDir}/{issuerSN}.key"), out _);
                    RSA publicKey = issuerCertificate.GetRSAPublicKey();
                    issuerCertificate = issuerCertificate.CopyWithPrivateKey(rsa);
                }
            }
            if (!string.IsNullOrEmpty(issuerSN))
            {
                while (validTo > issuerCertificate.NotAfter)
                {
                    validTo = validTo.AddHours(-2);
                }

                if (!(validTo > DateTime.Now))
                {
                    _logger.Error("The date is not in the accepted range");
                    throw new Exception("The date is not in the accepted range");
                }
            }
            this.validTo = validTo;

            subject = user;
            flags = ParseFlags(keyUsageFlags);
        }

        private X509Certificate2 GenerateCertificate()
        {
            _logger.Information("Generating certificate");
            var subjectText = $"CN={subject.Email}";
            currentRSA = RSA.Create(4096);

            var certificateRequest = new CertificateRequest(subjectText, currentRSA, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

            certificateRequest.CertificateExtensions.Add(new X509BasicConstraintsExtension(isAuthority, false, 0, true));
            certificateRequest.CertificateExtensions.Add(new X509KeyUsageExtension(flags, false));

            var generatedCertificate = issuerCertificate == null
                    ? certificateRequest.CreateSelfSigned(DateTime.Now, validTo)
                    : certificateRequest.Create(issuerCertificate, DateTime.Now, validTo,
                        Guid.NewGuid().ToByteArray());
            return generatedCertificate;
        }

        private async Task<Certificate> ExportGeneratedCertificate(X509Certificate2 cert, CertificateType type)
        {
            _logger.Information("Exporting generated certificate");
            var certificateForDb = new Certificate()
            {
                Issuer = issuer?.SerialNumber,
                Status = CertificateStatus.Valid,
                CertificateType = type,
                SerialNumber = cert.SerialNumber,
                SignatureAlgorithm = cert.SignatureAlgorithm.FriendlyName ?? "Unknown",
                User = subject,
                ValidFrom = cert.NotBefore,
                ValidTo = cert.NotAfter
            };

            try
            {
                File.WriteAllBytes($"{certDir}/{certificateForDb.SerialNumber}.crt", cert.Export(X509ContentType.Cert));
                File.WriteAllBytes($"{keyDir}/{certificateForDb.SerialNumber}.key", currentRSA.ExportRSAPrivateKey());
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error exporting certificate");
                Console.WriteLine(ex);
            }

            await _certificateRepository.Add(certificateForDb);

            return certificateForDb;
        }

        public Task<bool> VerifyCertificate(string certificateSerialNumber)
        {
            throw new NotImplementedException();
        }

        private X509KeyUsageFlags ParseFlags(string keyUsageFlags)
        {
            _logger.Information("Parsing key usage flags");
            if (string.IsNullOrEmpty(keyUsageFlags))
            {
                _logger.Error("KeyUsageFlags are mandatory");
                throw new Exception("KeyUsageFlags are mandatory");
            }

            var flagArray = keyUsageFlags.Split(",");
            var retVal = X509KeyUsageFlags.None;

            var possibleElements = Enum.GetValues<X509KeyUsageFlags>();

            foreach (var flag in flagArray)
            {
                if (int.TryParse(flag, out int index))
                {
                    var currentFlag = possibleElements[index];

                    retVal |= currentFlag;

                    if (currentFlag == X509KeyUsageFlags.KeyCertSign)
                    {
                        isAuthority = true;
                    }
                }
                else
                {
                    _logger.Error($"Unknown flag: {flag}");
                    throw new Exception($"Unknown flag: {flag}");
                }
            }

            return retVal;
        }

        public async Task<bool> ValidateCert(string serialNumber)
        {
            _logger.Information("Validating certificate by serial number");
            Certificate certDB = await _certificateRepository.GetBySerialNumber(serialNumber);
            if (certDB == null)
            {
                return false;
            }
            if (certDB.Status != CertificateStatus.Valid) return false;
            X509Certificate2 cert;
            using (RSA rsa = RSA.Create())
            {
                try
                {
                    cert = new X509Certificate2($"{certDir}/{serialNumber}.crt");
                    rsa.ImportRSAPrivateKey(File.ReadAllBytes($"{keyDir}/{serialNumber}.key"), out _);
                    RSA publicKey = cert.GetRSAPublicKey();
                    cert = cert.CopyWithPrivateKey(rsa);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Error validating certificate");
                    return false;
                }
            }

            if (!certDB.CertificateType.Equals(CertificateType.Root))
            {
                if (ValidateCert(certDB.Issuer).Result == false) return false; ;
            }

            if (!(cert.NotAfter > DateTime.Now))
            {
                return false;
            }
            if (!(cert.NotBefore < DateTime.Now))
            {
                return false;
            }

            return true;
        }

        public async Task<bool> ValidateCertFile(X509Certificate2 cert)
        {
            _logger.Information("Validating certificate file");
            Console.WriteLine(cert.SerialNumber);
            Certificate certDB = await _certificateRepository.GetBySerialNumber(cert.SerialNumber);
            if (certDB == null)
            {
                return false;
            }
            if (certDB.Status != CertificateStatus.Valid) return false;
            using (RSA rsa = RSA.Create())
            {
                try
                {
                    rsa.ImportRSAPrivateKey(File.ReadAllBytes($"{keyDir}/{cert.SerialNumber}.key"), out _);
                    RSA publicKey = cert.GetRSAPublicKey();
                    cert = cert.CopyWithPrivateKey(rsa);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Error validating certificate file");
                    return false;
                }
            }

            if (!certDB.CertificateType.Equals(CertificateType.Root))
            {
                if (ValidateCert(certDB.Issuer).Result == false) return false; ;
            }

            if (!(cert.NotAfter > DateTime.Now))
            {
                return false;
            }
            if (!(cert.NotBefore < DateTime.Now))
            {
                return false;
            }

            return true;
        }

        public async Task<Certificate> GetCertificateBySerialNumber(string serialNumber)
        {
            _logger.Information("Getting certificate by serial number");
            return await _certificateRepository.GetBySerialNumber(serialNumber);
        }

        public async Task<bool> RevokeCert(string serialNumber, string userEmail)
        {
            _logger.Information("Revoking certificate");
            Console.WriteLine(serialNumber);
            Certificate certDB = await _certificateRepository.GetBySerialNumber(serialNumber);
            if (certDB == null)
            {
                return false;
            }
            if (userEmail != null)
            {
                User user = await _userRepository.GetByEmail(userEmail);
                if (!user.Role.Equals(UserType.Admin))
                    if (!user.Email.Equals(certDB.User.Email))
                        return false;
            }
            List<Certificate> issuedCertificates = new List<Certificate>();
            try
            {
                issuedCertificates = (List<Certificate>)await _certificateRepository.GetAllIssued(serialNumber);
            }
            catch
            {
                Console.WriteLine("no More");
            }
            foreach (Certificate cert in issuedCertificates)
            {
                await RevokeCert(cert.SerialNumber, null);
            }
            certDB.Status = CertificateStatus.Revoked;
            await _certificateRepository.Update(certDB);
            return true;
        }

        public async Task<List<Certificate>> GetAllCertificatesPaginated(int page, int pageSize)
        {
            _logger.Information("Getting all certificates paginated");
            return await _certificateRepository.GetAllCertificatesPaginated(page, pageSize);
        }
    }
}
