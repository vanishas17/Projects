using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment.Models
{
    /// <summary>
    /// Used to save the ohlc data calculated for the stocks
    /// </summary>
    public class Ohlc
    {
        public double open { get; set; }
        public double close { get; set; }
        public double high { get; set; }
        public double low { get; set; }
        public String symbol { get; set; }

        public double volume { get; set; }

        public int bar_num { get; set; }

        public DateTime tradeInTime { get; set; }

        public double tradePrice { get; set; }

    }
}
