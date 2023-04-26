namespace IB_projekat.ActivationTokens.DTOS
{
    public class ActivationTokenDTO
    {
        public string value { get; set; }
        public DateTime expires { get; set; }
        public int userId { get; set; }
    }
}
