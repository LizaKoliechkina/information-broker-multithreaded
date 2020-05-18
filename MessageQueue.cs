using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InformationBroker
{
    class MessageQueue
    {
        private Queue<Message> queue = new Queue<Message>();

        public void addMessage(Message msg)
        {
            lock (this)
            {
                queue.Enqueue(msg);
            }
        }

        public Message peekMessage()
        {
            Message msg = null;
            lock (this)
            {
                if (queue.Count > 0)
                {
                    msg = queue.Dequeue();
                }
            }
            return msg;
        }

        public int Count()
        {
            lock (this)
            {
                return queue.Count();
            }
        }
    }
}
