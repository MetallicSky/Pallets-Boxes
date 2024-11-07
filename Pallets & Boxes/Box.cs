#nullable enable

namespace Pallets___Boxes
{
    internal class Box
    {
        /// <summary>
        /// Constructs box with semi-random sizes (will always fit given pallet) and adds it onto pallet
        /// </summary>
        public Box(in Pallet inputPallet)
        {
            double maxWidth = inputPallet.Width;
            double minWidth = maxWidth / 5.0;
            double maxLength = inputPallet.Length;
            double minLength = maxLength / 5.0;

            if (maxWidth <= 0)
                throw new ArgumentOutOfRangeException(nameof(inputPallet), inputPallet, "Pallet width can't be less or equal to 0");
            if (maxLength <= 0)
                throw new ArgumentOutOfRangeException(nameof(inputPallet), inputPallet, "Pallet length can't be less or equal to 0");
            Random rand = new();
            _height = rand.NextDouble() * (maxWidth - minWidth) + minWidth; // height doesn't really matter, so width is used as reference
            _width = rand.NextDouble() * (maxWidth - minWidth) + minWidth;
            _length = rand.NextDouble() * (maxLength - minLength) + minLength;
            Volume = _height * _width * _length;
            _weight = rand.Next(25, 76);
            _weight *= Volume;
            DateOnly NewDate = DateOnly.FromDateTime(DateTime.Today);
            ExpDate = NewDate.AddDays(rand.Next(1, 101)); // assigns random date between today and 100 days ahead
            inputPallet.AddBox(this);
            Id = ++_lastBoxId;
        }
        public Box(in double min = 0.1, in double max = 0.5)
        {
            if (min <= 0)
                throw new ArgumentOutOfRangeException(nameof(min), min, "Size can't be less or equal to 0");
            if (max <= 0)
                throw new ArgumentOutOfRangeException(nameof(max), max, "Size can't be less or equal to 0");
            if (min > max)
                throw new ArgumentException("Max can't be less than min", nameof(max));
            Random rand = new();
            _height = rand.NextDouble() * (max - min) + min;
            _width = rand.NextDouble() * (max - min) + min;
            _length = rand.NextDouble() * (max - min) + min;
            DateOnly NewDate = DateOnly.FromDateTime(DateTime.Today);
            ExpDate = NewDate.AddDays(rand.Next(1, 101)); // assigns random date between today and 100 days ahead
            Id = ++_lastBoxId;
        }
        public Box(in double width, in double height, in double length, in DateOnly expDate)
        {
            _belongsTo = null;
            Volume = 0;
            Width = width;
            Height = height;
            Length = length;
            ExpDate = expDate;
            Id = ++_lastBoxId;
        }
        private static int _lastBoxId = 0;
        private Pallet? _belongsTo;
        private double _height;
        private double _width;
        private double _length;
        private double _weight;
        /// <summary>
        /// Box expiration date
        /// </summary>
        public DateOnly? ExpDate { get; private set; }
        /// <summary>
        /// Unique box id, generated using static private variable
        /// </summary>
        public int Id { get; private set; }
        /// <summary>
        /// Box volume property, calculated from its height, length and width
        /// </summary>
        public double Volume { get; private set; }
        /// <summary>
        /// Pallet that box belongs to.
        /// </summary>
        public Pallet BelongsTo
        {
            get { return _belongsTo; }
            set
            {
                _belongsTo = value;
            }
        }
        /// <summary>
        /// Box height property, affects box volume and total volume of its pallet
        /// </summary>
        public double Height
        {
            get { return _height; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), value, "Height can't be less or equal 0");
                }
                double newVolume = _length * _width * value;
                if (_belongsTo != null)
                {
                    _belongsTo.TotalVolume += newVolume - Volume;
                }
                _height = value;
                Volume = newVolume;
            }
        }
        /// <summary>
        /// Box width property, affects box volume and total volume of its pallet. Will throw exception if makes box too big for its pallet
        /// </summary>
        public double Width
        {
            get { return _width; }
            set
            {
                double newVolume = _height * _length * value;
                if (_belongsTo != null)
                {
                    var badArgument = new ArgumentException("Box can't fit on the pallet with such width: " + value, nameof(value));
                    var nonPositiveArgument = new ArgumentOutOfRangeException(nameof(value), value, "Width can't be less or equal 0");
                    if (value <= 0)
                    {
                        throw nonPositiveArgument;
                    }
                    if ((value > _belongsTo.Length && _belongsTo.Length < _length) || // check if both new width and old length are bigger than pallet's length
                        (value > _belongsTo.Width && _belongsTo.Width < _length) || // check if both new width and old length are bigger than pallet's width
                        (value > _belongsTo.Width && value > _belongsTo.Length)) // check if new width bigger than both pallet's side
                    {
                        throw badArgument;
                    }
                    _belongsTo.TotalVolume += newVolume - Volume;
                }
                _width = value;
                Volume = newVolume;
            }
        }
        /// <summary>
        /// Box length property, affects box volume and total volume of its pallet. Will throw exception if makes box too big for its pallet
        /// </summary>
        public double Length
        {
            get { return _length; }
            set
            {
                double newVolume = _height * _width * value;
                if (_belongsTo != null)
                {
                    var badArgument = new ArgumentException("Box can't fit on the pallet with such length: " + value, nameof(value));
                    var nonPositiveArgument = new ArgumentOutOfRangeException(nameof(value), value, "Length can't be less or equal 0");
                    if (value <= 0)
                    {
                        throw nonPositiveArgument;
                    }
                    if ((value > _belongsTo.Length && _belongsTo.Length < _width) || // check if both new length and old width are bigger than pallet's length
                        (value > _belongsTo.Width && _belongsTo.Width < _width) || // check if both new length and old width are bigger than pallet's width
                        (value > _belongsTo.Width && value > _belongsTo.Length)) // check if new length bigger than both pallet's side
                    {
                        throw badArgument;
                    }
                    _belongsTo.TotalVolume += newVolume - Volume;
                }
                _length = value;
                Volume = newVolume;
            }
        }
        /// <summary>
        /// Box weight property, also affects pallet's TotalWeight
        /// </summary>
        public double Weight
        {
            get { return _weight; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), value, "Weight can't be less or equal 0");
                }
                if (_belongsTo != null)
                {
                    _belongsTo.TotalWeight += value - _weight;
                }
                _weight = value;
            }
        }

        public void PrintBox()
        {
            string output = "ID: " + Id;
            output += " Weight: " + (double)Math.Round(Weight, 2);
            output += " kg Exp. Date: " + ExpDate;
            output += " Volume: " + (double)Math.Round(Volume, 2);
            output += " m3 (Height: " + (double)Math.Round(Height, 2);
            output += " m Length: " + (double)Math.Round(Length, 2);
            output += " m Width: " + (double)Math.Round(Width, 2);
            output += " m)";
            Console.WriteLine(output);
        }
    }
}