using IB_projekat.ActivationTokens.DTOS;
using IB_projekat.ActivationTokens.Model;
using Microsoft.EntityFrameworkCore;

namespace IB_projekat.ActivationTokens.Repository
{
    public class ActivationTokenRepository : IActivationTokenRepository
    {

        public Task AddOne(ActivationTokenDTO activationTokenDTO)
        {
            throw new NotImplementedException();

        }

        public Task DeleteOne(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ActivationToken> GetByValue(string value)
        {
            throw new NotImplementedException();
        }
    }
}
