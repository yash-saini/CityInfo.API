using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [ApiController]
    [Route("api/cities")]
    public class CitiesController : ControllerBase
    {
        [HttpGet()]
        public JsonResult GetCities()
        {
            return new JsonResult(CitiesDataStore.current.Cities);
        }

        [HttpGet("{id}")]
        public JsonResult GetOneCity(int id)
        {
            return new JsonResult(CitiesDataStore.current.Cities.FirstOrDefault(x => x.Id == id));
        }
    }
}
