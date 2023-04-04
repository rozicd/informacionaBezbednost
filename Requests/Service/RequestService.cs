using IB_projekat.Certificates.DTOS;
using IB_projekat.Certificates.Model;
using IB_projekat.Certificates.Repository;
using IB_projekat.Requests.Model;
using IB_projekat.Requests.Model.Repository;
using IB_projekat.Users.Model;
using IB_projekat.Users.Repository;

namespace IB_projekat.Requests.Service
{
    public class RequestService : IRequestService
    {
        private readonly IRequestRepository _requestRepository;
        private readonly ICertificateRepository _certificateRepository;
        private readonly IUserRepository<User> _userRepository;

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
            request.User = requestDTO.User;
            request.Status = Status.Accepted;
            await _requestRepository.Update(request);
            // dodati generisanje
        }

        public async Task Create(RequestDTO requestDTO)
        {
            Request request = new Request();
            request.SignitureSerialNumber = requestDTO.SignitureSerialNumber;
            request.CertificateType = requestDTO.CertificateType;
            request.User = requestDTO.User;
            request.Status = Status.Pending;
            await _requestRepository.Add(request);
            Certificate certificate = _certificateRepository.GetBySerialNumber(request.SignitureSerialNumber);
            if (request.User.Id == certificate.Id)
            {
                Accept(requestDTO);
            }
        }

        public async Task Decline(RequestDTO requestDTO)
        {
            Request request = new Request();
            request.SignitureSerialNumber = requestDTO.SignitureSerialNumber;
            request.CertificateType = requestDTO.CertificateType;
            request.User = requestDTO.User;
            request.Status = Status.Declined;
            await _requestRepository.Update(request);
        }

        public async Task<IEnumerable<Request>> GetByUserId(int id)
        {
            return await _requestRepository.GetByUserId(id);
        }
    }
}
