using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;

namespace OHLC_Client
{
    class Program
    {
        /// <summary>
        /// TO DO : Client should be able to subscribe the ohlc data by passing stock name
        /// Currently and empty string is passed. This will perform ohlc analysis on all the feed.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            var connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5000/ohlchub")
                .Build();

            connection.StartAsync().Wait();
            connection.InvokeCoreAsync("SendMessage", args: new[]{ "subscribe", "", "15"});
            connection.On("ReceiveMessage", (string eventname, string stockname) =>
             {
                 Console.WriteLine("Running Analyzer on Server " + stockname);
            });

            Console.ReadLine();
        }
    }
}
