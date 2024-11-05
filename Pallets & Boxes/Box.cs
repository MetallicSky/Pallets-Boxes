#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pallets___Boxes
{
    internal class Box
    {
        private Box(in double min = 0.1, in double max = 0.5)
        {
            if (min <= 0)
                throw new ArgumentOutOfRangeException("min", min, "Size can't be less or equal to 0");
            if (max <= 0)
                throw new ArgumentOutOfRangeException("max", max, "Size can't be less or equal to 0");
            if (min > max)
                throw new ArgumentException("max can't be less than min", "max");
            Random rand = new Random();
            _height = rand.NextDouble() * (max - min) + min;
            _width = rand.NextDouble() * (max - min) + min;
            _length = rand.NextDouble() * (max - min) + min;
        }
        private Pallet? _belongsTo;
        private double _height;
        private double _width;
        private double _length;
        public int Id { get; private set; }
        public double Volume { get; private set; }
        public Pallet BelongsTo
        {
            get { return _belongsTo; }
            set
            {
                // TODO: implement logic to delete box from another pallet and add to current one
                _belongsTo = value;
            }
        }

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
                // TODO: implement logic for checking if box fits onto pallet
                _width = value;
                Volume = _height * _width * _length;
            }
        }
    }
}
