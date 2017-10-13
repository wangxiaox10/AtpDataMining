using System;
namespace atpDataMining
{
    public class Player
    {
        public string Name { get; set; }
        public int Age { get; set; }

        public bool RightHanded { get; set; }
        public string Link { get; set; }

        public int Rank { get; set; }
        public string BirthPlace { get; set; }
        public string BirthCountry { get; set; }

        public int Points { get; set; }

        public int TurnedPro { get; set; }
        public int Weight { get; set; }

        public int Height { get; set; }

        public string Residence { get; set; }

        public string ResidenceCountry { get; set; }
        public long Prize { get; set; }

        public DateTime BirthDay { get; set; }
    }
}
