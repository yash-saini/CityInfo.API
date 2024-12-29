using CityInfo.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [ApiController]
    [Route("api/cities")]
    public class CitiesController : ControllerBase
    {
        [HttpGet()]
        public ActionResult<IEnumerable<CityDTO>> GetCities()
        {
            var temp = CitiesDataStore.current.Cities;
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
           var temp = CitiesDataStore.current.Cities.FirstOrDefault(x => x.Id == id);

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
