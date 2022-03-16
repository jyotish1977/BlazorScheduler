using System;

namespace DemoApp.Models
{
    public class AppointmentDto
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Color { get; set; }
        public string Name { get; set; }
        public string JobId { get; set; }
        public string Status { get; set; }
        public string Text { get; set; }

    }
}
