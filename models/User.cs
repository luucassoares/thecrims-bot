using System;
using System.Collections.Generic;
using System.Text;

namespace thecrims_bot.models
{
    public class User
    {
        public int id { get; set; }
        public string eid { get; set; }
        public string username { get; set; }
        public int respect { get; set; }
        public int tolerance { get; set; }
        public int strength { get; set; }
        public int charisma { get; set; }
        public int intelligence { get; set; }
        public int cash { get; set; }
        public int stamina { get; set; }
        public string spirit_name { get; set; }
        public int level { get; set; }
        public int addiction { get; set; }
        public int tickets { get; set; }
        public bool in_prision { get; set; }

        public override string ToString()
        {
            return String.Format("Respect: {0}, Strength: {1}, Charisma: {2}, Intelligence: {3}, Cash: {4}, Stamina: {5}, Level: {6}, Tickets: {7}", respect, strength, charisma, intelligence, cash, stamina, level, tickets);
        }
    }
}
