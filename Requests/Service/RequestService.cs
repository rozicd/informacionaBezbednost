using IB_projekat.Certificates.DTOS;
using IB_projekat.Certificates.Model;
using IB_projekat.Certificates.Repository;
using IB_projekat.Certificates.Service;
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
        private readonly ICertificateService _certificateService;




        public RequestService(IRequestRepository requestRepository, ICertificateRepository certificateRepository, IUserRepository<User> userRepository , ICertificateService certificateService)
        {
            _requestRepository = requestRepository;
            _certificateService = certificateService;
            _certificateRepository = certificateRepository;
            _userRepository = userRepository;
        }

        public async Task Accept(int certId)
        {
            Request request = await _requestRepository.GetById(certId);
            request.Status = Status.Accepted;
            await _requestRepository.Update(request);
            await _certificateService.IssueCertificate(request.SignitureSerialNumber, request.User, request.Flags, DateTime.Now.AddHours(100000));
        }

        

        public async Task Create(RequestDTO requestDTO)
        {
            Request request = new Request();
            request.SignitureSerialNumber = requestDTO.SignitureSerialNumber;
            request.CertificateType = requestDTO.CertificateType;
            request.User = await _userRepository.GetById(requestDTO.UserId);
            request.Status = Status.Pending;
            request.Flags = requestDTO.Flags;
            await _requestRepository.Add(request);
            if (string.IsNullOrEmpty(request.SignitureSerialNumber))
            {
                await Accept(request.Id);

            }
            else
            {
                await Accept(request.Id);
            }
            /*Certificate certificate = await _certificateRepository.GetBySerialNumber(request.SignitureSerialNumber);
            if (request.User.Id == certificate.User.Id)
            {
                await Accept(request.Id);
            }
            }*/
        }

        public async Task Decline(int certId)
        {
            Request request = await _requestRepository.GetById(certId);
            request.Status = Status.Declined;
            await _requestRepository.Update(request);
        }

        public async Task<IEnumerable<Request>> GetByUserId(int id)
        {
            return await _requestRepository.GetByUsersId(id);
        }

        
    }
}
