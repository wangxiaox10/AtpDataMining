using System.Net.Http;
using HtmlAgilityPack;

namespace atpDataMining
{
    public class Parser
    {
        public async void Parse()
        {
            HttpClient hc = new HttpClient();
            HttpResponseMessage result = await hc.GetAsync($"http://www.atpworldtour.com/en/rankings/singles");

            var stream = await result.Content.ReadAsStreamAsync();

            var doc = new HtmlDocument();
            doc.Load(stream);

            var links = doc.DocumentNode.SelectNodes("//td[@class='player-cell']");//the parameter is use xpath see: https://www.w3schools.com/xml/xml_xpath.asp 


        }
    }
}