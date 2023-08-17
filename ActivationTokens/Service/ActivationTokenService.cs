using IB_projekat.ActivationTokens.Model;
using IB_projekat.ActivationTokens.Repository;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;

namespace IB_projekat.ActivationTokens.Service
{
    public class ActivationTokenService : IActivationTokenService
    {
        private const int TokenLength = 32; // Choose a suitable length for your tokens
        private const int ExpirationTimeMinutes = 30; // Choose a suitable expiration time for your tokens
        private readonly IActivationTokenRepository _tokenRepository;
        private readonly ILogger<ActivationTokenService> _logger;

        public ActivationTokenService(IActivationTokenRepository tokenRepository, ILogger<ActivationTokenService> logger)
        {
            _tokenRepository = tokenRepository;
            _logger = logger;
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
            _logger.LogInformation($"Generated token for userId: {userId}");
            return token;
        }

        public async Task<List<ActivationToken>> getTokenByUserId(int userId)
        {
            var tokens = await _tokenRepository.GetByUserId(userId);
            _logger.LogInformation($"Retrieved {tokens.Count} tokens for userId: {userId}");
            return tokens;
        }

        public async Task RemoveToken(ActivationToken token)
        {
            await _tokenRepository.DeleteOne(token);
            _logger.LogInformation($"Removed token for userId: {token.userId}");
        }

        public bool VerifyToken(string tokenValue, string storedHash)
        {
            var hash = ComputeHash(tokenValue);
            var isValid = string.Equals(hash, storedHash, StringComparison.Ordinal);
            if (isValid)
            {
                _logger.LogInformation("Token verification succeeded");
            }
            else
            {
                _logger.LogInformation("Token verification failed");
            }
            return isValid;
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
