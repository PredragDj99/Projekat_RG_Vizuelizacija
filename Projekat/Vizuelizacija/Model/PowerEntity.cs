using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Vizuelizacija.Model
{
    public class PowerEntity
    {
        private long id;
        private string name;
        private double x;
        private double y;
        private string toolTip;
        private Brush boja;
        private string brojKonekcija;
        private int obojSlikom;

        public PowerEntity()
        {

        }

        public long Id
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        public double X
        {
            get
            {
                return x;
            }

            set
            {
                x = value;
            }
        }

        public double Y
        {
            get
            {
                return y;
            }

            set
            {
                y = value;
            }
        }

        public string ToolTip
        {
            get
            {
                return toolTip;
            }
            set
            {
                toolTip = value;
            }
        }

        public Brush Boja
        {
            get
            {
                return boja;
            }
            set
            {
                boja = value;
            }
        }

        //Polje za vizuelizaciju
        public string BrojKonekcija
        {
            get
            {
                return brojKonekcija;
            }

            set
            {
                brojKonekcija = value;
            }
        }

        //za bojenje slikom
        public int ObojSlikom
        {
            get
            {
                return obojSlikom;
            }

            set
            {
                obojSlikom = value;
            }
        }
    }
}
