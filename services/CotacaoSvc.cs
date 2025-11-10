using inoaProjectx.Models;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Http.Headers;

namespace inoaProjectx.Services
{
    public class CotacaoSvc
    {
        private readonly HttpClient _httpClient;
        private const string _apiURL = "https://brapi.dev/api/quote/";
        public CotacaoSvc()
        {
            _httpClient = new HttpClient();

        }
        public async Task<CotacaoModel> GetCotacaoAsync(string symbol)
        {
            try
            {
                string url = $"{_apiURL}{symbol}";
                var response = await _httpClient.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Erro ao buscar a cotação do ativo {symbol}");
                }
                var content = await response.Content.ReadAsStringAsync();
                using (JsonDocument doc = JsonDocument.Parse(content))
                {
                    var root = doc.RootElement;
                    var results = root.GetProperty("results");
                    Console.WriteLine("results: " + results);
                    if (results.GetArrayLength() == 0)
                    {throw new Exception($"Ativo {symbol} não encontrado");
                    }
                        

                    var primeiroResultado = results[0];
                    var preco = primeiroResultado.GetProperty("regularMarketPrice").GetDouble();
                    var simbolo = primeiroResultado.GetProperty("symbol").GetString();

                    

                    return new CotacaoModel(simbolo, preco);
                }
                
            }
            catch (System.Exception)
            {
                Console.WriteLine($"Erro ao buscar a cotação do ativo {symbol}");
                throw;
            }
        }


    }
}
