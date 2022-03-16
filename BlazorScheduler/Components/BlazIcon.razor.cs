using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace BlazorScheduler.Components
{
    public partial class BlazIcon : BlazComponent
    {
        /// <summary>
        /// Gets or sets the icon.
        /// </summary>
        /// <value>The icon.</value>
        [Parameter]
        public string Icon { get; set; }

        /// <summary>
        /// Specifies the display style of the icon.
        /// </summary>
        [Parameter]
        public IconStyle? IconStyle { get; set; }

        /// <inheritdoc />
        protected override string GetComponentCssClass()
        {
            return $"rzi {(IconStyle.HasValue ? $"rzi-{IconStyle.Value.ToString().ToLower()} " : "")}d-inline-flex justify-content-center align-items-center";
        }
    }
}
