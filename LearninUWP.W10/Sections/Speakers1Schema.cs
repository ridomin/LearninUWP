using System;
using AppStudio.DataProviders;

namespace LearninUWP.Sections
{
    /// <summary>
    /// Implementation of the Speakers1Schema class.
    /// </summary>
    public class Speakers1Schema : SchemaBase
    {

        public string Name { get; set; }

        public string Description { get; set; }

        public string Image { get; set; }

        public string Url { get; set; }
    }
}
