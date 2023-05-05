using IB_projekat.Certificates.Model;
using IB_projekat.Certificates.Repository;
using IB_projekat.Requests.Model.Repository;
using IB_projekat.Users.Model;
using IB_projekat.Users.Repository;
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


        public CertificateService(ICertificateRepository certificateRepository, IUserRepository<User> userRepository, IRequestRepository requestRepository)
        {
            _userRepository = userRepository;
            _requestRepository = requestRepository;
            _certificateRepository = certificateRepository;
        }

        public async Task<IEnumerable<Certificate>> GetAll()
        {
            return await _certificateRepository.GetAll();
        }


        public async Task<Certificate> IssueCertificate(string? issuerSN, User user, string keyUsageFlags, DateTime validTo)
        {
            await Validate(issuerSN, user, keyUsageFlags, validTo);
            var cert = GenerateCertificate();

            var returnedCertificate = await ExportGeneratedCertificate(cert);
            return returnedCertificate;
        }

        private async Task Validate(string? issuerSN, User user, string keyUsageFlags, DateTime validTo)
        {

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
                    System.Console.WriteLine($"Comparing {validTo} and {DateTime.Now}");
                    throw new Exception("The date is not in the accepted range");
                }
            }
            this.validTo = validTo;

            subject = user;
            flags = ParseFlags(keyUsageFlags);
        }
        private X509Certificate2 GenerateCertificate()
        {
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

        private async Task<Certificate> ExportGeneratedCertificate(X509Certificate2 cert)
        {
            var certificateForDb = new Certificate()
            {
                Issuer = issuer?.SerialNumber,
                Status = CertificateStatus.Valid,
                CertificateType = isAuthority
                    ? issuerCertificate == null
                        ? CertificateType.Root
                        : CertificateType.Intermediate
                    : CertificateType.End,
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
            if (string.IsNullOrEmpty(keyUsageFlags))
            {
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
                    throw new Exception($"Unknown flag: {flag}");
                }
            }

            return retVal;
        }

        public async Task<List<Certificate>> GetAllCertificatesPaginated(int page, int pageSize)
        {
            return await _certificateRepository.GetAllCertificatesPaginated(page, pageSize);
        }
    }
}
