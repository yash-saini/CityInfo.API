using CityInfo.API.Models;

namespace CityInfo.API
{
    public class CitiesDataStore
    {
        public List<CityDTO> Cities { get; set; }

        public static CitiesDataStore current { get;} = new CitiesDataStore();

        public CitiesDataStore()
        {

            Cities = new List<CityDTO>()
            {
                new CityDTO()
                {
                    Id = 1,
                    Name = "New York City",
                    Description = "Financial hub"
                },
                new CityDTO()
                {
                    Id = 2,
                    Name = "Buffalo",
                    Description = "My City"
                },
            };
        }
    }
}
