using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace battleships_official2
{
    internal class GameResults
    {
        public string Player1 { get; set; }
        public string Player2 { get; set; }
        public string Winner { get; set; }
        public int GameTime { get; set; }
        public int Turns { get; set; }

        public int Wins { get; set; }
        public int Loses { get; set; }

    }
}
