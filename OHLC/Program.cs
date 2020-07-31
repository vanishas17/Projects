using Assignment.Helpers;
using Assignment.Models;
using Assignment.Util;
using ConsoleTables;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
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
        

        #endregion

        static void Main(string[] args)
        {

            CreateWebHostBuilder(args).Build().Run();

            //bool showMenu = true;
            //while (showMenu)
            //{
            //    showMenu = MainMenu();
            //}
        }

        private static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .UseStartup<Startup>();

        private static bool MainMenu()
        {
            Console.WriteLine("OHLC Analyzer");
            Console.WriteLine("Please choose your option");
            Console.WriteLine("1. Run Analyzer");
            Console.WriteLine("2. Exit");
            Console.Write("\r\nSelect an option: ");

            switch (Console.ReadLine().Trim())
            {
                case "1":
                    RunAnalyzer();
                    return true;
                case "2":                    
                    return false;
                case "":
                    return false;
                default:
                    return true;
            }
        }

        private static void RunAnalyzer()
        {
            //Task Producer = Task.Run(Reader);
            //Task Consumer = Task.Run(Processor);
        }

        
    }
}

