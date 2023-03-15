using System.Xml;
using Microsoft.AspNetCore.Mvc;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Application;
using chainChackerAPI.Entities;

namespace chainChackerAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }
    [HttpGet(Name = "GetTransaction")]
    public IEnumerable<ChainTransactionWE> Get(string Hash)
    {
        //import libruary
       
        //create crawler class 
        HtmlWeb web = new HtmlWeb();
        //download webPage
        List<ChainTransaction> chaiList = new List<ChainTransaction>();

        for (int i = 1; i < 5; i++)
        {
            HtmlDocument doc = web.Load("https://bscscan.com/txs?p=" + i);
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
                chaiList.Add(showList);
            }
            Thread.Sleep(1500);
        }
        return chaiList.Select(t=>new ChainTransactionWE(t,GetStatus(t.hash)));




    }
    public TransactionStatus GetStatus(string Hash)
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
        for (int i = 1; chaiList.Count < maxTransactionLimit; i++)
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
        double value = double.Parse(valueAndCurrency[0].Replace('.', ','));
        if (value > 0.01)
        {
            score += 40;
        }
        if (score<30)
        {
            return TransactionStatus.Normal;
        }
        else if (score < 70)
        {
            return TransactionStatus.Suspicious;


        }
        else
        {
            return TransactionStatus.Danger;
        }

    }

}

