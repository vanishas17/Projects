using Assignment.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks.Dataflow;

namespace Assignment.Helpers
{
    /// <summary>
    /// Global output object
    /// </summary>
     static class Common
    {
        public static List<Ohlc> barOutput = new List<Ohlc>();

        public static BufferBlock<Trade> bufferBlock = new BufferBlock<Trade>(
               new DataflowBlockOptions { BoundedCapacity = 10000 });
    }

 }
