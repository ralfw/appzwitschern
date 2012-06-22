using System;
using az.security;
using io.iron.ironmq;

namespace az.ironmqapi
{
    public class IronMQOperations
    {
        private readonly Client _client;
        private readonly Queue _queue;

        public IronMQOperations(string queueName, Token credentials)
        {
            _client = new Client(credentials.Key, credentials.Secret);
            _queue = _client.queue(queueName);
        }


        public void Enqueue(string data)
        {
            _queue.push(data);
        }


        public void Dequeue(Action<string> onDataReceived)
        {
            do
            {
                var msg = _queue.get();
                if (msg == null) break;

                onDataReceived(msg.Body);

                _queue.deleteMessage(msg);
            } while (true);

            onDataReceived(null);
        }
    }
}
