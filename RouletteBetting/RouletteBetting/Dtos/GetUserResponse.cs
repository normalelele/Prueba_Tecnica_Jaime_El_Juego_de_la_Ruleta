namespace RouletteBetting.Dtos
{
    public class GetUserResponse
    {
        public string Username { get; set; } = null!;
        public decimal Balance { get; set; }
    }
}
