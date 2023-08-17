using IB_projekat.PasswordResetTokens.Model;

namespace IB_projekat.PasswordResetTokens.Service
{
    public interface IPasswordResetTokenService
    {
        Task<PasswordResetToken> GenerateToken(int userId);
        Task<PasswordResetToken> GetTokenByToken(string token);
        Task DeleteToken(PasswordResetToken token);
        bool VerifyToken(string tokenValue, string storedHash);

    }
}