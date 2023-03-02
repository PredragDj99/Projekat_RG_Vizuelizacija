using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vizuelizacija.BFS
{
    public class PozicijaPolja
    {
        int pozX;
        int pozY;

        public PozicijaPolja(int pozX, int pozY)
        {
            this.pozX = pozX;
            this.pozY = pozY;
        }

        public int PozX { get => pozX; set => pozX = value; }
        public int PozY { get => pozY; set => pozY = value; }
    }
}
