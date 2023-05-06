using IB_projekat.SmsVerification.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IB_projekat.SmsVerification.Repository
{
    public class SmsVerificationRepository : ISmsVerificationRepository
    {
        private readonly DatabaseContext _context;

        public SmsVerificationRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task AddOne(SmsVerificationCode smsCode)
        {
            await _context.SmsVerificationCodes.AddAsync(smsCode);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteOne(SmsVerificationCode smsCode)
        {
            _context.SmsVerificationCodes.Remove(smsCode);
            await _context.SaveChangesAsync();
        }

        public async Task<SmsVerificationCode> GetByCode(string code)
        {
            return await _context.SmsVerificationCodes.SingleOrDefaultAsync(r => r.Code == code);
        }

        public async Task<List<SmsVerificationCode>> GetByUserId(int userId)
        {
            return await _context.SmsVerificationCodes.Where(r => r.UserId == userId).ToListAsync();
        }
    }

}