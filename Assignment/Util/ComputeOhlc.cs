using Assignment.Helpers;
using Assignment.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment.Util
{
    class ComputeOhlc
    {
        #region Members
        List<Trade> trades = new List<Trade>();
        Ohlc _previousOhlc;
        String _previousStockName = String.Empty;
        #endregion

        #region Public Methods
        /// <summary>
        /// This method processes the ticks data and calculates the OHLC for the stocks within the Bar interval
        /// </summary>
        /// <param name="trade"></param>
        /// <param name="bar_num"></param>
        /// <returns></returns>
        public async Task<Ohlc> FetchBarStats(Trade trade, int bar_num)
        {
             Ohlc ohlc = new Ohlc();

            //Check if its the first request when program starts
            if (_previousOhlc == null)
            {
                ohlc.open = ohlc.high = ohlc.low = trade.Price;
                ohlc.close = 0; // The close value will be 0 when its the first trade.
                ohlc.symbol = trade.StockName;
                ohlc.volume = trade.QuantityTraded;
                ohlc.tradeInTime = trade.TradeInDateTime;
                ohlc.bar_num = bar_num;
                ohlc.tradePrice = trade.Price;
                _previousOhlc = ohlc;
                _previousStockName = trade.StockName;
                ohlc.bar_num = bar_num;
                BarData.barOutput.Add(ohlc);
            }
            else
            {

                //Check if the current stock name is same as the previous trade record
                if (!_previousStockName.Equals(trade.StockName))
                {
                    _previousOhlc = BarData.barOutput.Where<Ohlc>(w => w.symbol == trade.StockName).FirstOrDefault();

                    //If the stock name occured first time, add to the output list
                    if (_previousOhlc == null)
                    {
                        ohlc.open = ohlc.high = ohlc.low = trade.Price;
                        ohlc.close = 0; // If the stock is listed first time within the bar
                        ohlc.symbol = trade.StockName;
                        ohlc.volume = trade.QuantityTraded;
                        ohlc.tradeInTime = trade.TradeInDateTime;
                        ohlc.bar_num = bar_num;
                        ohlc.tradePrice = trade.Price;
                        _previousOhlc = ohlc;
                        _previousStockName = trade.StockName;
                        ohlc.bar_num = bar_num;
                        BarData.barOutput.Add(ohlc);
                    }
                }
                else
                {
                    Ohlc _stockDetail = BarData.barOutput.Where(w => w.symbol == _previousStockName).FirstOrDefault();
                    _stockDetail.low = trade.Price < _previousOhlc.low ? trade.Price : _previousOhlc.low;
                    _stockDetail.high = trade.Price > _previousOhlc.high ? trade.Price : _previousOhlc.high;
                    _stockDetail.volume = ohlc.volume + trade.QuantityTraded;
                    _stockDetail.symbol = trade.StockName;
                    _stockDetail.tradeInTime = trade.TradeInDateTime;
                    _stockDetail.bar_num = bar_num;
                    _stockDetail.tradePrice = trade.Price;
                    _previousOhlc = _stockDetail;
                    _previousStockName = trade.StockName;
                }

            }

            return ohlc;
        
        }

        #endregion

    }

}
