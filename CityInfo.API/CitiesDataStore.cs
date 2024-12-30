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
                    Description = "Financial hub",
                    PointsOfInterest = new List<PointsOfInterestDTO>()
                    {
                        new PointsOfInterestDTO()
                        {
                            Id = 11,
                            Name = "Statue of Liberty",
                            Description = "Iconic site",
                        }
                    }

                },
                new CityDTO()
                {
                    Id = 2,
                    Name = "Buffalo",
                    Description = "My City",
                    PointsOfInterest = new List<PointsOfInterestDTO>()
                    {
                        new PointsOfInterestDTO()
                        {
                            Id = 11,
                            Name = "Niagara Falls",
                            Description = "Wonder of the World",
                        }
                    }
                },
            };
        }
    }
}
