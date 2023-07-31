using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
//using Test_Taste_Console_Application.Domain.Objects;
using System.Threading.Tasks;
using Test_Taste_Console_Application.Domain.DataTransferObjects;

using var httpClient = new HttpClient();


namespace Test_Taste_Console_Application.Domain.Objects
{
    public class Planet
    {
        public PlanetDto planet;

        //public  Planet(PlanetDto planet)
        //{
        //    this.planet = planet;
        //}

        public string Id { get; set; }
        public float SemiMajorAxis { get; set; }
        public string Name { get; set; }
        public dynamic Gravity { get; set; }
        public ICollection<Moon> Moons { get; set; }

        public float AverageMoonGravity
        {
            get => 0.0f;
        }

        public static async Task<List<Planet>> GetPlanetsWithMoons()
        {
            using (var httpClient = new HttpClient())
            {
                string apiUrl = "https://api.le-systeme-solaire.net/rest.php/bodies?filter[]=isPlanet,neq,0&filter[]=hasMoo" +
                               "n,eq,true";
                var response = await httpClient.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                dynamic data = JsonConvert.DeserializeObject(responseBody);

                List<Planet> planets = new List<Planet>();

                foreach (var body in data.bodies)
                {
                    Planet planet = new Planet
                    {
                        Name = body.englishName,
                        Moons = new List<Moon>()
                    };

                    //foreach (var moon in body.moons)
                    //{

                    //    // planet.Moons.Add(new Moon { Gravity = Convert.ToDouble(moon.gravity) });
                    //    ;
                    //}

                    planets.Add(planet);
                }

                return planets;
            }
        }

        static double CalculateAverageGravity(Planet planet)
        {
            double totalGravity = 0;

            foreach (var moon in planet.Moons)
            {
                totalGravity += moon.Gravity;
            }

            return totalGravity / planet.Moons.Count;
        }

        internal bool HasMoons()
        {
            throw new NotImplementedException();
        }
    }
}