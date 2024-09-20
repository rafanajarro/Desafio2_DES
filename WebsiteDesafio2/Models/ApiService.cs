using Newtonsoft.Json;
using System.Text;

namespace WebsiteDesafio2.Models
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> ObtenerDatosDeApi(string url)
        {
            var respuesta = await _httpClient.GetAsync(url);

            if (respuesta.IsSuccessStatusCode)
            {
                return await respuesta.Content.ReadAsStringAsync();
            }
            else
            {
                throw new Exception("Error al consumir la API");
            }
        }

        public async Task<string> EnviarDatosALaApi(string url, object datos)
        {
            var contenido = new StringContent(JsonConvert.SerializeObject(datos), Encoding.UTF8, "application/json");

            Console.WriteLine(contenido);

            var respuesta = await _httpClient.PostAsync(url, contenido);

            if (respuesta.IsSuccessStatusCode)
            {
                return await respuesta.Content.ReadAsStringAsync();
            }
            else
            {
                throw new Exception("Error al enviar los datos a la API");
            }
        }

        public async Task<string> ActualizarDatosApi(string url, object datos)
        {
            Console.WriteLine(url);

            var contenido = new StringContent(JsonConvert.SerializeObject(datos), Encoding.UTF8, "application/json");


            Console.WriteLine("Contenido"+contenido);

            var respuesta = await _httpClient.PutAsync(url, contenido);

            if (respuesta.IsSuccessStatusCode)
            {
                return await respuesta.Content.ReadAsStringAsync();
            }
            else
            {
                throw new Exception("Error al enviar los datos a la API");
            }
        }

        public async Task<string> EliminarDatosApi(string url)
        {
            Console.WriteLine(url);
            var respuesta = await _httpClient.DeleteAsync(url);

            if (respuesta.IsSuccessStatusCode)
            {
                return await respuesta.Content.ReadAsStringAsync();
            }
            else
            {
                throw new Exception("Error al enviar los datos a la API");
            }
        }
    }
}
