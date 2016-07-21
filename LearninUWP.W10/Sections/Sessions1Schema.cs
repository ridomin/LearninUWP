using System;
using AppStudio.DataProviders;

namespace LearninUWP.Sections
{
    /// <summary>
    /// Implementation of the Sessions1Schema class.
    /// </summary>
    public class Sessions1Schema : SchemaBase
    {

        public string Title { get; set; }

        public string Description { get; set; }

        public string Url { get; set; }

        public string Image { get; set; }

        public string Speaker { get; set; }

        public DateTime? SessionDate { get; set; }
    }
}
