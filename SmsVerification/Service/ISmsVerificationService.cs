using IB_projekat.SmsVerification.Model;

namespace IB_projekat.SmsVerification.Service
{
    public interface ISmsVerificationService
    {

        bool VerifyCode(string codeValue, string storedCode);
        Task<SmsVerificationCode> GenerateCode(int userId,VerificationType type);
        Task DeleteCode(SmsVerificationCode smsCode);
        SmsVerificationCode GetCodeByCodeValueAndType(string codeValue,VerificationType type);
        Task<List<SmsVerificationCode>> GetCodesByUserId(int userId);

    }
}