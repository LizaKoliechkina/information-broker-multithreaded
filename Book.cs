using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace InformationBroker
{
    class Book : IComparable<Book>
    {
        public String Name { get; private set; }
        public int Price { get; set; }
        public Warehouse warehouse { get; set; }

        public Book(String name, int price = 0, Warehouse warehouse = null)
        {
            this.Name = name;
            this.Price = price;
            this.warehouse = warehouse;
        }

        public int CompareTo([AllowNull] Book other)
        {
            return this.Price.CompareTo(other.Price);
        }
    }
}
