using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace LuckyAce
{
    class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        private decimal Balance;
    }
    class Event
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Name { get; set; }

    }
    class Match : Event {
        public int Id { get; set; }
        public DateTime Date { get; set; }

        public virtual void MatchName() 
        {
            Console.WriteLine("Team1 vs Team2");
        }

    }
    class Team1 : Match
    {
        public int Id { get; set; }
        //public string Name { get; set; }
        public string Roster { get; set; }
        public decimal Odds { get; set; }

        public string Name = "Liqud";
        public override void MatchName()
        {

            Team1 Team1 = new Team1();
            Name = "Liqud";
            Console.Write(Team1.Name);
        }
    }

    class Team2 : Match
    {
        public int Id { get; set; }
        //public string Name { get; set; }
        public string Roster { get; set; }
        public decimal Odds { get; set; }

        public string Name = "NAVI";

        public override void MatchName()
        {

            Team2 Team2 = new Team2();
            Console.WriteLine( " vs " + Team2.Name);
        }
    }
    class Bet
    {
        public int Id { get; set; }
        public User Player { get; set; }
        public Event GameEvent { get; set; }
        public decimal Amount { get; set; }

    }

}
