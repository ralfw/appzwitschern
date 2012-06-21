using System;
using Amazon.SQS.Model;
using az.security;
using npantarhei.runtime.contract;
using Message = npantarhei.runtime.messagetypes.Message;

namespace az.sqsapi
{
    public class SQSDequeue : SQSBase
    {
        public SQSDequeue(string queueName, Token credentials) : this("SQSDequeue", queueName, credentials) {}
        public SQSDequeue(string name, string queueName, Token credentials) : base(name, queueName, credentials) {}

        protected override void Process(IMessage input, Action<IMessage> continueWith, Action<FlowRuntimeException> unhandledException)
        {
            if (_queueUrl == null) Create_queue();
            Dequeue_message(continueWith);
        }

        private void Dequeue_message(Action<IMessage> continueWith)
        {
            var n_messages_received = 0;
            do
            {
                n_messages_received = 0;

                var receiveMessageResponse = _sqs.ReceiveMessage(new ReceiveMessageRequest {QueueUrl = _queueUrl, MaxNumberOfMessages = 10});
                var receiveMessageResult = receiveMessageResponse.ReceiveMessageResult;
                foreach (var message in receiveMessageResult.Message)
                {
                    continueWith(new Message(base.Name, message.Body));
                    Delete_message(message.ReceiptHandle);
                    n_messages_received++;
                }
            } while (n_messages_received > 0);
        }

        private void Delete_message(string receiptHandle)
        {
            var deleteRequest = new DeleteMessageRequest {
                QueueUrl = _queueUrl, 
                ReceiptHandle = receiptHandle
            };
            _sqs.DeleteMessage(deleteRequest);
        }
    }
}
