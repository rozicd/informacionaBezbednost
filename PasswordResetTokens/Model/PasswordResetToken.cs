namespace IB_projekat.PasswordResetTokens.Model
{
    public class PasswordResetToken
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int UserId { get; set; }
    }
}
