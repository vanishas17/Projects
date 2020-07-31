# Projects
Description

The code analyses OHLC for stocks for 15 second bar intervals and displays in the console. The bar interval is calculated based on the TS2 value in the feed file for the tick data.

The first tick in the feed file will initialize the bar interval for 15 seconds and set the bar_num 1. Once the bar closes, a new bar interval will b started and the process repeats.
Every stock that falls in the bar interval is analyzed for its ohlc values. The output shown will be stock wise ohlc values for all bar intervals. The bar_num will be incremented by 1 every time the bar interval changes

Implementation

The application is implemented using producer consumer pattern. Both Producer and Consumer run on different threads.
The Producer(Reader) task will read all the feed data from the trade.json file and fill the BufferBlock.
The Consumer(Processor) task will process the data tick by tick and calculate the ohlc and display the output for every 15 second interval

For websocket client server communications, SignalR nuget package is used.

Code Structure

The solution contains two projects. OHLC and OHLC_Client
OHLC is the server that performs the analysis logic
OHLC_Client is a client which can subscribe to the server

Currently the implementation supports creating a websocket connection and triggering the analyzer. Client subscription to server is pending.

Execution

Both the projects are set as startup projects and should be executed together.

Note: Unit test cases are not included.
