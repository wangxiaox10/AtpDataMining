using System.Collections.Generic;
using System.Net.Http;
using HtmlAgilityPack;
using System;
using static System.Int32;

namespace atpDataMining
{
    public class Parser
    {
        private readonly static string baseUrl = $"http://www.atpworldtour.com/";
        private readonly static string url = $"http://www.atpworldtour.com/en/rankings/singles";
        private readonly static string folder = "playersPage";

        public List<Player> Parse()
        {
            var web = new HtmlWeb();
            var doc = web.Load(url);

            if (!System.IO.Directory.Exists(folder))
            {
                System.IO.Directory.CreateDirectory(folder);
            }

            var playerList = new List<Player>();
            var links = doc.DocumentNode.SelectNodes("//td[@class='player-cell']");//the parameter is use xpath see: https://www.w3schools.com/xml/xml_xpath.asp 

            //download pages to local

            var rank = 1;
            foreach (var data in links)
            {
                var link = data.ChildNodes[1].Attributes[0].Value;
                var absolutUrl = new Uri(new Uri(baseUrl), link).ToString();
                var player = new Player
                {
                    Rank = rank++,
                    Name = data.InnerText.Trim('\t').Trim('\r').Trim('\n'),
                    Link = absolutUrl
                };
                playerList.Add(player);
            }
            return playerList;
        }

        public Player ParsePlayer(Player player)
        {
            Console.WriteLine(player.Rank + " " + player.Name);
            var playerUrl = player.Link;
            HtmlDocument doc = null;
            var downloadedFilePath = $"{folder}/{player.Name}";
            if (System.IO.File.Exists(downloadedFilePath))
            {
                var content = System.IO.File.ReadAllText(downloadedFilePath);
                doc = new HtmlDocument();
                doc.LoadHtml(content);
            }
            else
            {
                HttpClient hc = new HttpClient();
                var web = new HtmlWeb();
                doc = web.Load(playerUrl);
                doc.Save(downloadedFilePath);
            }

            //Birthday 
            var birthdayNode = doc.DocumentNode.SelectSingleNode("//span[@class='table-birthday']");
            var birthdayString = birthdayNode.InnerText.Replace('\t', '(').Replace('\n', '(').Replace('\r', '(').Trim('(').Trim(')');
            var birthday = DateTime.Parse(birthdayString);
            player.BirthDay = birthday;

            //Age 
            var today = DateTime.Today;
            // Calculate the age.
            var age = today.Year - birthday.Year;
            // Go back to the year the person was born in case of a leap year
            if (birthday > today.AddYears(-age)) age--;
            player.Age = age;

            //weight in kg 
            var weightNode = doc.DocumentNode.SelectSingleNode("//span[@class='table-weight-kg-wrapper']");
            var weightString = weightNode.InnerText.Replace('\t', '(').Replace('\n', '(').Replace('\r', '(').Trim('(').Trim(')').TrimEnd(new char[] { 'k', 'g' });
            int weight;
            if (TryParse(weightString, out weight))
            {
                player.Weight = weight;
            }

            //height in cm 
            var heightNode = doc.DocumentNode.SelectSingleNode("//span[@class='table-height-cm-wrapper']");
            var heightString = heightNode.InnerText.Replace('\t', '(').Replace('\n', '(').Replace('\r', '(').Trim('(').Trim(')').TrimEnd(new char[] { 'c', 'm' });
            int height;
            if (TryParse(heightString, out height))
            {
                player.Height = height;
            }

            //Turned pro
            var turnedProTextNode = doc.DocumentNode.SelectSingleNode("//div[text() = 'Turned Pro']");
            var turnedProYearNode = turnedProTextNode.SelectNodes("//div [@class='table-big-value']")[1];
            var turnedProYearStr = turnedProYearNode.InnerText.Replace('\t', '(').Replace('\n', '(').Replace('\r', '(').Trim('(').Trim(')');
            int year;
            if (TryParse(turnedProYearStr, out year))
            {
                player.TurnedPro = year;
            }

            //Residence 
            var residenceNode = doc.DocumentNode.SelectSingleNode("//div[text() = 'Residence']").ParentNode.ChildNodes[3];
            var residence = residenceNode.InnerText.Trim('\t').Trim('\n');
            player.Residence = residence;

            //ResidenceCountry 
            if (!string.IsNullOrEmpty(residence))
            {
                var places = residence.Split(',');
                var country = places[places.Length - 1];
                player.ResidenceCountry = country;
            }

            //BirthPlace 
            var birthPlaceNode = doc.DocumentNode.SelectSingleNode("//div[contains(text(), 'Birthplace')]").ParentNode.ChildNodes[3];
            var birthPlace = birthPlaceNode.InnerText.Replace("\r", string.Empty).Replace("\n", string.Empty).Replace("\t", string.Empty);
            player.BirthPlace = birthPlace;

            //BirthCountry 
            if (!string.IsNullOrEmpty(birthPlace))
            {
                var places = birthPlace.Split(',');
                var country = places[places.Length - 1];
                player.BirthCountry = country;
            }

            return player;
        }
    }
}