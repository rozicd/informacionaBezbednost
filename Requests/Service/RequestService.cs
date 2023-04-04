using IB_projekat.Certificates.DTOS;
using IB_projekat.Certificates.Model;
using IB_projekat.Certificates.Repository;
using IB_projekat.Requests.Model;
using IB_projekat.Requests.Model.Repository;
using IB_projekat.Users.Model;
using IB_projekat.Users.Repository;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace IB_projekat.Requests.Service
{
    public class RequestService : IRequestService
    {
        private readonly IRequestRepository _requestRepository;
        private readonly ICertificateRepository _certificateRepository;
        private readonly IUserRepository<User> _userRepository;
        private X509Certificate2 issuerCertificate;
        private DateTime validTo;
        private User subject;
        private bool isAuthority;
        private X509KeyUsageFlags flags;
        private string certDir = "certs";
        private string keyDir = "keys";
        private Certificate issuer;



        public RequestService(IRequestRepository requestRepository, ICertificateRepository certificateRepository, IUserRepository<User> userRepository)
        {
            _requestRepository = requestRepository;
            _certificateRepository = certificateRepository;
            _userRepository = userRepository;
        }

        public async Task Accept(RequestDTO requestDTO)
        {
            Request request = new Request();
            request.SignitureSerialNumber = requestDTO.SignitureSerialNumber;
            request.CertificateType = requestDTO.CertificateType;
            request.User = _userRepository.GetById(requestDTO.UserId).Result;
            request.Status = Status.Accepted;
            request.Flags = requestDTO.Flags;
            await _requestRepository.Update(request);
            IssueCertificate(request.SignitureSerialNumber, request.User, request.Flags, DateTime.Now.AddHours(100000));
        }

        public Certificate IssueCertificate(string? issuerSN, User user, string keyUsageFlags, DateTime validTo)
        {
            Validate(issuerSN, user, keyUsageFlags, validTo);
            var cert = GenerateCertificate();

            return ExportGeneratedCertificate(cert);
        }

        private void Validate(string? issuerSN, User user, string keyUsageFlags, DateTime validTo)
        {

            if (!string.IsNullOrEmpty(issuerSN))
            {
                issuer = _certificateRepository.GetBySerialNumber(issuerSN);
                //009CE3087D7227AE30
                issuerCertificate = new X509Certificate2($"{certDir}/{issuerSN}.crt");
                using (RSA rsa = RSA.Create())
                {
                    rsa.ImportRSAPrivateKey(File.ReadAllBytes($"{keyDir}/{issuerSN}.key"), out _);
                    issuerCertificate = issuerCertificate.CopyWithPrivateKey(rsa);
                }
            }

            if (!(validTo > DateTime.Now && (string.IsNullOrEmpty(issuerSN) || validTo < issuerCertificate.NotAfter)))
            {
                System.Console.WriteLine($"Comparing {validTo} and {DateTime.Now}");
                throw new Exception("The date is not in the accepted range");
            }
            this.validTo = validTo;

            subject = user;
            flags = ParseFlags(keyUsageFlags);
        }
        private X509Certificate2 GenerateCertificate()
        {
            var subjectText = $"CN={subject.Email}";
            RSA currentRSA = RSA.Create(4096);

            var certificateRequest = new CertificateRequest(subjectText, currentRSA, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

            certificateRequest.CertificateExtensions.Add(new X509BasicConstraintsExtension(isAuthority, false, 0, true));
            certificateRequest.CertificateExtensions.Add(new X509KeyUsageExtension(flags, false));

            var generatedCertificate = issuerCertificate == null
                    ? certificateRequest.CreateSelfSigned(DateTime.Now, validTo)
                    : certificateRequest.Create(issuerCertificate, DateTime.Now, validTo,
                        Guid.NewGuid().ToByteArray());
            return generatedCertificate;
        }

        private Certificate ExportGeneratedCertificate(X509Certificate2 cert)
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
                File.WriteAllBytes($"{keyDir}/{certificateForDb.SerialNumber}.key", RSA.Create(4096).ExportRSAPrivateKey());
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex);
            }

            _certificateRepository.Add(certificateForDb);

           

            return certificateForDb;
        }

        public async Task Create(RequestDTO requestDTO)
        {
            Request request = new Request();
            request.SignitureSerialNumber = requestDTO.SignitureSerialNumber;
            request.CertificateType = requestDTO.CertificateType;
            request.User = _userRepository.GetById(requestDTO.UserId).Result;
            request.Status = Status.Pending;
            request.Flags = requestDTO.Flags;
            await _requestRepository.Add(request);
            if (string.IsNullOrEmpty(request.SignitureSerialNumber))
            {
                Accept(requestDTO);

            }
            else 
            {
            
            Certificate certificate = _certificateRepository.GetBySerialNumber(request.SignitureSerialNumber);
            if (request.User.Id == certificate.User.Id)
            {
                Accept(requestDTO);
            }
            }
        }

        public async Task Decline(RequestDTO requestDTO)
        {
            Request request = new Request();
            request.SignitureSerialNumber = requestDTO.SignitureSerialNumber;
            request.CertificateType = requestDTO.CertificateType;
            request.User = _userRepository.GetById(requestDTO.UserId).Result;
            request.Status = Status.Declined;
            await _requestRepository.Update(request);
        }

        public async Task<IEnumerable<Request>> GetByUserId(int id)
        {
            return await _requestRepository.GetByUserId(id);
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
    }
}
