using IB_projekat.SmsVerification.Model;
using IB_projekat.SmsVerification.Repository;
using System;
using System.Security.Cryptography;

namespace IB_projekat.SmsVerification.Service
{
    public class SmsVerificationService : ISmsVerificationService
    {
        private const int CodeLength = 6; // Choose a suitable length for your codes
        private const int ExpirationTimeMinutes = 5; // Choose a suitable expiration time for your codes
        private readonly ISmsVerificationRepository _smsVerificationRepository;

        public SmsVerificationService(ISmsVerificationRepository smsVerificationRepository)
        {
            _smsVerificationRepository = smsVerificationRepository;
        }

        public bool VerifyCode(string codeValue, string storedCode)
        {
            return string.Equals(codeValue, storedCode, StringComparison.Ordinal);
        }

        public async Task<SmsVerificationCode> GenerateCode(int userId,VerificationType type)
        {
            var randomInt = new Random().Next(100000, 999999); // generate random integer between 100000 and 999999
            var codeValue = randomInt.ToString();
            var expires = DateTime.Now.AddMinutes(ExpirationTimeMinutes);
            var code = new SmsVerificationCode { Code = codeValue, Expires = expires, UserId = userId };
            code.type = type;
            await _smsVerificationRepository.AddOne(code);
            return code;
        }

        public async Task DeleteCode(SmsVerificationCode smsCode)
        {
            await _smsVerificationRepository.DeleteOne(smsCode);
        }

        public SmsVerificationCode GetCodeByCodeValueAndType(string codeValue,VerificationType type)
        {
            return _smsVerificationRepository.GetByCodeAndType(codeValue,type).Result;
        }

        public async Task<List<SmsVerificationCode>> GetCodesByUserId(int userId)
        {
            return await _smsVerificationRepository.GetByUserId(userId);
        }
    }
}
