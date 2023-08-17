using IB_projekat.ActivationTokens.Model;

namespace IB_projekat.ActivationTokens.Service
{
    public interface IActivationTokenService
    {
        public Task<ActivationToken> GenerateToken(int userId);
        public bool VerifyToken(string tokenValue, string storedHash);
        public Task RemoveToken(ActivationToken token);

        public Task<List<ActivationToken>> getTokenByUserId(int userId);
    }
}
