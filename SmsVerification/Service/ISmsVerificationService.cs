using IB_projekat.SmsVerification.Model;

namespace IB_projekat.SmsVerification.Service
{
    public interface ISmsVerificationService
    {

        bool VerifyCode(string codeValue, string storedCode);
        Task<SmsVerificationCode> GenerateCode(int userId);
        Task DeleteCode(SmsVerificationCode smsCode);
        SmsVerificationCode GetCodeByCodeValue(string codeValue);
        Task<List<SmsVerificationCode>> GetCodesByUserId(int userId);

    }
}