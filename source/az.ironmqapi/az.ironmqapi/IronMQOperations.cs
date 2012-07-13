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
            _client = new Client(credentials.Key, credentials.Secret, proto:"http", port:80);
            _queue = _client.Queue(queueName);
        }


        public void Enqueue(string data)
        {
            _queue.Enqueue(data);
        }


        public void Dequeue(Action<string> onDataReceived)
        {
            do
            {
                var msg = _queue.Dequeue();
                if (msg == null) break;

                onDataReceived(msg.Body);

                _queue.Delete(msg);
            } while (true);

            onDataReceived(null);
        }
    }
}
