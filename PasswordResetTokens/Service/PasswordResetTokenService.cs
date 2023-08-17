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
        private readonly Serilog.ILogger _logger;

        public PasswordResetTokenService(IPasswordResetTokenRepository passwordResetTokenRepository, Serilog.ILogger logger)
        {
            _passwordResetTokenRepository = passwordResetTokenRepository;
            _logger = logger;
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

            _logger.Information("Generated password reset token for user {UserId}.", userId);

            return passwordResetToken;
        }

        public async Task<PasswordResetToken> GetTokenByToken(string token)
        {
            var passwordResetToken = await _passwordResetTokenRepository.GetTokenByToken(token);

            if (passwordResetToken != null)
            {
                _logger.Information("Retrieved password reset token for token {Token}.", token);
            }
            else
            {
                _logger.Information("No password reset token found for token {Token}.", token);
            }

            return passwordResetToken;
        }

        public async Task DeleteToken(PasswordResetToken token)
        {
            await _passwordResetTokenRepository.DeleteToken(token);

            _logger.Information("Deleted password reset token for user {UserId}.", token.UserId);
        }

        public bool VerifyToken(string tokenValue, string storedHash)
        {
            var hash = ComputeHash(tokenValue);
            var isTokenValid = string.Equals(hash, storedHash, StringComparison.Ordinal);

            _logger.Information("Token verification result: {IsValid}.", isTokenValid);

            return isTokenValid;
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
