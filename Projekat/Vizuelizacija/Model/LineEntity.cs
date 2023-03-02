using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vizuelizacija.Model
{
    public class LineEntity
    {
        private long id;
        private string name;
        private bool isUnderground;
        private float r;
        private string conductorMaterial;
        private string lineType;
        private long thermalConstantHeat;
        private long firstEnd;
        private long secondEnd;
        private List<Point> vertices;
        //za BFS
        private double pocetakX;
        private double pocetakY;
        private double krajX;
        private double krajY;
        private string prolaz;

        public LineEntity()
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

        public bool IsUnderground
        {
            get
            {
                return isUnderground;
            }

            set
            {
                isUnderground = value;
            }
        }

        public float R
        {
            get
            {
                return r;
            }

            set
            {
                r = value;
            }
        }

        public string ConductorMaterial
        {
            get
            {
                return conductorMaterial;
            }

            set
            {
                conductorMaterial = value;
            }
        }

        public string LineType
        {
            get
            {
                return lineType;
            }

            set
            {
                lineType = value;
            }
        }

        public long ThermalConstantHeat
        {
            get
            {
                return thermalConstantHeat;
            }

            set
            {
                thermalConstantHeat = value;
            }
        }

        public long FirstEnd
        {
            get
            {
                return firstEnd;
            }

            set
            {
                firstEnd = value;
            }
        }

        public long SecondEnd
        {
            get
            {
                return secondEnd;
            }

            set
            {
                secondEnd = value;
            }
        }

        public List<Point> Vertices
        {
            get
            {
                return vertices;
            }

            set
            {
                vertices = value;
            }
        }

        public double PocetakX { get => pocetakX; set => pocetakX = value; }
        public double PocetakY { get => pocetakY; set => pocetakY = value; }
        public double KrajX { get => krajX; set => krajX = value; }
        public double KrajY { get => krajY; set => krajY = value; }
        public string Prolaz { get => prolaz; set => prolaz = value; }
    }
}
