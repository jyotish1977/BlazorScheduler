using DemoApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DemoApp.Services
{
    public class AppointmentService
    {
        public IEnumerable<AppointmentDto> GetAppointments(DateTime start, DateTime end)
        {
            return AllAppointments
                .Where(x => x.Start.Date <= end && start <= x.End.Date);
        }

        private readonly List<AppointmentDto> AllAppointments = new()
        {
            new AppointmentDto { Start = DateTime.Now, End = DateTime.Now.AddHours(1), Color = "yellow" },
            new AppointmentDto { Start = DateTime.Now.AddDays(3), End = DateTime.Now.AddDays(3).AddHours(1), Color = "red" },

            new AppointmentDto { Start = DateTime.Today, End = DateTime.Today.AddDays(1), Color = "yellow" },

            new AppointmentDto { Start = new DateTime(2021, 7, 9, 12, 00, 00), End = new DateTime(2021, 7, 9,11,30,00), Color = "green" },
            new AppointmentDto { Start = DateTime.Today.AddDays(4), End = DateTime.Today.AddDays(14), Color = "pink" },
            new AppointmentDto { Start = DateTime.Today.AddDays(1), End = DateTime.Today.AddDays(1), Color = "orange" },
            new AppointmentDto { Start = DateTime.Today.AddDays(1), End = DateTime.Today.AddDays(1), Color = "orange" },
            new AppointmentDto { Start = DateTime.Today.AddDays(1), End = DateTime.Today.AddDays(1), Color = "orange" },
            new AppointmentDto { Start = DateTime.Today.AddDays(1), End = DateTime.Today.AddDays(1), Color = "orange" },
            new AppointmentDto { Start = DateTime.Today.AddDays(1), End = DateTime.Today.AddDays(1), Color = "orange" },
            new AppointmentDto { Start = DateTime.Today.AddDays(1), End = DateTime.Today.AddDays(1), Color = "orange" },
            new AppointmentDto { Start = DateTime.Today.AddDays(1), End = DateTime.Today.AddDays(1), Color = "orange" },

            new AppointmentDto { Start = new DateTime(DateTime.Today.Year, 1, 1,11,30,00), End = new DateTime(DateTime.Today.Year, 1, 1,11,00,00), Color = "blue" },
            new AppointmentDto { Start = new DateTime(DateTime.Today.Year, 2, 2,10,30,00), End = new DateTime(DateTime.Today.Year, 2, 2,10,10,00), Color = "blue" },
            new AppointmentDto { Start = new DateTime(DateTime.Today.Year, 2, 14,10,20,00), End = new DateTime(DateTime.Today.Year, 2, 14,10,00,00), Color = "blue" },
            new AppointmentDto { Start = new DateTime(DateTime.Today.Year, 3, 17,10,15,15), End = new DateTime(DateTime.Today.Year, 3, 17,10,20,30), Color = "blue" },
            new AppointmentDto { Start = new DateTime(DateTime.Today.Year, 4, 22,09,30,00), End = new DateTime(DateTime.Today.Year, 4, 22,09,05,00), Color = "blue" },
            new AppointmentDto { Start = new DateTime(DateTime.Today.Year, 7, 4,08,30,00), End = new DateTime(DateTime.Today.Year, 7, 4,08,15,30), Color = "blue" },
            new AppointmentDto { Start = new DateTime(DateTime.Today.Year, 9, 11,08,10,00), End = new DateTime(DateTime.Today.Year, 9, 11,08,00,00), Color = "blue" },
            new AppointmentDto { Start = new DateTime(DateTime.Today.Year, 10, 31,07,55,00), End = new DateTime(DateTime.Today.Year, 10, 31,07,30,00), Color = "blue" },
            new AppointmentDto { Start = new DateTime(DateTime.Today.Year, 11, 11,07,00,00), End = new DateTime(DateTime.Today.Year, 11, 11,06,50,00), Color = "blue" },
            new AppointmentDto { Start = new DateTime(DateTime.Today.Year, 12, 7,06,40,00), End = new DateTime(DateTime.Today.Year, 12, 7,06,00,00), Color = "blue" },
            new AppointmentDto { Start = new DateTime(DateTime.Today.Year, 12, 25,05,59,00), End = new DateTime(DateTime.Today.Year, 12, 25,06,01,25), Color = "blue" },
            new AppointmentDto { Start = new DateTime(DateTime.Today.Year, 12, 31,06,24,00), End = new DateTime(DateTime.Today.Year, 12, 31,01,02,03), Color = "blue" },
        };
    }
}
