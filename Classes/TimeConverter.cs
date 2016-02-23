using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BerlinClock
{
    public class TimeConverter : ITimeConverter
    {
        private const int SecondsPartIndex = 2;
        private const int MinutesPartIndex = 1;
        private const int HoursPartIndex = 0;
        private const int TotalParts = 3;

        public string convertTime(string aTime)
        {
            var timeArray = aTime.Split(':');
            if (timeArray.Length != TotalParts)
                throw new ArgumentException("three part argument required e.g. 10:20:10");

            int[] parts = timeArray.Select(x => int.Parse(x)).ToArray();

            if (parts[HoursPartIndex] > 24 && parts[HoursPartIndex] < 0)
                throw new ArgumentException("hour part of aTime must be between 0 and 24.");

            if (parts[MinutesPartIndex] > 60 && parts[MinutesPartIndex] < 0)
                throw new ArgumentException("minute part of aTime must be between 0 and 60.");

            if (parts[SecondsPartIndex] > 60 && parts[SecondsPartIndex] < 0)
                throw new ArgumentException("second part of aTime must be between 0 and 60.");

            var builder = new StringBuilder();
            builder.Append(GetSecondsPart(parts[SecondsPartIndex]))
            .AppendLine()
            .Append(GetTopHoursPart(parts[HoursPartIndex]))
            .AppendLine()
            .Append(GetBottomHoursPart(parts[HoursPartIndex]))
            .AppendLine()
            .Append(GetTopMinutesPart(parts[MinutesPartIndex]))
            .AppendLine()
            .Append(GetBottomMinutesPart(parts[MinutesPartIndex]));
            return builder.ToString();        
        }

        protected string GetSecondsPart(int number)
        {
            if (number % 2 == 0) return "Y";
            else return "O";
        }

        protected string GetTopHoursPart(int number)
        {
            return GetOnOff(4, GetTopNumberOfOnSigns(number));
        }

        protected string GetBottomHoursPart(int number)
        {
            return GetOnOff(4, number % 5);
        }

        protected string GetTopMinutesPart(int number)
        {
            return GetOnOff(11, GetTopNumberOfOnSigns(number), "Y").Replace("YYY", "YYR");
        }

        protected string GetBottomMinutesPart(int number)
        {
            return GetOnOff(4, number % 5, "Y");
        }

        private string GetOnOff(int lamps, int onSigns)
        {
            return GetOnOff(lamps, onSigns, "R");
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
                builder.Append("O");
            }
            return builder.ToString();
        }

        private int GetTopNumberOfOnSigns(int number)
        {
            return (number - (number % 5)) / 5;
        }

    }
}
