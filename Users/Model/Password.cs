namespace IB_projekat.Users.Model
{
    public class Password
    {
        public int Id { get; set; }
        public User User { get; set; }
        public string DbPassword { get; set; }
        public PasswordStatus PasswordStatus { get; set; }
        public DateTime ExpirationDate { get; set; }

    }
    public enum PasswordStatus
    {
        ACTIVE,
        EXPIRED,
        INACTIVE
    }
}
