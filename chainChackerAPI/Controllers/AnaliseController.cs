using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using chainChackerAPI.Helper;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace chainChackerAPI

{
    [ApiController]
    [Route("[controller]")]
    public class AnaliseController : ControllerBase
    {
        [HttpGet(Name = "GetTransactionInfo")]
        public int Get(string Hash)
        {
            //import libruary

            //create crawler class 
            HtmlWeb web = new HtmlWeb();
            //download webPage
            List<ChainTransaction> chaiList = new List<ChainTransaction>();
            HtmlDocument doc = web.Load($"https://bscscan.com/tx/{Hash}");
            var walletHash = doc.GetElementbyId("spanFromAdd").InnerText;
            int maxTransactionLimit = int.MaxValue;
            ChainTransaction transactionInfo = null;
            for (int i = 1;chaiList.Count<maxTransactionLimit; i++)
            {
                doc = web.Load($"https://bscscan.com/txs?a={walletHash}&p={i}");

                //pull all of spans which consist 'hash'
                var HeaderFrom = doc.DocumentNode.SelectNodes("//tr");
                if (HeaderFrom == null)
                {
                    continue;
                }

                for (int j = 0; j < HeaderFrom.Count && chaiList.Count < maxTransactionLimit; j++)
                {

                    var tr = HeaderFrom[j];
                    HtmlNodeCollection childNodes = tr.ChildNodes;
                    ChainTransaction showList = new ChainTransaction();
                    
                        showList.hash = childNodes[1].InnerText;

                    showList.trasactionType = childNodes[2].InnerText;
                    if (DateTime.TryParse(childNodes[4].InnerText, out DateTime result))
                    {
                        showList.date = result;
                    }
                    showList.from = childNodes[6].InnerText;
                    showList.to = childNodes[8].InnerText;
                    showList.value = childNodes[9].InnerText;
                    if (childNodes[1].InnerText == Hash)
                    {
                        maxTransactionLimit = chaiList.Count + 2;
                        transactionInfo = showList;
                    }
                    chaiList.Add(showList);
                }
                Thread.Sleep(1500);
            }
            var останняТранзакція = chaiList[chaiList.Count - 1];
            var firstInSubset = chaiList[Math.Max(chaiList.Count - 6, 0)];
            TimeSpan dif = останняТранзакція.date - firstInSubset.date;
            int score = 0;
            if (dif < TimeSpan.FromSeconds(10))
            {
                score += 20;
            }
            if (Helper.Helper.isHoliday(transactionInfo.date))
            {
                score += 10;
            }
            var valueAndCurrency = transactionInfo.value.Split(' ');
            double value = double.Parse(valueAndCurrency[0].Replace( '.', ','));
            if (value > 0.01)
            {
                score += 40;
            }
            return (score);

        }

       


        }
    }


//Get info by transaction
//https://bscscan.com/tx/0x2a3259c9e685fd554e50676c227174223cc4d45aa045975d9ab313254caf0242
//extract wallet
//Get all transaction
//https://bscscan.com/txs?a=0x3f349bbafec1551819b8be1efea2fc46ca749aa1
//Find our transaction and nearest

