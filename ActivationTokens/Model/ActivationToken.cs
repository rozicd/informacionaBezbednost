namespace IB_projekat.ActivationTokens.Model
{
    public class ActivationToken
    {
        

        public int id { get; set; }
        public string hash { get; set; }
        public string value { get; set; }
        public DateTime expires { get; set; }
        public int userId { get; set; }
    }
}
