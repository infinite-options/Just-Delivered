using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using JustDelivered.Config;
using JustDelivered.Interfaces;
using JustDelivered.LogIn.Classes;
using Newtonsoft.Json;
using Xamarin.Forms;
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
        public string driver_uid { get; set; }

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
            driver_uid = "NULL";
        }


        public async Task<bool> FastSignUp(SignUpAccount account)
        {
            var client = new HttpClient();
            var content = new MultipartFormDataContent();

            if(account.platform == "DIRECT")
            {
                content.Add(new StringContent("NULL", Encoding.UTF8), "first_name");
                content.Add(new StringContent("NULL", Encoding.UTF8), "last_name");
                content.Add(new StringContent(account.password, Encoding.UTF8), "password");
                content.Add(new StringContent(account.email, Encoding.UTF8), "email");
                content.Add(new StringContent("NULL", Encoding.UTF8), "social");
                content.Add(new StringContent("NULL", Encoding.UTF8), "mobile_access_token");
                content.Add(new StringContent("NULL", Encoding.UTF8), "mobile_refresh_token");
                content.Add(new StringContent("NULL", Encoding.UTF8), "user_access_token");
                content.Add(new StringContent("NULL", Encoding.UTF8), "user_refresh_token");
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
            content.Add(new StringContent(GetVersion(), Encoding.UTF8), "referral_source");
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
                user.platform = account.socialID;
                user.route_id = "";


                return true;
            }
            return false;
        }

        string GetVersion()
        {
            string result = "NULL";
            try
            {
                var versionString = DependencyService.Get<IAppVersionAndBuild>().GetVersionNumber();
                var buildString = DependencyService.Get<IAppVersionAndBuild>().GetBuildNumber();

                result = "Version: " + versionString + ", Build: " + buildString;
            }
            catch
            {

            }

            return result;
        }

        public async Task<bool> UpdateUserProfile(Dictionary<string, object> account)
        {
            bool result = false;
            var client = new HttpClient();
            var content = new MultipartFormDataContent();

            if ((string)account["platform"] == "DIRECT")
            {
                content.Add(new StringContent("", Encoding.UTF8), "password");
                content.Add(new StringContent("NULL", Encoding.UTF8), "social");
                content.Add(new StringContent("FALSE", Encoding.UTF8), "mobile_access_token");
                content.Add(new StringContent("FALSE", Encoding.UTF8), "mobile_refresh_token");
                content.Add(new StringContent("FALSE", Encoding.UTF8), "user_access_token");
                content.Add(new StringContent("FALSE", Encoding.UTF8), "user_refresh_token");
                content.Add(new StringContent("NULL", Encoding.UTF8), "social_id");
            }
            else
            {
                content.Add(new StringContent("", Encoding.UTF8), "password");
                content.Add(new StringContent((string)account["platform"] == null ? "NULL" : (string)account["platform"], Encoding.UTF8), "social");
                content.Add(new StringContent("FALSE", Encoding.UTF8), "mobile_access_token");
                content.Add(new StringContent("FALSE", Encoding.UTF8), "mobile_refresh_token");
                content.Add(new StringContent("FALSE", Encoding.UTF8), "user_access_token");
                content.Add(new StringContent("FALSE", Encoding.UTF8), "user_refresh_token");
                content.Add(new StringContent((string)account["social_id"] == null ? "NULL" : (string)account["social_id"], Encoding.UTF8), "social_id");
            }

            // CONTENT, NAME

            content.Add(new StringContent((string)account["driver_uid"] == null ? "NULL" : (string)account["driver_uid"], Encoding.UTF8), "driver_uid");
            content.Add(new StringContent((string)account["firstName"]== null? "NULL":(string)account["firstName"], Encoding.UTF8), "first_name");
            content.Add(new StringContent((string)account["lastName"] == null ? "NULL" : (string)account["lastName"], Encoding.UTF8), "last_name");
            content.Add(new StringContent((string)account["organizations"] == null ? "NULL" : (string)account["organizations"], Encoding.UTF8), "business_uid");
            content.Add(new StringContent((string)account["referal"] == null ? "NULL" : (string)account["referal"], Encoding.UTF8), "referral_source");
            content.Add(new StringContent("[]", Encoding.UTF8), "driver_hours");
            content.Add(new StringContent((string)account["street"] == null ? "NULL" : (string)account["street"], Encoding.UTF8), "street");
            content.Add(new StringContent((string)account["unit"] == null ? "NULL" : (string)account["unit"], Encoding.UTF8), "unit");
            content.Add(new StringContent((string)account["city"] == null ? "NULL" : (string)account["city"], Encoding.UTF8), "city");
            content.Add(new StringContent((string)account["state"] == null ? "NULL" : (string)account["state"], Encoding.UTF8), "state");
            content.Add(new StringContent((string)account["zipcode"] == null ? "NULL" : (string)account["zipcode"], Encoding.UTF8), "zipcode");
            content.Add(new StringContent((string)account["latitude"] == null ? "NULL" : (string)account["latitude"], Encoding.UTF8), "latitude");
            content.Add(new StringContent((string)account["longitude"] == null ? "NULL" : (string)account["longitude"], Encoding.UTF8), "longitude");
            content.Add(new StringContent((string)account["email"] == null ? "NULL" : (string)account["email"], Encoding.UTF8), "email");
            content.Add(new StringContent((string)account["phoneNumber"] == null ? "NULL" : (string)account["phoneNumber"], Encoding.UTF8), "phone");
            content.Add(new StringContent((string)account["ssNumber"] == null ? "NULL" : (string)account["ssNumber"], Encoding.UTF8), "ssn");
            content.Add(new StringContent((string)account["driveLicenseNumber"] == null ? "NULL" : (string)account["driveLicenseNumber"], Encoding.UTF8), "license_num");
            content.Add(new StringContent((string)account["driveLicenseExperirationDate"] == null ? "NULL" : (string)account["driveLicenseExperirationDate"], Encoding.UTF8), "license_exp");
            content.Add(new StringContent((string)account["carYear"] == null ? "NULL" : (string)account["carYear"], Encoding.UTF8), "driver_car_year");
            content.Add(new StringContent((string)account["carModel"] == null ? "NULL" : (string)account["carModel"], Encoding.UTF8), "driver_car_model");
            content.Add(new StringContent((string)account["carMake"] == null ? "NULL" : (string)account["carMake"], Encoding.UTF8), "driver_car_make");
            content.Add(new StringContent((string)account["insuranceCarrier"] == null ? "NULL" : (string)account["insuranceCarrier"], Encoding.UTF8), "driver_insurance_carrier");
            content.Add(new StringContent((string)account["insuranceNumber"] == null ? "NULL" : (string)account["insuranceNumber"], Encoding.UTF8), "driver_insurance_num");
            content.Add(new StringContent((string)account["insuranceExpirationDate"] == null ? "NULL" : (string)account["insuranceExpirationDate"], Encoding.UTF8), "driver_insurance_exp_date");
            content.Add(new StringContent((string)account["emergencyPhoneNumber"] == null ? "NULL" : (string)account["emergencyPhoneNumber"], Encoding.UTF8), "contact_phone");
            content.Add(new StringContent((string)account["emergencyRelationship"] == null ? "NULL" : (string)account["emergencyRelationship"], Encoding.UTF8), "contact_relation");
            content.Add(new StringContent((string)account["accountNumber"] == null ? "NULL" : (string)account["accountNumber"], Encoding.UTF8), "bank_acc_info");
            content.Add(new StringContent((string)account["routingNumber"] == null ? "NULL" : (string)account["routingNumber"], Encoding.UTF8), "bank_routing_info");

            var emergencyFirstName = (string)account["emergencyFirstName"] == null ? "NULL" : (string)account["emergencyFirstName"];
            var emergencyLastName = (string)account["emergencyLastName"] == null ? "NULL" : (string)account["emergencyLastName"];
            var emergencyContact = JsonConvert.SerializeObject(new EmergencyContact(emergencyFirstName, emergencyLastName));

            content.Add(new StringContent(emergencyContact, Encoding.UTF8), "contact_name");

            if (account["insuranceImage"] == null)
            {
                var array = new byte[0];
                var image = new ByteArrayContent(array);

                // CONTENT, NAME, FILENAME
                content.Add(image, "driver_insurance_picture", "product_image.png");
            }
            else
            {
                var image = new ByteArrayContent((byte[])account["insuranceImage"]);

                // CONTENT, NAME, FILENAME
                content.Add(image, "driver_insurance_picture", "product_image.png");
            }

            var request = new HttpRequestMessage();

            request.RequestUri = new Uri(Constant.SignUpUrl);
            request.Method = HttpMethod.Post;
            request.Content = content;

            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                result = true;

            }

            return result;
        }
    }
}
