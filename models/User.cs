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
        public int credits { get; set; }
        public string country { get; set; }
        public string language { get; set; }
        public int tolerance { get; set; }
        public int strength { get; set; }
        public int charisma { get; set; }
        public int intelligence { get; set; }
        public int gang_points { get; set; }
        public int total_gang_points { get; set; }
        public int cash { get; set; }
        public int bank { get; set; }
        public int stamina { get; set; }
        public string spirit_name { get; set; }
        public int level { get; set; }
        public string level_text_name { get; set; }
        public int assault_points { get; set; }
        public string character_text_name { get; set; }
        public int addiction { get; set; }
        public string avatar { get; set; }
        public bool under_protection { get; set; }
        public int tickets { get; set; }
        public bool alive { get; set; }
        public bool in_prison { get; set; }
        public object prison_end_time { get; set; }
        public object prison_end_time_formatted { get; set; }
        public object rip_end_time { get; set; }
        public object rip_end_time_formatted { get; set; }
        public bool show_ads { get; set; }
        public bool vip { get; set; }
        public int nightclub_id { get; set; }
        public bool new_message { get; set; }
        public bool new_relation { get; set; }
        public bool new_temp_relation { get; set; }
        public bool new_gang { get; set; }
        public bool new_task { get; set; }
        public int robbery_power { get; set; }
        public int single_robbery_power { get; set; }
        public int gang_robbery_power { get; set; }
        public int assault_power { get; set; }
        public bool can_favourite_nightclubs { get; set; }
        public List<string> equipment { get; set; }
        public string push_id { get; set; }
        public object skill_points { get; set; }
        public string gang_push_id { get; set; }
        public bool new_relation_sound { get; set; }
        public bool is_crew { get; set; }
        public bool is_test { get; set; }
        public Gang gang { get; set; }
        public bool is_gang_leader { get; set; }
        public bool is_co_leader { get; set; }
        public bool mui { get; set; }
        public bool show_welcome_message { get; set; }
        public string fingerprint { get; set; }

        public override string ToString()
        {
            return String.Format("Respect: {0}, Tolerance: {1}, Strength: {2}\nCharisma: {3}, Intelligence: {4}, Cash: {5}\nStamina: {6}, Level: {7}, Tickets: {8}\nSingleRobberyPower: {9}, Gang: {10}", respect, tolerance, strength, charisma, intelligence, cash, stamina, level, tickets, single_robbery_power, gang != null);
        }
    }
}
