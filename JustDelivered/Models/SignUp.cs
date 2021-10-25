using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using JustDelivered.Config;
using JustDelivered.LogIn.Classes;
using Newtonsoft.Json;
using static JustDelivered.Views.DeliveriesPage;

namespace JustDelivered.Models
{
    public class SignUp
    {
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string business_uid { get; set; }
        public string referral_source { get; set; }
        public string driver_hours { get; set; }
        public string street { get; set; }
        public string unit { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zipcode { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string ssn { get; set; }
        public string license_num { get; set; }
        public string license_exp { get; set; }
        public string driver_car_year { get; set; }
        public string driver_car_model { get; set; }
        public string driver_car_make { get; set; }
        public string driver_insurance_carrier { get; set; }
        public string driver_insurance_num { get; set; }
        public string driver_insurance_exp_date { get; set; }
        public string contact_name { get; set; }
        public string contact_phone { get; set; }
        public string contact_relation { get; set; }
        public string bank_acc_info { get; set; }
        public string bank_routing_info { get; set; }
        public string password { get; set; }
        public string mobile_access_token { get; set; }
        public string mobile_refresh_token { get; set; }
        public string user_access_token { get; set; }
        public string user_refresh_token { get; set; }
        public string social_id { get; set; }
        public string social { get; set; }

        public SignUp()
        {
            first_name = "NULL";
            last_name = "NULL";
            business_uid = "NULL";
            referral_source = "NULL";
            driver_hours = "NULL";
            street = "NULL";
            unit = "NULL";
            city = "NULL";
            state = "NULL";
            zipcode = "NULL";
            latitude = "NULL";
            longitude = "NULL";
            email = "NULL";
            phone = "NULL";
            ssn = "NULL";
            license_num = "NULL";
            license_exp = "NULL";
            driver_car_year = "NULL";
            driver_car_model = "NULL";
            driver_car_make = "NULL";
            driver_insurance_carrier = "NULL";
            driver_insurance_num = "NULL";
            driver_insurance_exp_date = "NULL";
            contact_name = "NULL";
            contact_phone = "NULL";
            contact_relation = "NULL";
            bank_acc_info = "NULL";
            bank_routing_info = "NULL";
            password = "NULL";
            mobile_access_token = "NULL";
            mobile_refresh_token = "NULL";
            user_access_token = "NULL";
            user_refresh_token = "NULL";
            social_id = "NULL";
            social = "NULL";
        }


        public async Task<bool> FastSignUp(SignUpAccount account)
        {
            var client = new HttpClient();
            var content = new MultipartFormDataContent();
            var scheduleToSubmit = new ScheduleToSubmit();

    
            if(account.platform == "DIRECT")
            {
                content.Add(new StringContent("", Encoding.UTF8), "first_name");
                content.Add(new StringContent("", Encoding.UTF8), "last_name");
                content.Add(new StringContent(account.password, Encoding.UTF8), "password");
                content.Add(new StringContent(account.email, Encoding.UTF8), "email");
                content.Add(new StringContent("NULL", Encoding.UTF8), "social");
                content.Add(new StringContent("FALSE", Encoding.UTF8), "mobile_access_token");
                content.Add(new StringContent("FALSE", Encoding.UTF8), "mobile_refresh_token");
                content.Add(new StringContent("FALSE", Encoding.UTF8), "user_access_token");
                content.Add(new StringContent("FALSE", Encoding.UTF8), "user_refresh_token");
                content.Add(new StringContent("NULL", Encoding.UTF8), "social_id");
            }
            else
            {
                content.Add(new StringContent(account.firstName, Encoding.UTF8), "first_name");
                content.Add(new StringContent(account.lastName, Encoding.UTF8), "last_name");
                content.Add(new StringContent("", Encoding.UTF8), "password");
                content.Add(new StringContent(account.email, Encoding.UTF8), "email");
                content.Add(new StringContent(account.platform, Encoding.UTF8), "social");
                content.Add(new StringContent(account.accessToken, Encoding.UTF8), "mobile_access_token");
                content.Add(new StringContent(account.refreshToken, Encoding.UTF8), "mobile_refresh_token");
                content.Add(new StringContent("FALSE", Encoding.UTF8), "user_access_token");
                content.Add(new StringContent("FALSE", Encoding.UTF8), "user_refresh_token");
                content.Add(new StringContent(account.socialID, Encoding.UTF8), "social_id");
            }

            // CONTENT, NAME

            content.Add(new StringContent("NULL", Encoding.UTF8), "business_uid");
            content.Add(new StringContent("NULL", Encoding.UTF8), "referral_source");
            content.Add(new StringContent("[]", Encoding.UTF8), "driver_hours");
            content.Add(new StringContent("NULL", Encoding.UTF8), "street");
            content.Add(new StringContent("NULL", Encoding.UTF8), "unit");
            content.Add(new StringContent("NULL", Encoding.UTF8), "city");
            content.Add(new StringContent("NULL", Encoding.UTF8), "state");
            content.Add(new StringContent("NULL", Encoding.UTF8), "zipcode");
            content.Add(new StringContent("NULL", Encoding.UTF8), "latitude");
            content.Add(new StringContent("NULL", Encoding.UTF8), "longitude");
            content.Add(new StringContent("NULL", Encoding.UTF8), "email");
            content.Add(new StringContent("NULL", Encoding.UTF8), "phone");
            content.Add(new StringContent("NULL", Encoding.UTF8), "ssn");
            content.Add(new StringContent("NULL", Encoding.UTF8), "license_num");
            content.Add(new StringContent("NULL", Encoding.UTF8), "license_exp");
            content.Add(new StringContent("NULL", Encoding.UTF8), "driver_car_year");
            content.Add(new StringContent("NULL", Encoding.UTF8), "driver_car_model");
            content.Add(new StringContent("NULL", Encoding.UTF8), "driver_car_make");
            content.Add(new StringContent("NULL", Encoding.UTF8), "driver_insurance_carrier");
            content.Add(new StringContent("NULL", Encoding.UTF8), "driver_insurance_num");
            content.Add(new StringContent("NULL", Encoding.UTF8), "driver_insurance_exp_date");
            content.Add(new StringContent("NULL", Encoding.UTF8), "contact_name");
            content.Add(new StringContent("NULL", Encoding.UTF8), "contact_phone");
            content.Add(new StringContent("NULL", Encoding.UTF8), "contact_relation");
            content.Add(new StringContent("NULL", Encoding.UTF8), "bank_acc_info");
            content.Add(new StringContent("NULL", Encoding.UTF8), "bank_routing_info");

            var array = new byte[0];
            var temp = new ByteArrayContent(array);

            // CONTENT, NAME, FILENAME
            content.Add(temp, "driver_insurance_picture", "product_image.png");

            var request = new HttpRequestMessage();

            request.RequestUri = new Uri(Constant.SignUpUrl);
            request.Method = HttpMethod.Post;
            request.Content = content;

            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var contentString = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<FastSignUpAccount>(contentString);
                Debug.WriteLine("contentString: " + contentString);

                DateTime today = DateTime.Now;
                DateTime expDate = today.AddDays(Constant.days);

                user = new User();
                user.id = data.result.driver_uid;
                user.sessionTime = expDate;
                user.email = "";
                user.socialId = "";
                user.platform = "";
                user.route_id = "";


                return true;
            }
            return false;
        }

    }
}
