using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Pallets___Boxes
{
    internal class Pallet
    {
        public Pallet()
        {

        }

        private double _height;
        private double _width;
        private double _length;
        public int Id { get; private set; }
        public double Volume { get; private set; }
        public double Area { get; private set; }
        public double Height
        {
            get { return _height; }
            set
            {
                _height = value;
                Volume = _height * _width * _length;
            }
        }
        public double Width
        {
            get { return _width; }
            set
            {
                if
                _width = value;
                Volume = _height * _width * _length;
            }
        }



    }
}
