using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack_Desktop
{
    class BlackJack
    {
        static void Main(string[] args)
        {
            //config hard-code
            List<Player> prefillPlayers = new List<Player>() {
                new Player("John Doe", 1000)
            };
            int decksInShoe = 8;
            double targetPenetration = 0.3;
            double payoutMultiplier = 1;

            //Game Start
            Console.WriteLine("Welcome to Dan's Blackjack Tables!!");
            Console.WriteLine();

            Table blackJackTable = new Table(prefillPlayers, decksInShoe, targetPenetration, payoutMultiplier);
            blackJackTable.playGame();

            Console.WriteLine("Please come again!!");
            Console.ReadLine();
        }
    }
}
