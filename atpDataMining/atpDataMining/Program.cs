﻿using Newtonsoft.Json;

namespace atpDataMining
{
    class Program
    {
        static void Main(string[] args)
        {
            var dataRepo = new DataRepository();
            var c = new Parser();
            var playersInfo = c.Parse();
            foreach (var player in playersInfo)
            {
                c.ParsePlayer(player);
            }

            dataRepo.SavePlayers(playersInfo);

            var json = JsonConvert.SerializeObject(playersInfo);

            var file = new System.IO.StreamWriter("test.txt");
            file.WriteLine(json);
            file.Close();

            System.Console.ReadLine();
        }
    }
}
