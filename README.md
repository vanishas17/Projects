# Projects
The code calculates OHLC for stocks for 15 second bar intervals and displays in console. Each bar interval is set as bar_num starting from 1.

The first tick in the feed file will initialize the bar interval for the bar_num 1. Subsequent intervals will be incremented to bar_num++
Every stock that falls in the bar interval is analyzed for its ohlc values. The output displays stock wise ohlc values for the bar interval.

The application is implemented using producer consumer pattern.
The Producer(Reader) task will read all the feed data from the trade.json file and fill the BufferBlock.
The Consumer(Processor) task will process the data tick by tick and calculate the ohlc and display the output for every 15 second interval
