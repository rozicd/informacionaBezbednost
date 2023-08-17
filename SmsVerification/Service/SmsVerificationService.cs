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
        private readonly Serilog.ILogger _logger;

        public SmsVerificationService(ISmsVerificationRepository smsVerificationRepository, Serilog.ILogger logger)
        {
            _smsVerificationRepository = smsVerificationRepository;
            _logger = logger;
        }

        public bool VerifyCode(string codeValue, string storedCode)
        {
            bool isValid = string.Equals(codeValue, storedCode, StringComparison.Ordinal);
            _logger.Information("Verification code: {CodeValue} - Valid: {IsValid}", codeValue, isValid);
            return isValid;
        }

        public async Task<SmsVerificationCode> GenerateCode(int userId, VerificationType type)
        {
            var randomInt = new Random().Next(100000, 999999); // generate random integer between 100000 and 999999
            var codeValue = randomInt.ToString();
            var expires = DateTime.Now.AddMinutes(ExpirationTimeMinutes);
            var code = new SmsVerificationCode { Code = codeValue, Expires = expires, UserId = userId };
            code.type = type;
            await _smsVerificationRepository.AddOne(code);
            _logger.Information("Generated SMS verification code: {CodeValue} for user with ID {UserId}", codeValue, userId);
            return code;
        }

        public async Task DeleteCode(SmsVerificationCode smsCode)
        {
            await _smsVerificationRepository.DeleteOne(smsCode);
            _logger.Information("Deleted SMS verification code with ID {CodeId}", smsCode.Id);
        }

        public SmsVerificationCode GetCodeByCodeValueAndType(string codeValue, VerificationType type)
        {
            SmsVerificationCode code = _smsVerificationRepository.GetByCodeAndType(codeValue, type).Result;
            _logger.Information("Retrieved SMS verification code with value: {CodeValue} and type: {Type}", codeValue, type);
            return code;
        }

        public async Task<List<SmsVerificationCode>> GetCodesByUserId(int userId)
        {
            List<SmsVerificationCode> codes = await _smsVerificationRepository.GetByUserId(userId);
            _logger.Information("Retrieved {Count} SMS verification codes for user with ID {UserId}", codes.Count, userId);
            return codes;
        }
    }
}
