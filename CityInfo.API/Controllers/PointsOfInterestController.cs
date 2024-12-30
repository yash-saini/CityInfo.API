using CityInfo.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [Route("api/cities/{cityId}/pointsofinterest")]
    [ApiController]
    public class PointsOfInterestController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<PointsOfInterestDTO>> GetPointsOfInterest(int cityId)
        {
            var concernedCity = CitiesDataStore.current.Cities.FirstOrDefault(x => x.Id == cityId);

            if (concernedCity == null)
            {
                return NotFound();
            }

            var pointOfInterests = concernedCity.PointsOfInterest;
            if (pointOfInterests == null)
            {
                return NotFound();
            }

            else
            {
                return Ok(pointOfInterests);
            }
        }

        [HttpGet("{pointOfInterestID}")]
        public ActionResult<List<PointsOfInterestDTO>> GetSpecificPointOfInterest(int cityId, int pointOfInterestID)
        {
            var concernedCity = CitiesDataStore.current.Cities.FirstOrDefault(x => x.Id == cityId);
            if (concernedCity == null)
            {
                return NotFound();
            }
            var pointOfInterest = concernedCity.PointsOfInterest.FirstOrDefault(poi => poi.Id == pointOfInterestID);
            if (pointOfInterest == null)
            {
                return NotFound();
            }

            else
            {
                return Ok(pointOfInterest);
            }
        }
    }
}
