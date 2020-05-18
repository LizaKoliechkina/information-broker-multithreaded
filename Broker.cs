using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace InformationBroker
{
    class Broker
    {
        public int Id { get; set; }
        public List<Client> clients = new List<Client>();
        public List<Warehouse> warehouses = new List<Warehouse>();
        public MessageQueue clientRequestQueue = new MessageQueue();
        public MessageQueue warehouseResponseQueue = new MessageQueue();
        private Thread thSendClientRequests;
        private Thread thSendWarehouseResponses;

        public volatile Boolean bookmarketOpened = false;

        public Broker()
        {
            this.Id = 0; // needed to send messages
            this.thSendClientRequests = new Thread(SendClientRequests);
            this.thSendWarehouseResponses = new Thread(SendWarehouseResponses);
            this.bookmarketOpened = true;
            thSendClientRequests.Start();
            thSendWarehouseResponses.Start();
            //ThreadPool.QueueUserWorkItem(SendClientRequests);
            //ThreadPool.QueueUserWorkItem(SendWarehouseResponses);
        }

        private void SendClientRequests()
        {
            while (bookmarketOpened)
            {
                if (clientRequestQueue.Count() > 0)
                {
                    Message msg = null;
                    lock (this)
                    {
                        msg = clientRequestQueue.peekMessage();
                    }
                    if (msg != null)
                    {
                        Console.WriteLine("Got a request from Client {0}", msg.FromId);
                        for (int i = 0; i < warehouses.Count; i++)
                        {
                            lock (warehouses[i])
                            {
                                warehouses[i].requestQueue.addMessage(msg);
                            }
                        }
                    }
                }
                else
                {
                    Thread.Sleep(10);
                }
            }
        }

        private void SendWarehouseResponses()
        {
            int wait = 0;
            while (wait < 100)
            {
                if (warehouseResponseQueue.Count() > 0)
                {
                    Message msg = null;
                    lock (this)
                    {
                        msg = warehouseResponseQueue.peekMessage();
                    }
                    if (msg != null)
                    {
                        for (int i=0; i<clients.Count; i++)
                        {
                            // msg has the Id of a client to which this message should be sent
                            // finds this client from the list 
                            if (clients[i].Id == msg.ToId)
                            {
                                lock (clients[i])
                                {
                                    clients[i].responseQueue.addMessage(msg);
                                }
                                break;
                            }
                        }
                    }
                }
                else
                {
                    Thread.Sleep(10);
                    wait++;
                }
            }
            finishBookmarketWork();
        }

        private void finishBookmarketWork()
        {
            bookmarketOpened = false;
            Console.WriteLine("Broker finished its work!");
        }

    }
}
