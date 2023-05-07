using IB_projekat.Certificates.DTOS;
using IB_projekat.Requests.Model;

namespace IB_projekat.Requests.Service
{
    public interface IRequestService
    {
        Task Accept(int certId);
        Task Decline(int certId);
        Task Create(RequestDTO requestDTO);
        Task<List<Request>> GetByUserId(int id);
        Task<List<Request>> GetAll(int page, int pageSize);
        Task<List<Request>> GetRequestsByCertificateSerialNumber(int userId, int page, int pageSize);


    }
}
