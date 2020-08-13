using System;
using System.Collections.Generic;
using Plugin.Connectivity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryApp.Models
{
    public class Elements
    {
        public int route_id { get; set; }
        public string name { get; set; }
        public string house_address { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zipcode { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }

        public string parsedPhone
        {
            get
            {
                if (phone.Length == 10)
                {
                    return phone;
                }
                else if (phone.Length == 12)
                {
                    string formatedPhone = "" + phone.Substring(2, 3) + phone.Substring(5, 3) + phone.Substring(8, 4);
                    return formatedPhone;
                }
                else
                {
                    string noPhoneNumber = "Phone # Not Available";
                    return noPhoneNumber;
                }
            }
        }

        public string firstNameAndFirstLetterLastName
        {
            get
            {
                string formattedName = "";
                string n = name;
                int i = 0;
                while((int)n[i]!=(int)' ')
                {
                    formattedName += n[i];
                    i++;
                }
                int j = i + 1;
                if(j < n.Length)
                {
                    formattedName += ". ";
                    formattedName += n[j];
                }
                return formattedName;
            }
        }
    }

    public class ServingNowList
    {
        public string message { get; set; }
        public IList<Elements> result { get; set; }
    }
}
