using System;

namespace OBL_Opgave_5_TCP_server.Models
{
    public class FootballPlayer
    {
        //Kode af Nicolas Lauridsen, Datamatiker 3b Zealand Erhvervsakademi.
        public FootballPlayer() 
        {

        }

        public FootballPlayer(int id, double price, string name, int shirtnumber)
        {
            Id = id;
            Price = price;
            Name = name;
            ShirtNumber = shirtnumber;
        }

        private int _id;
        public int Id { get => _id;
            set { _id = value; }
        }
        private string _name;

        public string Name
        {
            get => _name;
            set
            {
                if (value.Length >= 4)
                {
                    _name = value;
                }
                else
                {
                    throw new ArgumentException();
                }
            }
        }

        private double _price;
        public double Price { 
            get => _price;
            set
            {
                if (value > 0)
                {
                    _price = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException();
                }
            }

        }

        private int _shirtnumber;
        public int ShirtNumber { get => _shirtnumber;
            set
            {
                if (value is >= 1 and <= 100)
                {
                    _shirtnumber = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}
