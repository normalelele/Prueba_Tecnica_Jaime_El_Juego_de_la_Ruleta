namespace RouletteBetting.Dtos
{
    public class BetRequest
    {
        public string Type { get; set; } = null!;
        public string? Color { get; set; }
        public string? Parity { get; set; }
        public int? Number { get; set; }
        public decimal BetAmount { get; set; }
        public int? ResultNumber { get; set; }
        public string ResultColor { get; set; } = null!;
    }
}
