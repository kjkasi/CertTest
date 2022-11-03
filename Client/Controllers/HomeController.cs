using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace Client.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet("disco")]
        public async Task<ActionResult> GetDiscoAsycn()
        {
            var client = _httpClientFactory.CreateClient();

            var disco = await client.GetDiscoveryDocumentAsync("https://host.docker.internal:5001");
            if (disco.IsError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, disco.Exception?.InnerException?.Message);
            }
            
            return Ok(disco.HttpResponse);
        }
    }
}
