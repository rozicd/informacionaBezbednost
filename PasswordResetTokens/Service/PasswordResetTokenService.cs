using IB_projekat.PasswordResetTokens.Model;
using IB_projekat.PasswordResetTokens.Repository;
using System.Security.Cryptography;
using System.Text;

namespace IB_projekat.PasswordResetTokens.Service
{
    public class PasswordResetTokenService : IPasswordResetTokenService
    {
        private const int TokenLength = 32; // Choose a suitable length for your tokens
        private const int ExpirationTimeMinutes = 30; // Choose a suitable expiration time for your tokens
        private readonly IPasswordResetTokenRepository _passwordResetTokenRepository;

        public PasswordResetTokenService(IPasswordResetTokenRepository passwordResetTokenRepository)
        {
            _passwordResetTokenRepository = passwordResetTokenRepository;
        }

        public async Task<PasswordResetToken> GenerateToken(int userId)
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
            var expirationDate = DateTime.Now.AddMinutes(ExpirationTimeMinutes);
            var passwordResetToken = new PasswordResetToken { Token = tokenValue, ExpirationDate = expirationDate, UserId = userId };
            await _passwordResetTokenRepository.AddToken(passwordResetToken);
            return passwordResetToken;
        }

        public async Task<PasswordResetToken> GetTokenByToken(string token)
        {
            return await _passwordResetTokenRepository.GetTokenByToken(token);
        }

        public async Task DeleteToken(PasswordResetToken token)
        {
            await _passwordResetTokenRepository.DeleteToken(token);
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
