using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace atpDataMining
{
    public class Parser
    {
        public async Task<List<Player>> Parse()
        {
            HttpClient hc = new HttpClient();
            var url = $"http://www.atpworldtour.com/en/rankings/singles";
            var web = new HtmlWeb();
            var doc = web.Load(url);
            //HttpResponseMessage result = await hc.GetAsync($);

            //var stream = await result.Content.ReadAsStreamAsync();

            //var doc = new HtmlDocument();
            //doc.Load(stream);

            var playerList = new List<Player>();
            var links = doc.DocumentNode.SelectNodes("//td[@class='player-cell']");//the parameter is use xpath see: https://www.w3schools.com/xml/xml_xpath.asp 


            foreach (var data in links)
            {
                var link = data.ChildNodes[1].Attributes[0].Value;
                var absolutUrl = url + link;
                var player = new Player
                {
                    Name = data.InnerText.Trim('\t').Trim('\r').Trim('\n'),
                    Link = absolutUrl
                };
                playerList.Add(player);
                System.Console.WriteLine(data.FirstChild);
            }
            return playerList;
        }
    }
}