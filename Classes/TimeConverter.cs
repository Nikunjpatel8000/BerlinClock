using BerlinClock.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BerlinClock
{
    public sealed class TimeConverter : ITimeConverter
    {
        private const int SecondsPartIndex = 2;
        private const int MinutesPartIndex = 1;
        private const int HoursPartIndex = 0;
        private const int TotalParts = 3;
        private const string OffSign = "O";
        private const string YellowBulb = "Y";
        private const string RedBulb = "R";
        
        public string convertTime(string aTime)
        {
            var timeArray = aTime.Split(':');
            if (timeArray.Length != TotalParts)
                throw new ArgumentException("three part argument required e.g. 10:20:10");

            int[] timeparts = timeArray.Select(int.Parse).ToArray();

            if (timeparts[HoursPartIndex] > Hour.Max && timeparts[HoursPartIndex] < Hour.Min)
                throw new ArgumentException(string.Format("hour part of aTime must be between {0} and {1}.", Hour.Max, Hour.Min));

            if (timeparts[MinutesPartIndex] > Minute.Max && timeparts[MinutesPartIndex] < Minute.Min)
                throw new ArgumentException(string.Format("minute part of aTime must be between {0} and {1}.", Minute.Max, Minute.Min));

            if (timeparts[SecondsPartIndex] > Second.Max && timeparts[SecondsPartIndex] < Second.Min)
                throw new ArgumentException(string.Format("second part of aTime must be between {0} and {1}.", Second.Max, Second.Min));

            var builder = new StringBuilder();
            builder.Append(GetSecondsPart(timeparts[SecondsPartIndex]))
            .AppendLine()
            .Append(GetTopHoursPart(timeparts[HoursPartIndex]))
            .AppendLine()
            .Append(GetBottomHoursPart(timeparts[HoursPartIndex]))
            .AppendLine()
            .Append(GetTopMinutesPart(timeparts[MinutesPartIndex]))
            .AppendLine()
            .Append(GetBottomMinutesPart(timeparts[MinutesPartIndex]));
            return builder.ToString();        
        }

        private string GetSecondsPart(int number)
        {
            return number % 2 == 0 ? YellowBulb : OffSign;
        }

        private string GetTopHoursPart(int number)
        {
            return GetOnOff(4, GetTopNumberOfOnSigns(number));
        }

        private string GetBottomHoursPart(int number)
        {
            return GetOnOff(4, number % 5);
        }

        private string GetTopMinutesPart(int number)
        {
            return GetOnOff(11, GetTopNumberOfOnSigns(number), YellowBulb).Replace("YYY", "YYR");
        }

        private string GetBottomMinutesPart(int number)
        {
            return GetOnOff(4, number % 5, YellowBulb);
        }

        private string GetOnOff(int lamps, int onSigns)
        {
            return GetOnOff(lamps, onSigns, RedBulb);
        }
        private string GetOnOff(int lamps, int onSigns, String onSign)
        {
            var builder = new StringBuilder();
            for (int i = 0; i < onSigns; i++)
            {
                builder.Append(onSign);
            }
            for (int i = 0; i < (lamps - onSigns); i++)
            {
                builder.Append(OffSign);
            }
            return builder.ToString();
        }

        private int GetTopNumberOfOnSigns(int number)
        {
            return (number - (number % 5)) / 5;
        }

    }
}
