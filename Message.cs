using System;
using System.Collections.Generic;
using System.Text;

namespace InformationBroker
{
    class Message
    {
        public int FromId { get; set; }
        public int ToId { get; set; }
        public Book book;

        public Message(int from, int to, Book book)
        {
            this.FromId = from;
            this.ToId = to;
            this.book = book;
        }

        public override string ToString()
        {
            return "From: " + FromId + ", To: " + ToId + ", book: " + book.Name + ", " + book.Price + ".";
        }
    }
}
