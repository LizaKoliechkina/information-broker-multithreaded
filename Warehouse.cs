using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace InformationBroker
{
    class Warehouse
    {
        public int Id { get; set; }
        public List<Book> books = new List<Book>();
        private Thread thread;
        public MessageQueue requestQueue;
        public MessageQueue responseQueue;
        private Broker broker;

        public Warehouse(int id, String[] bookNames, int[] prices, Broker broker)
        {
            this.Id = id;
            for (int i=0; i < bookNames.Length; i++)
            {
                this.books.Add(new Book(bookNames[i], prices[i], this));
            }
            this.broker = broker;
            this.responseQueue = broker.warehouseResponseQueue;
            this.requestQueue = new MessageQueue();

            //ThreadPool.QueueUserWorkItem(Task);
            thread = new Thread(Task);
            thread.Start();
        }

        private void Task()
        {
            CheckIn();
            while (broker.bookmarketOpened)
            {
                if (requestQueue.Count() > 0)
                {
                    Message msg = null;
                    lock (this)
                    {
                        msg = requestQueue.peekMessage();
                    }
                    if (msg != null)
                    {
                        for (int i = 0; i < books.Count; i++)
                        {
                            if (msg.book.Name == books[i].Name)
                            {
                                lock (broker)
                                {
                                    responseQueue.addMessage(new Message(Id, msg.FromId, books[i]));
                                }
                            }
                        }
                    }
                } 
                else
                {
                    Thread.Sleep(5);
                }
            }
            CheckOut();
        }

        private void CheckIn()
        {
            this.broker.warehouses.Add(this);
            Console.WriteLine("New warehouse {0} registered.", Id);
        }
        private void CheckOut()
        {
            lock (broker)
            {
                this.broker.warehouses.Remove(this);
            }
            Console.WriteLine("Warehouse {0} finished the work.", Id);
        }
    }
}
