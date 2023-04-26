using IB_projekat.ActivationTokens.Model;
using IB_projekat.ActivationTokens.Repository;
using System.Security.Cryptography;
using System.Text;

namespace IB_projekat.ActivationTokens.Service
{
    public class ActivationTokenService : IActivationTokenService
    {
        private const int TokenLength = 32; // Choose a suitable length for your tokens
        private const int ExpirationTimeMinutes = 30; // Choose a suitable expiration time for your tokens
        private IActivationTokenRepository _tokenRepository;

        public ActivationTokenService(IActivationTokenRepository tokenRepository)
        {
            _tokenRepository = tokenRepository;
        }

        public async Task<ActivationToken> GenerateToken(int userId)
        {
            var randomBytes = new byte[TokenLength];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }
            var tokenValue = Convert.ToBase64String(randomBytes).Replace("+", "-")
                                                        .Replace("/", "_")
                                                        .Replace("=", "");
            var hash = ComputeHash(tokenValue);
            var expires = DateTime.Now.AddMinutes(ExpirationTimeMinutes);
            ActivationToken token = new ActivationToken { value = tokenValue, hash = hash, expires = expires, userId = userId };
            await _tokenRepository.AddOne(token);
            return token;
        }

        public async Task<List<ActivationToken>> getTokenByUserId(int userId)
        {
            return await _tokenRepository.GetByUserId(userId);
        }

        public async Task RemoveToken(ActivationToken token)
        {
            await _tokenRepository.DeleteOne(token);
        }

        public bool VerifyToken(string tokenValue, string storedHash)
        {
            var hash = ComputeHash(tokenValue);
            return string.Equals(hash, storedHash, StringComparison.Ordinal);
        }

        private string ComputeHash(string input)
        {
            using (var sha256 = SHA256.Create())
            {
                var inputBytes = Encoding.UTF8.GetBytes(input);
                var hashBytes = sha256.ComputeHash(inputBytes);
                return Convert.ToBase64String(hashBytes)
                                                        .Replace("+", "-")
                                                        .Replace("/", "_")
                                                        .Replace("=", "");
            }
        }



    }
}
