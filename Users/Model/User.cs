using IB_projekat.Certificates.Model;

namespace IB_projekat.Users.Model
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public UserType Role { get; set; }
        public bool IsOAuth { get; set; }

    }

    public enum UserType
    {
        Unauthorized,
        Authorized,
        Admin
    }


}
