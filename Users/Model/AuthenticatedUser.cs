using IB_projekat.Certificates.Model;
namespace IB_projekat.Users.Model
{


    public class AuthenticatedUser : User
    {
        public ICollection<Certificate> Certificates { get; set; }
    }

}
