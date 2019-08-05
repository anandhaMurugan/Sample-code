using System;
namespace Helseboka.iOS.Legetimer.View.TableViewCell
{
    public class SymptomCellModel
    {
		public string symptomName { get; set; }

        public string symptomDescription { get; set; }

		public SymptomCellModel(string name, string value)
        {
			symptomName = name;
			symptomDescription = value;
        }
    }
}