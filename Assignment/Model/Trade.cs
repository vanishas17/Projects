using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment.Models
{
    class Trade
    {
        [JsonProperty("sym")]
        public string StockName { get; set; }

        [JsonProperty("P")]
        public double Price { get; set; }

        [JsonProperty("Q")]
        public double QuantityTraded { get; set; }

        [JsonProperty("TS2")]
        public Int64 TimeStamp { get; set; }

        public DateTime TradeInDateTime
        {
            get
            {
                long nanoseconds = TimeStamp;
                DateTime epocTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

                DateTime result = epocTime.AddTicks(nanoseconds / 100);

                return DateTime.ParseExact(result.ToString("yyyy-MM-dd HH:mm:ss"), "yyyy-MM-dd HH:mm:ss", null);
            }
        }
    }
}
