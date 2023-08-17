using IB_projekat.ActivationTokens.DTOS;
using IB_projekat.ActivationTokens.Model;

namespace IB_projekat.ActivationTokens.Repository
{
    public interface IActivationTokenRepository
    {
        Task AddOne(ActivationToken activationToken);
        Task DeleteOne(ActivationToken activationToken);

        Task<List<ActivationToken>> GetByUserId(int id);
    }
}
