using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Domain
{
    [ApiController]
    [Route("[controller]")]
    public class PeerController : ControllerBase
    {
        private readonly ILogger<PeerController> _logger;
        private readonly IFizzBuzz _fizzBuzz;
        private static readonly Random _random = new Random();

        public PeerController(ILogger<PeerController> logger, IFizzBuzz fizzBuzz)
        {
            _logger = logger;
            _fizzBuzz = fizzBuzz;
        }

        [HttpGet("/test")]
        public async Task<ActionResult<string>> FizzBuzzTestAsync()
        {
            try
            {
                var result = await _fizzBuzz.CalculateResultAsync();
                _logger.LogInformation("FizzBuzz result calculated successfully.");
                return Ok($"Result: {result}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating FizzBuzz result.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        public ActionResult<string> FizzBuzz()
        {
            var rand = _random.Next(2);
            if (rand == 0)
            {
                return $"{_random.Next(100)}:{_random.Next(100)}";
            }
            return $"{_random.Next(100)}:{_random.Next(100)}:{_random.Next(100)}";
        }
    }
}
