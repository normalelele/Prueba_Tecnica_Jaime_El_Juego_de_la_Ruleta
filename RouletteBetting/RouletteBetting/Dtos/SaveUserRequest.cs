namespace RouletteBetting.Dtos
{
    public class SaveUserRequest
    {
        public string Username { get; set; } = null!;
        public decimal Amount { get; set; }
    }
}
