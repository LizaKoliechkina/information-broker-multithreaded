using System;

namespace InformationBroker
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Bookmarket is opened!");
            MessageQueue warehouseResponseQueue = new MessageQueue();
            Broker broker = new Broker();
            Client c1 = new Client(0, "Harry Potter", broker);
            Client c2 = new Client(1, "The Adventures of Tom Sawyer", broker);
            Client c3 = new Client(2, "Sherlock Holmes", broker);

            Warehouse wh1 = new Warehouse(0, new string[] { "Harry Potter", "The Adventures of Tom Sawyer" }, new int[] { 35, 40 }, broker);
            Warehouse wh2 = new Warehouse(1, new string[] { "Sherlock Holmes", "The Adventures of Tom Sawyer" }, new int[] { 10, 20 }, broker);
            Warehouse wh3 = new Warehouse(2, new string[] { "Harry Potter", "The Adventures of Tom Sawyer", "Sherlock Holmes" }, new int[] { 25, 30, 31 }, broker);

        }
    }
}
