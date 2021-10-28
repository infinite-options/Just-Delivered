using System;
namespace JustDelivered.Models
{
    public class UpdateDriverSchedule
    {
        public ScheduleToSubmit driver_hours { get; set; }
        public string uid { get; set; }

        public UpdateDriverSchedule()
        {
            driver_hours = new ScheduleToSubmit();
            uid = "";
        }
    }
}
