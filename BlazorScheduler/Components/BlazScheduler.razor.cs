using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Linq.Dynamic.Core;
using System.Collections.ObjectModel;

namespace BlazorScheduler.Components
{
    public partial class BlazScheduler<TItem> : BlazComponent, IScheduler
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public RenderFragment<TItem> Template { get; set; }

        /// <summary>
        /// Gets or sets the data of RadzenScheduler. It will display an appointment for every item of the collection which is within the current view date range.
        /// </summary>
        /// <value>The data.</value>
        [Parameter]
        public IEnumerable<TItem> Data { get; set; }

        /// <summary>
        /// Specifies the property of <typeparamref name="TItem" /> which will set <see cref="AppointmentData.Start" />.
        /// </summary>
        /// <value>The name of the property. Must be a <c>DateTime</c> property.</value>
        [Parameter]
        public string StartProperty { get; set; }

        /// <summary>
        /// Specifies the property of <typeparamref name="TItem" /> which will set <see cref="AppointmentData.End" />.
        /// </summary>
        /// <value>The name of the property. Must be a <c>DateTime</c> property.</value>
        [Parameter]
        public string EndProperty { get; set; }

        private int selectedIndex { get; set; }

        /// <summary>
        /// Specifies the initially selected view.
        /// </summary>
        /// <value>The index of the selected.</value>
        [Parameter]
        public int SelectedIndex { get; set; }

        /// <summary>
        /// Gets or sets the text of the today button. Set to <c>Today</c> by default.
        /// </summary>
        /// <value>The today text.</value>
        [Parameter]
        public string TodayText { get; set; } = "Today";

        /// <summary>
        /// Gets or sets the initial date displayed by the selected view. Set to <c>DateTime.Today</c> by default.
        /// </summary>
        /// <value>The date.</value>
        [Parameter]
        public DateTime Date { get; set; } = DateTime.Today;

        /// <summary>
        /// Gets or sets the current date displayed by the selected view. Initially set to <see cref="Date" />. Changes during navigation.
        /// </summary>
        /// <value>The current date.</value>
        public DateTime CurrentDate { get; set; }

        /// <summary>
        /// Specifies the property of <typeparamref name="TItem" /> which will set <see cref="AppointmentData.Text" />.
        /// </summary>
        /// <value>The name of the property. Must be a <c>DateTime</c> property.</value>
        [Parameter]
        public string TextProperty { get; set; }

        [Parameter]
        public string JobIdProperty { get; set; }

        [Parameter]
        public string NameProperty { get; set; }

        [Parameter]
        public EventCallback<SchedulerSlotSelectEventArgs> SlotSelect { get; set; }


        [Parameter]
        public EventCallback<SchedulerAppointmentSelectEventArgs<TItem>> AppointmentSelect { get; set; }

        [Parameter]
        public Action<SchedulerAppointmentRenderEventArgs<TItem>> AppointmentRender { get; set; }

        [Parameter]
        public Action<SchedulerSlotRenderEventArgs> SlotRender { get; set; }

        /// <summary>
        /// A callback that will be invoked when the scheduler needs data for the current view. Commonly used to filter the
        /// data assigned to <see cref="Data" />.
        /// </summary>
        [Parameter]
        public EventCallback<SchedulerLoadDataEventArgs> LoadData { get; set; }

        IList<ISchedulerView> Views { get; set; } = new List<ISchedulerView>();

        ISchedulerView SelectedView
        {
            get
            {
                return Views.ElementAtOrDefault(selectedIndex);
            }
        }

        /// <inheritdoc />
        public IDictionary<string, object> GetAppointmentAttributes(AppointmentData item)
        {
            var args = new SchedulerAppointmentRenderEventArgs<TItem> { Data = (TItem)item.Data, Start = item.Start, End = item.End };

            AppointmentRender?.Invoke(args);

            return args.Attributes;
        }

        /// <inheritdoc />
        public IDictionary<string, object> GetSlotAttributes(DateTime start, DateTime end)
        {
            var args = new SchedulerSlotRenderEventArgs { Start = start, End = end, View = SelectedView };

            SlotRender?.Invoke(args);

            return args.Attributes;
        }

        /// <inheritdoc />
        public RenderFragment RenderAppointment(AppointmentData item)
        {
            if (Template != null)
            {
                TItem context = (TItem)item.Data;
                return Template(context);
            }

            return builder => builder.AddContent(0, string.Format("{0},{1},{2}",item.JobId,item.Name,item.Text));
        }

        /// <inheritdoc />
        public async Task SelectSlot(DateTime start, DateTime end)
        {
            await SlotSelect.InvokeAsync(new SchedulerSlotSelectEventArgs { Start = start, End = end });
        }

        /// <inheritdoc />
        public async Task SelectAppointment(AppointmentData data)
        {
            await AppointmentSelect.InvokeAsync(new SchedulerAppointmentSelectEventArgs<TItem> { Start = data.Start, End = data.End, Data = (TItem)data.Data });
        }

        /// <inheritdoc />
        public async Task AddView(ISchedulerView view)
        {
            if (!Views.Contains(view))
            {
                Views.Add(view);

                if (SelectedView == view)
                {
                    await InvokeLoadData();
                }

                StateHasChanged();
            }
        }

        /// <summary>
        /// Causes the current scheduler view to render. Enumerates the items of <see cref="Data" /> and creates instances of <see cref="AppointmentData" /> to
        /// display in the current view. Use it when <see cref="Data" /> has changed.
        /// </summary>
        public async Task Reload()
        {
            appointments = null;

            await InvokeLoadData();

            StateHasChanged();
        }

        /// <inheritdoc />
        public bool IsSelected(ISchedulerView view)
        {
            return selectedIndex == Views.IndexOf(view);
        }

        async Task OnChangeView(ISchedulerView view)
        {
            selectedIndex = Views.IndexOf(view);

            await InvokeLoadData();
        }

        async Task OnPrev()
        {
            CurrentDate = SelectedView.Prev();

            await InvokeLoadData();
        }

        async Task OnToday()
        {
            CurrentDate = DateTime.Now.Date;

            await InvokeLoadData();
        }

        async Task OnNext()
        {
            CurrentDate = SelectedView.Next();

            await InvokeLoadData();
        }

        /// <inheritdoc />
        public void RemoveView(ISchedulerView view)
        {
            Views.Remove(view);
        }

        /// <inheritdoc />
        protected override void OnInitialized()
        {
            CurrentDate = Date;
            selectedIndex = SelectedIndex;

            double height = 0;

            var style = CurrentStyle;

            if (style.ContainsKey("height"))
            {
                var pixelHeight = style["height"];

                if (pixelHeight.EndsWith("px"))
                {
                    height = Convert.ToDouble(pixelHeight.TrimEnd("px".ToCharArray()));
                }
            }

            if (height > 0)
            {
                heightIsSet = true;

                Height = height;
            }
        }

        IEnumerable<AppointmentData> appointments;
        DateTime rangeStart;
        DateTime rangeEnd;
        Func<TItem, DateTime> startGetter;
        Func<TItem, DateTime> endGetter;
        Func<TItem, string> textGetter;

        Func<TItem, string> jobIdGetter;
        Func<TItem, string> nameGetter;

        /// <inheritdoc />
        public override async Task SetParametersAsync(ParameterView parameters)
        {
            var needsReload = false;

            if (parameters.DidParameterChange(nameof(Date), Date))
            {
                CurrentDate = parameters.GetValueOrDefault<DateTime>(nameof(Date));
                needsReload = true;
            }

            if (parameters.DidParameterChange(nameof(SelectedIndex), SelectedIndex))
            {
                selectedIndex = parameters.GetValueOrDefault<int>(nameof(SelectedIndex));
                needsReload = true;
            }

            if (parameters.DidParameterChange(nameof(Data), Data))
            {
                appointments = null;
            }

            if (parameters.DidParameterChange(nameof(StartProperty), StartProperty))
            {
                startGetter = PropertyAccess.Getter<TItem, DateTime>(parameters.GetValueOrDefault<string>(nameof(StartProperty)));
            }

            if (parameters.DidParameterChange(nameof(EndProperty), EndProperty))
            {
                endGetter = PropertyAccess.Getter<TItem, DateTime>(parameters.GetValueOrDefault<string>(nameof(EndProperty)));
            }

            if (parameters.DidParameterChange(nameof(TextProperty), TextProperty))
            {
                textGetter = PropertyAccess.Getter<TItem, string>(parameters.GetValueOrDefault<string>(nameof(TextProperty)));
            }

            if (parameters.DidParameterChange(nameof(TextProperty), TextProperty))
            {
                textGetter = PropertyAccess.Getter<TItem, string>(parameters.GetValueOrDefault<string>(nameof(TextProperty)));
            }

            if (parameters.DidParameterChange(nameof(JobIdProperty), JobIdProperty))
            {
                jobIdGetter = PropertyAccess.Getter<TItem, string>(parameters.GetValueOrDefault<string>(nameof(JobIdProperty)));
            }


            if (parameters.DidParameterChange(nameof(NameProperty), NameProperty))
            {
                nameGetter = PropertyAccess.Getter<TItem, string>(parameters.GetValueOrDefault<string>(nameof(NameProperty)));
            }


            await base.SetParametersAsync(parameters);

            if (needsReload)
            {
                await InvokeLoadData();
            }
        }

        private async Task InvokeLoadData()
        {
            if (SelectedView != null)
            {
                await LoadData.InvokeAsync(new SchedulerLoadDataEventArgs { Start = SelectedView.StartDate, End = SelectedView.EndDate });
            }
        }

        /// <inheritdoc />
        public bool IsAppointmentInRange(AppointmentData item, DateTime start, DateTime end)
        {
            if (item.Start == item.End && item.Start >= start && item.End < end)
            {
                return true;
            }

            return item.End > start && item.Start < end;
        }

        /// <inheritdoc />
        public IEnumerable<AppointmentData> GetAppointmentsInRange(DateTime start, DateTime end)
        {
            if (Data == null)
            {
                return Array.Empty<AppointmentData>();
            }
            if (start == rangeStart && end == rangeEnd && appointments != null)
            {
                return appointments;
            }
            rangeStart = start;
            rangeEnd = end;
            var predicate = $"{EndProperty} >= @0 && {StartProperty} < @1";
            appointments = Data.AsQueryable()
                               .Where(predicate, start, end)
                               .ToList()
                               .Select(item => new AppointmentData { Start = startGetter(item), End = endGetter(item), 
                                   Text = textGetter(item), JobId = jobIdGetter(item), Name = nameGetter(item), Data = item });

            return appointments;
        }

        class Rect
        {
            public double Width { get; set; }
            public double Height { get; set; }
        }

        /// <inheritdoc />
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                var rect = await JSRuntime.InvokeAsync<Rect>("Radzen.createScheduler", Element, Reference);

                if (!heightIsSet)
                {
                    heightIsSet = true;
                    Resize(rect.Width, rect.Height);
                }
            }
        }

        /// <summary>
        /// Invoked from client-side via interop when the scheduler size changes.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        [JSInvokable]
        public void Resize(double width, double height)
        {
            var stateHasChanged = false;

            if (height != Height)
            {
                Height = height;
                stateHasChanged = true;
            }

            if (stateHasChanged)
            {
                StateHasChanged();
            }
        }

        /// <inheritdoc />
        public override void Dispose()
        {
            base.Dispose();

            if (IsJSRuntimeAvailable)
            {
                JSRuntime.InvokeVoidAsync("Radzen.destroyScheduler", Element);
            }
        }

        private bool heightIsSet = false;
        private double Height { get; set; } = 400; // Default height set from theme.
        double IScheduler.Height
        {
            get
            {
                return Height;
            }
        }
        /// <inheritdoc />
        protected override string GetComponentCssClass()
        {
            return $"rz-scheduler";
        }
        [Parameter] public RenderFragment Appointments { get; set; } = null!;

        private readonly ObservableCollection<Appointment> _appointments = new();
        private DotNetObjectReference<Scheduler> _objReference = null!;

        [Parameter] public Func<DateTime, DateTime, Task>? OnRequestNewData { get; set; }
        [Parameter] public Func<DateTime, DateTime, Task>? OnAddingNewAppointment { get; set; }
        [Parameter] public Func<DateTime, Task>? OnOverflowAppointmentClick { get; set; }

        [Parameter] public int MaxVisibleAppointmentsPerDay { get; set; } = 5;
        [Parameter] public bool EnableDragging { get; set; } = true;
        [Parameter] public bool EnableRescheduling { get; set; }
        [Parameter] public string ThemeColor { get; set; } = "aqua";
        public Appointment? DraggingAppointment { get; private set; }

    }
}
