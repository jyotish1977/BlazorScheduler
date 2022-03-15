using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace BlazorScheduler.Components
{
    public class BlazSelectBarItem : BlazComponent
    {
        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        [Parameter]
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        [Parameter]
        public object Value { get; set; }

        BlazCommon _selectBar;

        /// <summary>
        /// Gets or sets the select bar.
        /// </summary>
        /// <value>The select bar.</value>
        [CascadingParameter]
        public BlazCommon SelectBar
        {
            get
            {
                return _selectBar;
            }
            set
            {
                if (_selectBar != value)
                {
                    _selectBar = value;
                    _selectBar.AddItem(this);
                }
            }
        }

        /// <summary>
        /// Disposes this instance.
        /// </summary>
        public void Dispose()
        {
            base.Dispose();
            SelectBar?.RemoveItem(this);
        }

        internal void SetText(string value)
        {
            Text = value;
        }

        internal void SetValue(object value)
        {
            Value = value;
        }
    }
}
