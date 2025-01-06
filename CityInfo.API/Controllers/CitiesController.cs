using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [ApiController]
    [Route("api/cities")]
    public class CitiesController : ControllerBase
    {
        private readonly CitiesDataStore _citiesDataStore;
        private readonly ILogger<CitiesController> _logger;
        public CitiesController(ILogger<CitiesController> logger, CitiesDataStore citiesDataStore)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _citiesDataStore = citiesDataStore ?? throw new ArgumentNullException(nameof(_citiesDataStore));
        }

        [HttpGet()]
        public ActionResult<IEnumerable<CityDTO>> GetCities()
        {
            var temp = _citiesDataStore.Cities;
            if (temp != null)
            {
                return Ok(temp);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("{id}")]
        public ActionResult<CityDTO> GetOneCity(int id)
        {
           var temp = _citiesDataStore.Cities.FirstOrDefault(x => x.Id == id);

            if (temp != null)
            {
                return Ok(temp);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
