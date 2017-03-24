using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application.Business.Erp.SupplyChain.Client.Basic.CommonClass
{
    public class TimeSlice
    {
        public TimeSlice(int year)
        {
            Slice = new List<KeyValuePair<int, DateTime[]>>();
            SetYear(year);
        }
        public int Year { get; set; }

        public void SetYear(int year)
        {
            Year = year;
            GetSlice();
        }

        public List<KeyValuePair<int, DateTime[]>> Slice { get; set; }

        public void GetSlice()
        {
            Slice.Clear();
            var startTime = new DateTime(Year, 1, 1);
            for (int i = 1; i < 13; i++)
            {
                var endTime = startTime.AddMonths(1).AddDays(-1);
                Slice.Add(new KeyValuePair<int, DateTime[]>(i, new DateTime[] { startTime, endTime }));
                startTime = startTime.AddMonths(1);
            }
        }
    }
}
