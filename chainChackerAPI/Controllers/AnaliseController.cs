using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using chainChackerAPI.Helper;
using chainChackerAPI.Entities;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace chainChackerAPI

{
    [ApiController]
    [Route("[controller]")]
    public class AnaliseController : ControllerBase
    {
        [HttpGet("getMain/{page}")]
        public IEnumerable<ChainTransactionWE> GetMain(int page)
        {
            //import libruary

            //create crawler class 
            HtmlWeb web = new HtmlWeb();
            //download webPage
            List<ChainTransaction> chaiList = new List<ChainTransaction>();

            HtmlDocument doc = web.Load("https://bscscan.com/txs?p=" + page);
            //pull all of spans which consist 'hash'
            var HeaderFrom = doc.DocumentNode.SelectNodes("//tr");
            if (HeaderFrom == null)
                return chaiList.Select(t => new ChainTransactionWE(t, TransactionStatus.None));

            for (int j = 0; j < HeaderFrom.Count; j++)
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
                if (showList.hash != "")
                    chaiList.Add(showList);
            }
            Thread.Sleep(500);

            return chaiList.Select(t => new ChainTransactionWE(t, TransactionStatus.None));
        }


        [HttpGet("getByWallet/{address}/{page}")]
        public IEnumerable<ChainTransactionWE> GetByWallet(string address, int page)
        {
            List<ChainTransaction> chaiList = new List<ChainTransaction>();

            chaiList = ParsePage($"https://bscscan.com/txs?a={address}&p={page}");
            Thread.Sleep(500);

            return chaiList.Select(t => new ChainTransactionWE(t, TransactionStatus.None));
        }


        [HttpGet("getWalletStatus/{address}")]
        public TransactionStatus GetByWallet(string address)
        {
            List<ChainTransaction> chaiList = new List<ChainTransaction>();

            for (int i = 1; i <= 3; i++)
            {
                chaiList.AddRange(ParsePage($"https://bscscan.com/txs?a={address}&p={i}"));
                Thread.Sleep(500);
            }

            Dictionary<TransactionStatus, int> statusDict = new Dictionary<TransactionStatus, int>();

            for (int i = 0; i < (int)TransactionStatus.Danger; i++)
            {
                statusDict[(TransactionStatus)i] = 0;
            }

            foreach (ChainTransaction t in chaiList)
            {
                statusDict[CountScore(chaiList, t)]++;
                if (statusDict[TransactionStatus.Suspicious] > 30 || statusDict[TransactionStatus.Danger] > 1)
                    return TransactionStatus.Danger;
            }
            if (statusDict[TransactionStatus.Suspicious] > 10)
                return TransactionStatus.Suspicious;

            return TransactionStatus.Normal;
        }

        [HttpGet("getTransactionStatus/{hash}")]
        public TransactionStatus GetStatus(string hash)
        {
            try
            {
                //create crawler class 
                HtmlWeb web = new HtmlWeb();
                //download webPage
                List<ChainTransaction> chaiList = new List<ChainTransaction>();
                HtmlDocument doc = web.Load($"https://bscscan.com/tx/{hash}");
                var walletHash = doc.GetElementbyId("spanFromAdd").InnerText;
                int maxTransactionLimit = int.MaxValue;
                ChainTransaction transactionInfo = null;
                for (int i = 1; chaiList.Count < maxTransactionLimit; i++)
                {
                    chaiList.AddRange(ParsePage($"https://bscscan.com/txs?a={walletHash}&p={i}"));
                    if (chaiList.Any(x => x.hash == hash))
                    {
                        transactionInfo = chaiList.Where(x => x.hash == hash).First();
                        break;
                    }

                    Thread.Sleep(500);
                }

                return CountScore(chaiList, transactionInfo);
               
            }
            catch (Exception ex)
            {
                return TransactionStatus.Error;
            }

        }

        private TransactionStatus CountScore(List<ChainTransaction> chaiList, ChainTransaction currentTransaction)
        {
            int tranIndex = chaiList.Select((v, i) => new { t = v, index = i }).First(x => x.t.hash == currentTransaction.hash).index;

            int left = Math.Max(tranIndex - 3, 0);
            int right = Math.Min(tranIndex + 3, chaiList.Count);

            var lastTransaction = chaiList[right];
            var firstInSubset = chaiList[left];
            TimeSpan dif = lastTransaction.date - firstInSubset.date;
            int score = 0;
            if (dif < TimeSpan.FromSeconds(10))
            {
                score += 20;
            }
            if (Helper.Helper.isHoliday(currentTransaction.date))
            {
                score += 10;
            }
            var valueAndCurrency = currentTransaction.value.Split(' ');
            double value = double.Parse(valueAndCurrency[0].Replace('.', ','));
            if (value > 0.01)
            {
                score += 40;
            }
            if (score < 30)
            {
                return TransactionStatus.Normal;
            }
            else if (score < 70)
            {
                return TransactionStatus.Suspicious;
            }
            return TransactionStatus.Danger;
        }

        private List<ChainTransaction> ParsePage(string page)
        {
            HtmlWeb web = new HtmlWeb();
            List<ChainTransaction> chaiList = new List<ChainTransaction>();
            HtmlDocument doc = web.Load(page);
            //pull all of spans which consist 'hash'
            var HeaderFrom = doc.DocumentNode.SelectNodes("//tr");
            for (int j = 0; j < HeaderFrom.Count; j++)
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
                if (showList.hash != "")
                    chaiList.Add(showList);
            }
            return chaiList;
        }
    }
}


//Get info by transaction
//https://bscscan.com/tx/0x2a3259c9e685fd554e50676c227174223cc4d45aa045975d9ab313254caf0242
//extract wallet
//Get all transaction
//https://bscscan.com/txs?a=0x3f349bbafec1551819b8be1efea2fc46ca749aa1
//Find our transaction and nearest

