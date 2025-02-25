// See https://aka.ms/new-console-template for more information
using System;


using System;
namespace LuckyAce
{
    class Program
    {
        static void Main(string[] args)
        {
            Event myObj = new Event();
            myObj.Name = "DreamLeague 25 сезон";
            Match Match1 = new Match();
            Match Team1 = new Team1();
            Match Team2 = new Team2();
            Match1.MatchName();
            Team1.MatchName();
            Team2.MatchName();
            Console.WriteLine(myObj.Name);
        }
    }
}




