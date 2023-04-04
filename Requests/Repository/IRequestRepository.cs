namespace IB_projekat.Requests.Model.Repository
{
    public interface IRequestRepository
    {
        Request GetById(int id);
        Task Add(Request request);
        Task Update(Request request);
        Task<IEnumerable<Request>> GetAll();
        Task<IEnumerable<Request>> GetByUserId(int id);
    }
}
