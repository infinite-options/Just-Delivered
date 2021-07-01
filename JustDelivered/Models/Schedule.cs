using System;
using System.Collections.ObjectModel;

namespace JustDelivered.Models
{
    public class Schedule
    {
        public string day { get; set; }
        public ObservableCollection<PickerTimeHour> startHour { get; set; }
        public ObservableCollection<PickerTimeMinute> startMinute { get; set; }
        public ObservableCollection<PickerTime> startTime { get; set; }
        public ObservableCollection<PickerTimeHour> endHour { get; set; }
        public ObservableCollection<PickerTimeMinute> endMinute { get; set; }
        public ObservableCollection<PickerTime> endTime { get; set; }

    }
}
