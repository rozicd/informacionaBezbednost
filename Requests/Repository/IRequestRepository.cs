namespace IB_projekat.Requests.Model.Repository
{
    public interface IRequestRepository
    {
        Task<Request> GetById(int id);
        Task Add(Request request);
        Task Update(Request request);
        Task<List<Request>> GetAll(int page, int pageSize);
        Task<List<Request>> GetByUsersId(int id);
        Task<List<Request>> GetRequestsByCertificateSerialNumber(int userId, int page, int pageSize);
    }
}
