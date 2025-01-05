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

        [HttpGet("{pointOfInterestID}", Name = "GetSpecificPointOfInterest")]
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

        [HttpPost]
        public ActionResult<PointsOfInterestDTO> CreatePointOfInterest(int cityId, PointsOfInterestForCreationDTO pointOfInterest)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var concernedCity = CitiesDataStore.current.Cities.FirstOrDefault(x => x.Id == cityId);
            if (concernedCity == null)
            {
                return NotFound();
            }

            var maxPointOfInterestId = CitiesDataStore.current.Cities.SelectMany(c => c.PointsOfInterest).Max(p => p.Id);
            var finalPointOfInterest = new PointsOfInterestDTO()
            {
                Id = ++maxPointOfInterestId,
                Name = pointOfInterest.Name,
                Description = pointOfInterest.Description
            };

            concernedCity.PointsOfInterest.Add(finalPointOfInterest);

            return CreatedAtAction("GetSpecificPointOfInterest", new { cityId, pointOfInterestID = finalPointOfInterest.Id }, finalPointOfInterest);
        }

        [HttpPut("{pointOfInterestID}")]    
        public ActionResult UpdatePointOfInterest(int cityId, int pointOfInterestID, PointsOfInterestForUpdateDTO pointOfInterest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var concernedCity = CitiesDataStore.current.Cities.FirstOrDefault(x => x.Id == cityId);
            if (concernedCity == null)
            {
                return NotFound();
            }

            var pointOfInterestFromStore = concernedCity.PointsOfInterest.FirstOrDefault(poi => poi.Id == pointOfInterestID);
            if (pointOfInterestFromStore == null)
            {
                return NotFound();
            }

            pointOfInterestFromStore.Name = pointOfInterest.Name;
            pointOfInterestFromStore.Description = pointOfInterest.Description;

            return NoContent();
        }

        [HttpPatch("{pointOfInterestID}")]
        public ActionResult PartiallyUpdatePointOfInterest(int cityId, int pointOfInterestID, [FromBody] Microsoft.AspNetCore.JsonPatch.JsonPatchDocument<PointsOfInterestForUpdateDTO> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }

            var concernedCity = CitiesDataStore.current.Cities.FirstOrDefault(x => x.Id == cityId);
            if (concernedCity == null)
            {
                return NotFound();
            }

            var pointOfInterestFromStore = concernedCity.PointsOfInterest.FirstOrDefault(poi => poi.Id == pointOfInterestID);
            if (pointOfInterestFromStore == null)
            {
                return NotFound();
            }

            var pointOfInterestToPatch = new PointsOfInterestForUpdateDTO()
            {
                Name = pointOfInterestFromStore.Name,
                Description = pointOfInterestFromStore.Description
            };

            patchDocument.ApplyTo(pointOfInterestToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(!TryValidateModel(pointOfInterestToPatch))
            {
                return BadRequest(ModelState);
            }
            pointOfInterestFromStore.Name = pointOfInterestToPatch.Name;
            pointOfInterestFromStore.Description = pointOfInterestToPatch.Description;

            return NoContent();
        }
    }
}
