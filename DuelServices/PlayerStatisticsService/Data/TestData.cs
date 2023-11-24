namespace PlayerStatisticsService.Data;

using System;
using System.Net.Http;
using System.Threading.Tasks;

class TestData
{
    static async Task sendData()
    {
        using (HttpClient client = new HttpClient())
        {
            string apiUrl = "http://localhost/ziel-endpunkt?parameter1=Wert1&parameter2=Wert2";
            HttpResponseMessage response = await client.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                string responseData = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseData);
            }
            else
            {
                Console.WriteLine($"Fehler: {response.StatusCode}");
            }
        }
    }
}
