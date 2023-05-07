using IB_projekat.Certificates.DTOS;
using IB_projekat.Certificates.Model;
using IB_projekat.Certificates.Repository;
using IB_projekat.Certificates.Service;
using IB_projekat.PaginatedResponseModel;
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
            await _certificateService.IssueCertificate(request.SignitureSerialNumber, request.User, request.Flags, DateTime.Now.AddHours(100000),request.CertificateType);
        }

        

        public async Task Create(RequestDTO requestDTO)
        {
            if(requestDTO.SignitureSerialNumber == null)
            {
                requestDTO.SignitureSerialNumber = " ";
            }
            Request request = new Request();
            request.SignitureSerialNumber = requestDTO.SignitureSerialNumber;
            request.CertificateType = requestDTO.CertificateType;
            request.User = await _userRepository.GetById(requestDTO.UserId);
            request.Status = Status.Pending;
            request.Flags = requestDTO.Flags;
            await _requestRepository.Add(request);
            if(requestDTO.SignitureSerialNumber == "")
            {
                await Accept(request.Id);
                return;
            }
            Certificate certificate = await _certificateRepository.GetBySerialNumber(request.SignitureSerialNumber);
            if (request.User.Id == certificate.User.Id)
            {
                await Accept(request.Id);
            }
        }

        public async Task Decline(int certId)
        {
            Request request = await _requestRepository.GetById(certId);
            request.Status = Status.Declined;
            await _requestRepository.Update(request);
        }

        public async Task<List<Request>> GetByUserId(int id)
        {
            return await _requestRepository.GetByUsersId(id);
        }

        public async Task<PaginationResponse<Request>> GetRequestsByCertificateSerialNumber(int userId, int page, int pageSize)
        {
            List<Request> requests =  await _requestRepository.GetRequestsByCertificateSerialNumber(userId, page, pageSize);
            int totalCount = await _requestRepository.GetTotalCountForUser(userId);
                return new PaginationResponse<Request>(requests, page, pageSize, totalCount);
        }

        public async Task<PaginationResponse<Request>> GetAll(int page, int pageSize)
        {
            List<Request> requests = await _requestRepository.GetAll(page, pageSize);
            int totalCount = await _requestRepository.GetRequestsCount();
            return new PaginationResponse<Request>(requests, page, pageSize, totalCount);
        }
    }
}
