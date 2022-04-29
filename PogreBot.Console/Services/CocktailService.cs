using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PogreBot.Console.Services
{
    public class CocktailService
    {
        private readonly HttpClient _httpClient;
        public CocktailService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
    }
}
