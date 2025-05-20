using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarCards
{
    internal class Player
    {
        public string Name { get; set; }
        public int Points { get; set; }
        public List<Card> Cards { get; set; }
    }
}
