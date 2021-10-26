using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using JustDelivered.Config;
using Newtonsoft.Json;

namespace JustDelivered.Models
{
    public class Connect
    {
        public Connect()
        {
        }

        public async Task<Driver> GetUserProfile(string userID)
        {
            Driver result = null;

            try
            {
                var client = new HttpClient();
                var input = new ProfileInput() { driver_uid = userID };
                var serelizedObject = JsonConvert.SerializeObject(input);
                var stringContent = new StringContent(serelizedObject, Encoding.UTF8, "application/json");
                var endpointCall = await client.PostAsync(Constant.DriverProfile, stringContent);

                if (endpointCall.IsSuccessStatusCode)
                {
                    var endpointContentString = await endpointCall.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<Driver>(endpointContentString);
                }
            }
            catch
            {

            }

            return result;
        }
    }
}
