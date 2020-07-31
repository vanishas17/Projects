using Assignment.Helpers;
using Assignment.Models;
using Assignment.Util;
using ConsoleTables;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Assignment
{
    class Program
    {
        #region Static Members
        static BufferBlock<Trade> bufferBlock = new BufferBlock<Trade>(
               new DataflowBlockOptions { BoundedCapacity = 10000 });
        static int count = 0;
        static int bar_num = 1;
        static ConsoleTable table = null;
        static string inputKey;

        #endregion

        static void Main(string[] args)
        {
            Task Producer = Task.Run(Reader);

            Task Consumer = Task.Run(Processor);

            Console.Read();
        }
        
        static async Task Reader()
        {
            var myJsonString = File.ReadAllText(@"Feed\trades.json");
            var tradeList = JsonConvert.DeserializeObject<List<Trade>>(myJsonString);

            foreach (var item in tradeList)
            {
                bufferBlock.SendAsync<Trade>(item);
            }

        }

        static async Task Processor()
        {
            ComputeOhlc c = new ComputeOhlc();

            DateTime _interval_start_time = new DateTime();
            DateTime _interval_end_time = new DateTime();

            while (await bufferBlock.OutputAvailableAsync())
            {
                Trade newTrade = await bufferBlock.ReceiveAsync();

                if (count == 0)//first record
                {
                    _interval_start_time = newTrade.TradeInDateTime;
                    _interval_end_time = _interval_start_time.AddSeconds(15);
                    //bar_num = 1;
                    c.FetchBarStats(newTrade, bar_num);
                    table = new ConsoleTable("Stock", "Open", "High", "Low", "Volume", "Close", "Time");
                } else
                {
                    //Subsequent Records in the feed. If it belongs to the bar interval
                    if (newTrade.TradeInDateTime >= _interval_start_time && newTrade.TradeInDateTime <= _interval_end_time)
                    {
                        c.FetchBarStats(newTrade, bar_num);
                    } else
                    {
                        //Logic to print the previous block data
                        await PrintBarOutput();
                        Thread.Sleep(100);
                        //increment the bar_num
                        bar_num++;
                        _interval_start_time = _interval_end_time.AddSeconds(1);
                        _interval_end_time = _interval_start_time.AddSeconds(15);
                        c.FetchBarStats(newTrade, bar_num);

                    }
                }
                
                count++;
            }
        }

        static async Task PrintBarOutput()
        {
            if (BarData.barOutput != null && BarData.barOutput.Count > 0)
            {
                var result = BarData.barOutput.Where(w => w.bar_num == bar_num);
                
                if (result != null)
                {
                    Console.WriteLine("================================== BAR {0} ====================================\n\n", bar_num);

                    if (table == null)
                    {
                        table = new ConsoleTable("Stock", "Open", "High", "Low", "Volume", "Close", "Time");
                    }

                    foreach (var item in result)
                    {
                        table.AddRow(item.symbol, item.open, item.high, item.low, item.volume, item.tradePrice, item.tradeInTime.ToString());
                    }
                    table.Write(Format.Alternative);
                    table.Rows.Clear();
                }

                BarData.barOutput.RemoveAll(w => w.bar_num == bar_num);

            }
        }

    }
}

