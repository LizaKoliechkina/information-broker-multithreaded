using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace InformationBroker
{
    class Client
    {
        public int Id { get; private set; }
        private Thread thread;
        public Book book;
        public MessageQueue requestQueue;
        public MessageQueue responseQueue;
        private Broker broker;
        private List<Book> offeredBooks = new List<Book>();

        public Client(int id, string bookName, Broker broker)
        {
            this.Id = id;
            this.book = new Book(bookName);
            this.broker = broker;
            this.requestQueue = broker.clientRequestQueue;
            this.responseQueue = new MessageQueue();

            //ThreadPool.QueueUserWorkItem(Task);
            thread = new Thread(Task);
            thread.Start();
        }

        private void sendRequest()
        {
            Console.WriteLine("Client {0} sent a request.", Id);
            lock (broker)
            {
                requestQueue.addMessage(new Message(Id, broker.Id, book));
            }
        }

        private void Task()
        {
            CheckIn();
            sendRequest();
            int wait = 0;

            while (broker.bookmarketOpened && wait < 20)
            {
                if (responseQueue.Count() > 0)
                {
                    Message msg = null;
                    lock (this)
                    {
                        msg = responseQueue.peekMessage();
                    }
                    if (msg != null)
                    {
                        offeredBooks.Add(msg.book);
                    }
                }
                else
                {
                    wait++;
                    Thread.Sleep(10);
                }
            }
            
            if (offeredBooks.Count > 0) { chooseBook(); }
            CheckOut();
        }

        private void chooseBook()
        {
            // sorts all offered books according to price and chooses the cheapest
            offeredBooks.Sort();
            book = offeredBooks[0];
        }

        private void CheckIn()
        {
            this.broker.clients.Add(this);
            Console.WriteLine("New client {0} registered.", Id);
        }
        private void CheckOut()
        {
            if (book.Price != 0)
            {
                Console.WriteLine("Client {0} bought a book '{1}' for the price {2}$ at the Warehouse {3}", Id, book.Name, book.Price, book.warehouse.Id);
            }
            else if (book.Price == 0)
            {
                Console.WriteLine("Client {0} did not find a book '{1}' at any warehouse.", Id, book.Name);
            }

            lock (broker)
            {
                this.broker.clients.Remove(this);
            }
            Console.WriteLine("Client {0} checked out from the system.", Id);
        }


    }
}
