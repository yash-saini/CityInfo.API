using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CityInfo.API.Controllers
{
    [ApiController]
    [Route("api/cities")]
    public class CitiesController : ControllerBase
    {
        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly ILogger<CitiesController> _logger;
        const int maxCitiesPageSize = 20;

        public CitiesController(ILogger<CitiesController> logger, ICityInfoRepository cityInfoRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
        }

        [HttpGet()]
        public async Task<ActionResult<IEnumerable<CityWithoutPointOfInterestDTO>>> GetCities(string? name, string? searchQuery, int pageNumber = 1, int pageSize = 10)
        {
            //var temp = _citiesDataStore.Cities;
            //if (temp != null)
            //{
            //    return Ok(temp);
            //}
            //else
            //{
            //    return NotFound();
            //}
            if (pageSize > maxCitiesPageSize)
            {
                pageSize = maxCitiesPageSize;
            }
            var (cities , paginationMetaData) = await _cityInfoRepository.GetCitiesAsync(name, searchQuery, pageNumber, pageSize); 
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetaData));
            var results = new List<CityWithoutPointOfInterestDTO>();
            foreach (var city in cities)
            {
                results.Add(new CityWithoutPointOfInterestDTO
                {
                    Id = city.Id,
                    Name = city.Name,
                    Description = city.Description
                });
            }
            return Ok(results);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOneCity(int id, bool includePointsOfInterest = false)
        {
           //var temp = _citiesDataStore.Cities.FirstOrDefault(x => x.Id == id);

           // if (temp != null)
           // {
           //     return Ok(temp);
           // }
           // else
           // {
           //     return NotFound();
           // }
           var city = await _cityInfoRepository.GetCityAsync(id, includePointsOfInterest);
            if (city == null)
              {
                return NotFound();
              }
            if(includePointsOfInterest)
            {
                var cityResult = new CityDTO
                {
                    Id = city.Id,
                    Name = city.Name,
                    Description = city.Description
                };
                var pointsOfInterest = await _cityInfoRepository.GetPointsOfInterestsForCityAsync(id);
                foreach (var poi in pointsOfInterest)
                {
                    cityResult.PointsOfInterest.Add(new PointsOfInterestDTO
                    {
                        Id = poi.Id,
                        Name = poi.Name,
                        Description = poi.Description
                    });
                }
                return Ok(cityResult);
            }
            var result = new CityWithoutPointOfInterestDTO
                {
                Id = city.Id,
                Name = city.Name,
                Description = city.Description
            };
            return Ok(result);
        }
    }
}
