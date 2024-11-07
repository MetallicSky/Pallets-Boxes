namespace Pallets___Boxes
{
    internal class Pallet
    {
        /// <summary>
        /// Default constructor with 1m dimensions, 30kg weight 
        /// </summary>
        public Pallet()
        {
            TotalVolume = 1;
            PalletVolume = 1;
            _height = 1;
            _width = 1;
            _length = 1;
            ExpDate = null;
            TotalWeight = 0;
            PalletWeight = 30;
            Id = ++_lastPalletId;
        }
        /// <summary>
        /// Constructor with specified dimensions and weight
        /// </summary>
        public Pallet(in double height, in double width, in double length, in double palletWeight)
        {
            Height = height;
            Width = width;
            Length = length;
            ExpDate = null;
            TotalWeight = 0;
            PalletWeight = palletWeight;
            Id = ++_lastPalletId;
        }
        /// <summary>
        /// static id shared between all Pallet instances, used for unique id generation
        /// </summary>
        private static int _lastPalletId = 0;
        /// <summary>
        /// Private list of all pallet's boxes
        /// </summary>
        private readonly List<Box> _boxes = [];
        /// <summary>
        /// Private pallet height, only contributes to palletVolume and isn't important to the size of boxes able to fit onto pallet
        /// </summary>
        private double _height;
        /// <summary>
        /// Private pallet width, determines the size of boxes that can fit onto it
        /// </summary>
        private double _width;
        /// <summary>
        /// Private pallet length, determines the size of boxes that can fit onto it
        /// </summary>
        private double _length;
        /// <summary>
        /// Private pallet weight, contributes to total weight
        /// </summary>
        private double _palletWeight;
        /// <summary>
        /// Readonly list of all pallet's boxes
        /// </summary>
        public IReadOnlyList<Box> Boxes => _boxes;
        /// <summary>
        /// Pallet expiration date, equals soonest exp date of all its boxes
        /// </summary>
        public DateOnly? ExpDate { get; private set; }
        /// <summary>
        /// Unique pallet id, generated using static private variable
        /// </summary>
        public int Id { get; private set; }
        /// <summary>
        /// Volume of pallet without any of its boxes
        /// </summary>
        public double PalletVolume { get; private set; }
        /// <summary>
        /// Combined volume of pallet and all its boxes. WARNING: setter is internal and not meant to be used without prior checks
        /// </summary>
        public double TotalVolume { get; internal set; }
        /// <summary>
        /// Pallet volume, setter changes TotalVolume accordingly
        /// </summary>
        public double PalletWeight
        {
            get { return _palletWeight; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Weight can't be less or equal 0");
                }
                TotalWeight += value - PalletWeight;
                _palletWeight = value;
            }
        }
        /// <summary>
        /// Combined weight of pallet and all its boxes. WARNING: setter is internal and not meant to be used without prior checks
        /// </summary>
        public double TotalWeight { get; internal set; }
        /// <summary>
        /// Pallet height property, doesn't affect boxes compatibility. Changes pallet and total volume accordingly
        /// </summary>
        public double Height
        {
            get { return _height; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Height can't be less or equal 0");
                }
                _height = value;
                double newPalletVolume = _height * _width * _length;
                TotalVolume += newPalletVolume - PalletVolume;
                PalletVolume = newPalletVolume;
            }
        }
        /// <summary>
        /// Pallet width property, affects boxes compatibility and checks if all of them will fit, throwing exception if not. Changes pallet and total volume accordingly
        /// </summary>
        public double Width
        {
            get { return _width; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Width can't be less or equal 0");
                }
                double oldWidth = _width;
                _width = value;
                if (CheckSizes())
                {
                    double newPalletVolume = _height * _width * _length;
                    TotalVolume += newPalletVolume - PalletVolume;
                    PalletVolume = newPalletVolume;
                }
                else
                {
                    _width = oldWidth;
                    throw new ArgumentException("At least one box wouldn't fit onto pallet with new width", nameof(value));
                }
            }
        }
        /// <summary>
        /// Pallet length property, affects boxes compatibility and checks if all of them will fit, throwing exception if not. Changes pallet and total volume accordingly
        /// </summary>
        public double Length
        {
            get { return _length; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("Length", "Length can't be less or equal 0");
                }
                double oldLength = _length;
                _length = value;
                if (CheckSizes())
                {
                    double newPalletVolume = _height * _width * _length;
                    TotalVolume += newPalletVolume - PalletVolume;
                    PalletVolume = newPalletVolume;
                }
                else
                {
                    _length = oldLength;
                    throw new ArgumentException("At least one box wouldn't fit onto pallet with new length", nameof(value));
                }
                
            }
        }

        /// <summary>
        /// Adds box without specified pallet to the current one. Also checks if it fits onto it, throwing exception if it's not
        /// </summary>
        /// <param name="box">box to be inserted onto pallet</param>
        public void AddBox(Box box)
        {
            if (box.BelongsTo != null)
            {
                throw new ArgumentException("Box already belongs to some pallet. Did you mean to use MoveBox()?", nameof(box));
            }
            if (box.Length <= 0 || box.Width <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(box), "Size can't be less or equal 0");
            }
            if (CheckSizes(box) == false)
            {
                throw new ArgumentException("Box can't fit on the pallet with such size", nameof(box));
            }
            _boxes.Add(box);
            if (ExpDate == null || box.ExpDate < ExpDate)
                ExpDate = box.ExpDate;
            TotalVolume += box.Volume;
            TotalWeight += box.Weight;
            box.BelongsTo = this;
        }
        /// <summary>
        /// Adds box without specified pallet to the current one. Also checks if it fits onto it, throwing exception if it's not
        /// </summary>
        /// <param name="box">box to be inserted onto pallet</param>
        public void RemoveBox(Box box)
        {
            if (_boxes.Remove(box) == false)
                throw new ArgumentException("Specified box wasn't found or isn't able to be deleted", nameof(box));
            if (box.ExpDate == ExpDate) // if removed box has the same expDate as pallet's then there is a possibility that box was the reason for that expDate
            {
                DateOnly? newExpDate = null; // if there are no boxes on the pallet, next loop wouldn't proc

                for (int i = 0; i < Boxes.Count; i++)
                {
                    if (Boxes[i].ExpDate == ExpDate) // if we found another box with the same expDate, then the rest of the search wouldn't yield anything new
                        break;
                    if (newExpDate == null || newExpDate > Boxes[i].ExpDate)
                        newExpDate = Boxes[i].ExpDate;
                }
                ExpDate = newExpDate;
            }
            TotalVolume -= box.Volume;
            TotalWeight -= box.Weight;
            box.BelongsTo = null;
        }
        /// <summary>
        /// Moves specified box from this pallet onto another
        /// </summary>
        /// <param name="box">Box to be inserted onto pallet</param>
        /// <param name="targetPallet">Pallet where box should be moved to</param>
        public void MoveBox(Box box, Pallet targetPallet)
        {
            if (box.BelongsTo != this)
                throw new ArgumentException("Box doesn't belong to the current pallet", nameof(targetPallet));
            if (this == targetPallet)
                throw new ArgumentException("targetPallet can't be the pallet method was called from", nameof(targetPallet));
            if (box == null)
                throw new ArgumentException("Box can't be null", nameof(box));
            if (targetPallet == null)
                throw new ArgumentException("Pallet can't be null. Did you mean to use RemoveBox()?", nameof(targetPallet));
            RemoveBox(box);
            targetPallet.AddBox(box);
        }
        /// <summary>
        /// Sorts boxes list by specified parameter. Example: SortBoxesBy(box => box.ExpDate, (x, y) => y.CompareTo(x)) // Descending order, SortBoxesBy(box => box.ExpDate, (x, y) => x.CompareTo(y)); // ascending order
        /// </summary>
        public void SortBoxesBy<T>(Func<Box, T> keySelector) where T : IComparable<T>
        {
            _boxes.Sort((box1, box2) => keySelector(box1).CompareTo(keySelector(box2)));
        }
        /// <summary>
        /// Checks whole list of boxes if they fit onto current pallet (usually used after pallet size change)
        /// </summary>
        private bool CheckSizes()
        {
            for (int i = 0; i < _boxes.Count; i++)
            {
                if (CheckSizes(_boxes[i]) == false)
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// Checks if this box fits onto pallet
        /// </summary>
        private bool CheckSizes(in Box box)
        {
            double bLength = box.Length;
            double bWidth = box.Width;
            if ((bLength > _length && _length < bWidth) || // check if both box length and box width are bigger than pallet's length
            (bLength > _width && _width < bWidth) || // check if both new length and old length are bigger than pallet's width
            (bLength > _width && bLength > _length) || // check if box length bigger than both pallet's side
            (bWidth > _width && bWidth > _length)) // check if box width bigger than both pallet's side
            {
                return false;
            }
            return true;
        }

        public void PrintPallet()
        {
            string output = "ID: " + Id;
            output += " Total Weight: " + (double)Math.Round(TotalWeight, 2);
            output += " Pallet Weight: " + (double)Math.Round(PalletWeight, 2);
            output += " kg Exp. Date: " + ExpDate;
            output += " Total Volume: " + (double)Math.Round(TotalVolume, 2);
            output += " m3 Pallet Volume: " + (double)Math.Round(PalletVolume, 2);
            output += " m3 (Height: " + (double)Math.Round(Height, 2);
            output += " m Length: " + (double)Math.Round(Length, 2);
            output += " m Width: " + (double)Math.Round(Width, 2);
            output += " m)";
            Console.WriteLine(output);
            for (int i = 0; i < _boxes.Count; i++)
            {
                if (i < _boxes.Count - 1)
                {
                    Console.Write("╠═");
                }
                else
                {
                    Console.Write("╚═");
                }
                _boxes[i].PrintBox();
            }
        }

    }
}
