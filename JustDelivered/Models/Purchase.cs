using System;
using System.Collections.ObjectModel;
using static JustDelivered.Views.DeliveriesPage;

namespace JustDelivered.Models
{
    public class Purchase
    {

        public string pur_customer_uid { get; set; }
        public string pur_business_uid { get; set; }
        public ObservableCollection<DeliveryItem> items { get; set; }
        public string order_instructions { get; set; }
        public string delivery_instructions { get; set; }
        public string order_type { get; set; }
        public string delivery_first_name { get; set; }
        public string delivery_last_name { get; set; }
        public string delivery_phone_num { get; set; }
        public string delivery_email { get; set; }
        public string delivery_address { get; set; }
        public string delivery_unit { get; set; }
        public string delivery_city { get; set; }
        public string delivery_state { get; set; }
        public string delivery_zip { get; set; }
        public string delivery_latitude { get; set; }
        public string delivery_longitude { get; set; }
        public string purchase_notes { get; set; }
        public string start_delivery_date { get; set; }
        public string pay_coupon_id { get; set; }
        public string amount_due { get; set; }
        public string amount_discount { get; set; }
        public string amount_paid { get; set; }
        public string info_is_Addon { get; set; }
        public string cc_num { get; set; }
        public string cc_exp_date { get; set; }
        public string cc_cvv { get; set; }
        public string cc_zip { get; set; }
        public string charge_id { get; set; }
        public string payment_type { get; set; }
        public string subtotal { get; set; }
        public string service_fee { get; set; }
        public string delivery_fee { get; set; }
        public string driver_tip { get; set; }
        public string taxes { get; set; }
        public string ambassador_code { get; set; }

        public Purchase()
        {
            pur_customer_uid = "";
            pur_business_uid = "";
            items = new ObservableCollection<DeliveryItem>();
            order_instructions = "";
            delivery_instructions = "";
            order_type = "";
            delivery_first_name = "";
            delivery_last_name = "";
            delivery_phone_num = "";
            delivery_email = "";
            delivery_address = "";
            delivery_unit = "";
            delivery_city = "";
            delivery_state = "";
            delivery_zip = "";
            delivery_latitude = "";
            delivery_longitude = "";
            purchase_notes = "purchase_notes";
            start_delivery_date = "";
            pay_coupon_id = "";
            amount_due = "0.00";
            amount_discount = "0.00";
            amount_paid = "0.00";
            info_is_Addon = "FALSE";
            cc_num = "";
            cc_exp_date = "";
            cc_cvv = "";
            cc_zip = "";
            charge_id = "NONE";
            payment_type = "NONE";
            subtotal = "0.00";
            service_fee = "0.00";
            delivery_fee = "0.00";
            driver_tip = "0.00";
            taxes = "0.00";
            ambassador_code = "0.00";
        }
    }
}
