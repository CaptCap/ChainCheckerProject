using System.Xml;
using Microsoft.AspNetCore.Mvc;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Application;

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
    public IEnumerable<ChainSaver> Get()
    {
        //import libruary
       
        //create crawler class 
        HtmlWeb web = new HtmlWeb();
        //download webPage
        List<ChainSaver> chaiList = new List<ChainSaver>();

        for (int i = 1; i < 2; i++)
        {
            HtmlDocument doc = web.Load("https://bscscan.com/txs?p=" + i);
            //pull all of spans which consist 'hash'
            var HeaderNames = doc.DocumentNode.SelectNodes("//span[contains(@class, 'hash-tag')]");
            var HeaderTime = doc.DocumentNode.SelectNodes("//td[contains(@class, 'showDate')]");
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



           


        }
        return (chaiList);



    }
}

