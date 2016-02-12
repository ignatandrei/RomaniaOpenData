using System;

namespace ROPObjects
{

    public class TimePeriod
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }

    /// <summary>
    /// not a really good inheritance
    /// but it is enoguh to be programmed here
    /// </summary>
    public class SingleTimePeriod:TimePeriod
    {
        public SingleTimePeriod()
        {

        }
        /// <summary>
        /// string must be yyyy-MM-dd
        /// </summary>
        /// <param name="date">yyyy-MM-dd</param>
        public SingleTimePeriod(string date)
        {
            var dt = DateTime.ParseExact(date, "yyyy-MM-dd", null);
            this.Start = dt;
            this.End = dt;
        }
        public SingleTimePeriod(DateTime dt)
        {
            this.Start = dt;
            this.End = dt;
        }
    }
}