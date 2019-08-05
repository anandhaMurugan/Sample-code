using System;
namespace Helseboka.iOS.Legetimer.View.TableViewCell
{
    public class LegetimerCellModelData
    {

		public string legetimerHeading { get; set; }

        public string symptomDescription { get; set; }

		public string date { get; set; }

		public string time { get; set; }

		public bool isfeedBackNeeded { get; set; }

       
        public LegetimerCellModelData( string heading,string description, string legeTimerdate, string visitTime, bool isfeedBackRequired)
        {
			legetimerHeading = heading;
			symptomDescription = description;
			date = legeTimerdate;
			time = visitTime;
			isfeedBackNeeded = isfeedBackRequired;
        }
    }
}
