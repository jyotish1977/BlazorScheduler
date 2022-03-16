using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace BlazorScheduler.Components
{
    public partial class BlazWeekView : SchedulerViewBase
    {
        public override string Icon => "calendar_view_week";

        [Parameter]
        public override string Text { get; set; } = "Week";

        [Parameter]
        public override string JobId { get; set; } = "";

        [Parameter]
        public override string Name { get; set; } = "";

        [Parameter]
        public string TimeFormat { get; set; } = "h tt";

        [Parameter]
        public TimeSpan StartTime { get; set; } = TimeSpan.FromHours(8);

        [Parameter]
        public TimeSpan EndTime { get; set; } = TimeSpan.FromHours(24);

        [Parameter]
        public int MinutesPerSlot { get; set; } = 30;
        /// <inheritdoc />
        public override DateTime StartDate
        {
            get
            {
                return Scheduler.CurrentDate.Date.StartOfWeek();
            }
        }

        public override DateTime EndDate
        {
            get
            {
                return StartDate.EndOfWeek().AddDays(1);
            }
        }

        public override string Title
        {
            get
            {
                return $"{StartDate.ToString(Scheduler.Culture.DateTimeFormat.ShortDatePattern)} - {StartDate.EndOfWeek().ToString(Scheduler.Culture.DateTimeFormat.ShortDatePattern)}";
            }
        }

        public override DateTime Next()
        {
            return Scheduler.CurrentDate.Date.AddDays(7);
        }

        public override DateTime Prev()
        {
            return Scheduler.CurrentDate.Date.AddDays(-7);
        }
    }
}
