using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Assignment.Models;
using Newtonsoft.Json;

namespace Assignment
{
    class Program
    {
        static BufferBlock<Trade> bufferBlock = new BufferBlock<Trade>();
        static void Main(string[] args)
        {
            Console.WriteLine("Running OHLC Application!");
            Task.Run(Reader);
        }

        static async Task Reader()
        {
            var myJsonString = File.ReadAllText(@"TradeFeed\trades1.json");
            var tradeList = JsonConvert.DeserializeObject<List<Trade>>(myJsonString);

            foreach (var item in tradeList)
            {
                bufferBlock.Post<Trade>(item);
            }
        }
    }
}
