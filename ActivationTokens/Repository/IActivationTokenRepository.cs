using IB_projekat.ActivationTokens.DTOS;
using IB_projekat.ActivationTokens.Model;

namespace IB_projekat.ActivationTokens.Repository
{
    public interface IActivationTokenRepository
    {
        Task<ActivationToken> GetByValue(string value);
        Task AddOne(ActivationTokenDTO activationTokenDTO);
        Task DeleteOne(int id);
    }
}
