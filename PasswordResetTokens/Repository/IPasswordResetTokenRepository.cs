using IB_projekat.PasswordResetTokens.Model;

namespace IB_projekat.PasswordResetTokens.Repository
{
    public interface IPasswordResetTokenRepository
    {
        Task AddToken(PasswordResetToken token);
        Task<PasswordResetToken> GetTokenByToken(string token);
        Task DeleteToken(PasswordResetToken token);

    }
}