using System;

namespace Contest.Service
{
    public class CreateContestCmd
    {
        public DateTime Date { get; set; }
        public int StreetNumber { get; set; }
        public string Street { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public bool Indoor { get; set; }
        public ushort CountField { get; set; }
        public uint CountMinPlayerByTeam { get; set; }
        public uint CountMaxPlayerByTeam { get; set; }
    }
}