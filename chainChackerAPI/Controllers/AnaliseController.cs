using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace chainChackerAPI

{
    [ApiController]
    [Route("[controller]")]
    public class AnaliseController : ControllerBase
    {
        [HttpGet(Name = "GetTransactionInfo")]
        public IEnumerable<ChainSaver> Get()
        {
            //import libruary

            //create crawler class 
            HtmlWeb web = new HtmlWeb();
            //download webPage
            List<ChainSaver> chaiList = new List<ChainSaver>();
            HtmlDocument doc = web.Load("https://bscscan.com/tx/0x2a3259c9e685fd554e50676c227174223cc4d45aa045975d9ab313254caf0242");
            var walletHash = doc.GetElementbyId("spanFromAdd").InnerText;
            for (int i = 1; i < 5; i++)
            {
                doc = web.Load($"https://bscscan.com/txs?a={walletHash}&p={i}");

                //pull all of spans which consist 'hash'
                var HeaderFrom = doc.DocumentNode.SelectNodes("//tr");
                for (int j = 0; j < HeaderFrom.Count; j++)
                {

                    var tr = HeaderFrom[j];
                    HtmlNodeCollection childNodes = tr.ChildNodes;
                    ChainSaver showList = new ChainSaver();
                    showList.hash = childNodes[1].InnerText;
                    showList.trasactionType = childNodes[2].InnerText;
                    if (DateTime.TryParse(childNodes[4].InnerText, out DateTime result))
                    {
                        showList.date = result;
                    }
                    showList.from = childNodes[6].InnerText;
                    showList.to = childNodes[8].InnerText;
                    showList.value = childNodes[9].InnerText;
                    chaiList.Add(showList);
                }
                Thread.Sleep(1500);
            }
            return (chaiList);

        }

       


        }
    }


//Get info by transaction
//https://bscscan.com/tx/0x2a3259c9e685fd554e50676c227174223cc4d45aa045975d9ab313254caf0242
//extract wallet
//Get all transaction
//https://bscscan.com/txs?a=0x3f349bbafec1551819b8be1efea2fc46ca749aa1
//Find our transaction and nearest

