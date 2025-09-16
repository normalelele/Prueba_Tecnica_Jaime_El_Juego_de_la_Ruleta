using Microsoft.AspNetCore.Mvc;
using RouletteBetting.Dtos;

namespace RouletteBetting.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RouletteController : ControllerBase
    {
        private static readonly Random _random = new();

        [HttpGet("spin")]
        public ActionResult<SpinResultDto> Spin()
        {
            var number = _random.Next(0, 37);

            var color = _random.Next(0, 2) == 0 ? "red" : "black";
            var parity = (number % 2 == 0) ? "even" : "odd";

            var dto = new SpinResultDto(number, color, parity);

            return Ok(dto);
        }

        [HttpPost("prize")]
        public ActionResult<BetResult> CalculatePrize([FromBody] BetRequest request)
        {
            if (request == null) return BadRequest("Solicitud requerida.");
            if (request.BetAmount <= 0) return BadRequest("El monto de apuesta debe ser mayor que 0.");

            bool won = false;
            decimal prize = 0m;
            decimal bet = request.BetAmount;

            var resultNumber = request.ResultNumber ?? -1;
            var resultColor = (request.ResultColor ?? string.Empty).ToLowerInvariant();

            switch (request.Type?.ToLowerInvariant())
            {
                case "color":
                    if (!string.IsNullOrEmpty(request.Color) && request.Color.ToLowerInvariant() == resultColor)
                    {
                        won = true;
                        prize = bet * 0.5m;
                    }
                    break;

                case "even-odd-color":
                case "paritycolor":
                    if (!string.IsNullOrEmpty(request.Parity) && !string.IsNullOrEmpty(request.Color))
                    {
                        var parityResult = (resultNumber % 2 == 0) ? "even" : "odd";
                        if (parityResult == request.Parity.ToLowerInvariant()
                            && request.Color.ToLowerInvariant() == resultColor)
                        {
                            won = true;
                            prize = bet * 1m;
                        }
                    }
                    break;

                case "number-color":
                    if (request.Number.HasValue && !string.IsNullOrEmpty(request.Color))
                    {
                        if (request.Number.Value == resultNumber && request.Color.ToLowerInvariant() == resultColor)
                        {
                            won = true;
                            prize = bet * 3m;
                        }
                    }
                    break;

                default:
                    return BadRequest("Tipo de apuesta no valido.");
            }

            if (!won) prize = -bet;

            var response = new BetResult { Won = won, Prize = prize };

            return Ok(response);
        }
    }
}
