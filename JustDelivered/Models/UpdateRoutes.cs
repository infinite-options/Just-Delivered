using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace JustDelivered.Models
{
    public class UpdateRoutes
    {
        public string route_id {get;set;}
        public IList<string> purchase_uids { get; set; }

        public UpdateRoutes()
        {
            
        }

        public UpdateRoutes(string route_id, IList<string> purchase_uids)
        {
            this.route_id = route_id;
            this.purchase_uids = purchase_uids;
        }

        public void UpdateDeliveryStatus(string route_id, IList<string> purchase_uids)
        {
            try
            {
                var client = new HttpClient();
                var list = new UpdateRoutes(route_id, purchase_uids);

                var deliveryJSON = JsonConvert.SerializeObject(list);
                Debug.WriteLine("DELIVERY JSON: " + deliveryJSON);
                //var content = new StringContent(deliveryJSON, Encoding.UTF8, "application/json");

                //var RDSResponse = await client.PostAsync("https://0ig1dbpx3k.execute-api.us-west-1.amazonaws.com/dev/api/v2/UpdateDeliveryStatus", content);
                //Debug.WriteLine("UPDATE DELIVERY STATUS ENDPOINT " + RDSResponse.IsSuccessStatusCode);

            }
            catch (Exception ErrorUpdatingStatus)
            {
                Debug.WriteLine("Exception: " + ErrorUpdatingStatus.Message);
            }
        }
    }
}
