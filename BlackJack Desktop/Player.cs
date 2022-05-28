using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack_Desktop
{
    public class Player
    {
        public string Name { get; set; }
        public int Money { get; set; }

        public Player(string name, int money)
        {
            Name = name;
            Money = money;
        }
    }
}
