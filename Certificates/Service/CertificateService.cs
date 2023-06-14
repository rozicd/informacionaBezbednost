using IB_projekat.Certificates.Model;
using IB_projekat.Certificates.Repository;
using IB_projekat.Requests.Model.Repository;
using IB_projekat.Users.Model;
using IB_projekat.Users.Repository;
using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace IB_projekat.Certificates.Service
{
    public class CertificateService : ICertificateService
    {
        private readonly ILogger _logger;
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

        public CertificateService(ICertificateRepository certificateRepository, IUserRepository<User> userRepository, IRequestRepository requestRepository, ILogger logger)
        {
            _userRepository = userRepository;
            _requestRepository = requestRepository;
            _certificateRepository = certificateRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<Certificate>> GetAll()
        {
            return await _certificateRepository.GetAll();
        }

        public async Task<Certificate> IssueCertificate(string? issuerSN, User user, string keyUsageFlags, DateTime validTo, CertificateType type)
        {
            await Validate(issuerSN, user, keyUsageFlags, validTo);
            var cert = GenerateCertificate();

            var returnedCertificate = await ExportGeneratedCertificate(cert, type);
            return returnedCertificate;
        }

        private async Task Validate(string? issuerSN, User user, string keyUsageFlags, DateTime validTo)
        {
            _logger.Information("Certificate validation started.");

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
                    _logger.Warning("The provided validTo date {validTo} is not in the accepted range");
                    throw new Exception("The date is not in the accepted range");
                }
            }

            this.validTo = validTo;
            subject = user;
            flags = ParseFlags(keyUsageFlags);

            _logger.Information("Certificate validation successful.");
        }

        private X509Certificate2 GenerateCertificate()
        {
            _logger.Information("Generating certificate.");

            var subjectText = $"CN={subject.Email}";
            currentRSA = RSA.Create(4096);

            var certificateRequest = new CertificateRequest(subjectText, currentRSA, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

            certificateRequest.CertificateExtensions.Add(new X509BasicConstraintsExtension(isAuthority, false, 0, true));
            certificateRequest.CertificateExtensions.Add(new X509KeyUsageExtension(flags, false));

            var generatedCertificate = issuerCertificate == null
                ? certificateRequest.CreateSelfSigned(DateTime.Now, validTo)
                : certificateRequest.Create(issuerCertificate, DateTime.Now, validTo, Guid.NewGuid().ToByteArray());

            _logger.Information("Certificate generation successful.");

            return generatedCertificate;
        }

        private async Task<Certificate> ExportGeneratedCertificate(X509Certificate2 cert, CertificateType type)
        {
            _logger.Information("Exporting generated certificate files.");

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

                _logger.Information("Generated certificate files exported successfully.");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error while exporting generated certificate files.");
                Console.WriteLine(ex);
            }

            await _certificateRepository.Add(certificateForDb);

            _logger.Information("Generated certificate added to the database.");

            return certificateForDb;
        }

        public Task<bool> VerifyCertificate(string certificateSerialNumber)
        {
            throw new NotImplementedException();
        }

        private X509KeyUsageFlags ParseFlags(string keyUsageFlags)
        {
            if (string.IsNullOrEmpty(keyUsageFlags))
            {
                _logger.Warning("KeyUsageFlags are mandatory");
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
                    _logger.Warning($"Unknown flag: {flag}");
                    throw new Exception($"Unknown flag: {flag}");
                }
            }

            return retVal;
        }

        public async Task<bool> ValidateCert(string serialNumber)
        {
            _logger.Information("Validating certificate with serial number {serialNumber}.", serialNumber);

            Certificate certDB = await _certificateRepository.GetBySerialNumber(serialNumber);
            if (certDB == null)
            {
                return false;
            }

            if (certDB.Status != CertificateStatus.Valid)
            {
                _logger.Information("Certificate with serial number {serialNumber} is not in a valid status.", serialNumber);
                return false;
            }

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
                    _logger.Error(ex, "Error while validating certificate files.");
                    return false;
                }
            }

            if (!certDB.CertificateType.Equals(CertificateType.Root))
            {
                if (await ValidateCert(certDB.Issuer) == false)
                {
                    _logger.Information("Certificate issuer with serial number {serialNumber} is not valid.", certDB.Issuer);
                    return false;
                }
            }

            if (certDB.ValidFrom > DateTime.Now || certDB.ValidTo < DateTime.Now)
            {
                _logger.Information("Certificate with serial number {serialNumber} is not valid in the current time range.", serialNumber);
                return false;
            }

            _logger.Information("Certificate with serial number {serialNumber} is valid.", serialNumber);
            return true;
        }
    }
}
