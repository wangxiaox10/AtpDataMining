using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System;

namespace atpDataMining
{
    public class Parser
    {
        private readonly static string baseUrl = $"http://www.atpworldtour.com/";
        private readonly static string url = $"http://www.atpworldtour.com/en/rankings/singles";
        public List<Player> Parse()
        {
            HttpClient hc = new HttpClient();
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

                var absolutUrl = new Uri(new Uri(baseUrl), link).ToString();
                var player = new Player
                {
                    Name = data.InnerText.Trim('\t').Trim('\r').Trim('\n'),
                    Link = absolutUrl
                };
                playerList.Add(player);
            }
            return playerList;
        }

        public Player ParsePlayer(Player player)
        {
            Console.WriteLine(player.Name);
            HttpClient hc = new HttpClient();
            var playerUrl = player.Link;
            var web = new HtmlWeb();
            var doc = web.Load(playerUrl);

            var birthdayNode = doc.DocumentNode.SelectSingleNode("//span[@class='table-birthday']");
            var birthdayString = birthdayNode.InnerText.Replace('\t', '(').Replace('\n', '(').Replace('\r', '(').Trim('(').Trim(')');
            var birthday = DateTime.Parse(birthdayString);

            player.BirthDay = birthday;

            var today = DateTime.Today;
            // Calculate the age.
            var age = today.Year - birthday.Year;
            // Go back to the year the person was born in case of a leap year
            if (birthday > today.AddYears(-age)) age--;

            player.Age = age;

            return player;
        }
    }
}