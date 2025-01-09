using CityInfo.API.Entities;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace CityInfo.API.Controllers
{
    [Route("api/cities/{cityId}/pointsofinterest")]
    [ApiController]
    public class PointsOfInterestController : ControllerBase
    {
        private readonly ILogger<PointsOfInterestController> _logger;
        private readonly IMailService _mailService;
        private readonly ICityInfoRepository _cityInfoRepository;

        public PointsOfInterestController(ILogger<PointsOfInterestController> logger, IMailService mailService, ICityInfoRepository cityInfoRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mailService = mailService ?? throw new ArgumentNullException(nameof(mailService));
            _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PointsOfInterestDTO>>> GetPointsOfInterest(int cityId)
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                _logger.LogInformation($"City with id {cityId} wasn't found when accessing points of interest.");
                return NotFound();
            }

            var pointsOfInterestForCity = await _cityInfoRepository.GetPointsOfInterestsForCityAsync(cityId);
            var pointsOfInterestForCityResults = new List<PointsOfInterestDTO>();
             foreach (var pointOfInterest in pointsOfInterestForCity)
            {
                pointsOfInterestForCityResults.Add(new PointsOfInterestDTO
                {
                    Id = pointOfInterest.Id,
                    Name = pointOfInterest.Name,
                    Description = pointOfInterest.Description
                });
            }
             return Ok(pointsOfInterestForCityResults);
        }

        [HttpGet("{pointOfInterestID}", Name = "GetSpecificPointOfInterest")]
        public async Task<ActionResult<PointsOfInterestDTO>> GetSpecificPointOfInterest(int cityId, int pointOfInterestID)
        {
               if (!await _cityInfoRepository.CityExistsAsync(cityId))
                {
                    return NotFound();
                }

               var pointOfInterest = await _cityInfoRepository.GetPointOfInterestsForCityAsync(cityId, pointOfInterestID);

            if (pointOfInterest == null)    
                {
                return NotFound();
            }

            var pointOfInterestResult = new PointsOfInterestDTO()
            {
                Id = pointOfInterest.Id,
                Name = pointOfInterest.Name,
                Description = pointOfInterest.Description
            };
            return Ok(pointOfInterestResult);
        }

        [HttpPost]
        public async Task<ActionResult<PointsOfInterestDTO>> CreatePointOfInterest(int cityId, PointsOfInterestForCreationDTO pointOfInterest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (pointOfInterest.Description == pointOfInterest.Name)
            {
                ModelState.AddModelError("Description", "The provided description should be different from the name.");
            }

            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                return NotFound();
            }

            var finalPointOfInterest = new PointOfInterest()
            {
                Name = pointOfInterest.Name,
                Description = pointOfInterest.Description
            };

            await _cityInfoRepository.AddPointOfInterestForCityAsync(cityId, finalPointOfInterest);

            await _cityInfoRepository.SaveAsync();

            var createdPointOfInterestToReturn = new PointsOfInterestDTO()
            {
                Id = finalPointOfInterest.Id,
                Name = finalPointOfInterest.Name,
                Description = finalPointOfInterest.Description
            };

            return CreatedAtRoute("GetSpecificPointOfInterest", new { cityId, pointOfInterestID = createdPointOfInterestToReturn.Id }, createdPointOfInterestToReturn);
        }

        [HttpPut("{pointOfInterestID}")]    
        public async Task<ActionResult> UpdatePointOfInterest(int cityId, int pointOfInterestID, PointsOfInterestForUpdateDTO pointOfInterest)
        {
           if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (pointOfInterest.Description == pointOfInterest.Name)
            {
                ModelState.AddModelError("Description", "The provided description should be different from the name.");
            }

            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                return NotFound();
            }

            var pointOfInterestFromStore = await _cityInfoRepository.GetPointOfInterestsForCityAsync(cityId, pointOfInterestID);
            if (pointOfInterestFromStore == null)
            {
                return NotFound();
            }

            pointOfInterestFromStore.Name = pointOfInterest.Name;
            pointOfInterestFromStore.Description = pointOfInterest.Description;

            await _cityInfoRepository.SaveAsync();

            return NoContent();    
        }

        [HttpPatch("{pointOfInterestID}")]
        public async Task<ActionResult> PartiallyUpdatePointOfInterest(int cityId, int pointOfInterestID, [FromBody] Microsoft.AspNetCore.JsonPatch.JsonPatchDocument<PointsOfInterestForUpdateDTO> patchDocument)
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                return NotFound();
            }

            var pointOfInterestFromStore = await _cityInfoRepository.GetPointOfInterestsForCityAsync(cityId, pointOfInterestID);
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

            if (pointOfInterestToPatch.Description == pointOfInterestToPatch.Name)
            {
                ModelState.AddModelError("Description", "The provided description should be different from the name.");
            }

            TryValidateModel(pointOfInterestToPatch);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            pointOfInterestFromStore.Name = pointOfInterestToPatch.Name;
            pointOfInterestFromStore.Description = pointOfInterestToPatch.Description;

            await _cityInfoRepository.SaveAsync();

            return NoContent();
        }

        [HttpDelete("{pointOfInterestID}")]
        public async  Task<ActionResult> DeletePointOfInterest(int cityId, int pointOfInterestID)
        { 
            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                return NotFound();
            }

            var pointOfInterestFromStore = await _cityInfoRepository.GetPointOfInterestsForCityAsync(cityId, pointOfInterestID);
            if (pointOfInterestFromStore == null)
            {
                return NotFound();
            }

            _cityInfoRepository.DeletePointOfInterest(pointOfInterestFromStore);
            await _cityInfoRepository.SaveAsync();

            _mailService.Send("Point of interest deleted.", $"Point of interest {pointOfInterestFromStore.Name} with id {pointOfInterestFromStore.Id} was deleted.");

            return NoContent();
        }
    }
}
