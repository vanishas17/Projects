using Assignment.Helpers;
using Assignment.Models;
using Assignment.Util;
using ConsoleTables;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Assignment
{
    public class OhlcHub :Hub
    {
        static ComputeOhlc c = new ComputeOhlc();
        static DateTime _interval_start_time = new DateTime();
        static DateTime _interval_end_time = new DateTime();
        static int count = 0;
        static int bar_num = 1;
        static ConsoleTable table = null;
        static string _inputStockName = String.Empty;
        static double _inputinterval = 15; //Setting default interval for bar to be 15 seconds


        private readonly ILogger _logger;
        public OhlcHub(ILogger<OhlcHub> logger)
        {
            _logger = logger;
        }

        public async Task SendMessage(string eventname, string stockname, string interval)
        {
            _inputStockName = stockname;
            _inputinterval = Convert.ToDouble(interval);

            
            Task _producer = Task.Run(Reader);
            Task _consumer = Task.Run(Processor);

            await Clients.All.SendAsync("ReceiveMessage", "subscribe", _inputStockName);

        }

        static async Task Reader()
        {
            var myJsonString = File.ReadAllText(@"Feed\trades.json");
            var tradeList = JsonConvert.DeserializeObject<List<Trade>>(myJsonString);

            foreach (var item in tradeList)
            {

                if (_inputStockName.Equals(String.Empty) || (!_inputStockName.Equals(String.Empty) && item.StockName.Equals(_inputStockName)))
                {
                    await Common.bufferBlock.SendAsync<Trade>(item);
                }
            }

        }
         async Task Processor()
        {

            while (await Common.bufferBlock.OutputAvailableAsync())
            {
                Trade newTrade = await Common.bufferBlock.ReceiveAsync();

                if (count == 0)//first record
                {
                    _interval_start_time = newTrade.TradeInDateTime;
                    _interval_end_time = _interval_start_time.AddSeconds(_inputinterval);

                    await c.FetchBarStats(newTrade, bar_num);
                    table = new ConsoleTable("Stock", "Open", "High", "Low", "Volume", "Close", "Trade-In Time");
                }
                else
                {
                    //Subsequent Records in the feed. If it belongs to the bar interval
                    if (newTrade.TradeInDateTime >= _interval_start_time && newTrade.TradeInDateTime <= _interval_end_time)
                    {
                        await c.FetchBarStats(newTrade, bar_num);
                    }
                    else
                    {

                        //Logic to print the previous block data
                        await PrintBarOutput();
                        Thread.Sleep(1000);
                        //increment the bar_num
                        bar_num++;

                        //ReSet interval start and end
                        _interval_start_time = _interval_end_time.AddSeconds(1);
                        _interval_end_time = _interval_start_time.AddSeconds(15);

                        await c.FetchBarStats(newTrade, bar_num);

                    }
                }

                count++;
            }
        }

        static Task PrintBarOutput()
        {
            if (Common.barOutput != null && Common.barOutput.Count > 0)
            {
                var result = Common.barOutput.Where(w => w.bar_num == bar_num);

                if (result != null)
                {
                    Console.WriteLine("\n================================== BAR {0} ====================================\n\n", bar_num);

                    if (table == null)
                    {
                        table = new ConsoleTable("Stock", "Open", "High", "Low", "Volume", "Close", "Trade-In Time");
                    }

                    foreach (var item in result)
                    {
                        table.AddRow(item.symbol, item.open, item.high, item.low, item.volume, item.tradePrice, item.tradeInTime.ToString());
                    }
                    table.Write(Format.Alternative);
                    table.Rows.Clear();
                }


                Common.barOutput.RemoveAll(w => w.bar_num == bar_num);

            }

            return Task.CompletedTask;
        }
    }
}