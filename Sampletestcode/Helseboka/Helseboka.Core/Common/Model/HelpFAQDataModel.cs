using System;

namespace Helseboka.Core.Common.Model
{
    public class HelpFAQDataModel
    {
		public String Title { get; set; }
        public String Description { get; set; }
        public Boolean IsExpanded { get; set; } = false;

		public HelpFAQDataModel() { }

		public HelpFAQDataModel(String title, String description)
        {
            Title = title;
            Description = description;
        }
    }
}
