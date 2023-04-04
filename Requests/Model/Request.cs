using IB_projekat.Certificates.DTOS;
using IB_projekat.Certificates.Model;
using IB_projekat.Users.Model;

namespace IB_projekat.Requests.Model
{
    public class Request
    {
        public int Id { get; set; }
        public CertificateType CertificateType { get; set; }
        public string SignitureSerialNumber { get; set; }
        public User User { get; set; }
        public Status Status { get; set; }

        
    }
    public enum Status
    {
        Pending,
        Accepted,
        Declined
    }
}
