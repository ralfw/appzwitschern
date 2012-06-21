using System;
using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;
using az.security;

namespace az.sqsapi
{
    public class SQSOperations
    {
        protected readonly AmazonSQS _sqs;
        private readonly string _queueName;
        protected string _queueUrl;

        public SQSOperations(string queueName, Token awsCredentials) {
            _queueName = queueName;
            _sqs = AWSClientFactory.CreateAmazonSQSClient(awsCredentials.Key, awsCredentials.Secret);
        }

        public void Enqueue(string data) {
            if (_queueUrl == null) {
                Create_queue();
            }

            var sendMessageRequest = new SendMessageRequest {
                QueueUrl = _queueUrl,
                MessageBody = data
            };
            _sqs.SendMessage(sendMessageRequest);
        }

        public void Dequeue(Action<string> onDataReceived) {
            if (_queueUrl == null) {
                Create_queue();
            }

            var n_messages_received = 0;
            do {
                n_messages_received = 0;

                var receiveMessageResponse = _sqs.ReceiveMessage(new ReceiveMessageRequest { QueueUrl = _queueUrl, MaxNumberOfMessages = 10 });
                var receiveMessageResult = receiveMessageResponse.ReceiveMessageResult;
                foreach (var message in receiveMessageResult.Message) {
                    onDataReceived(message.Body);
                    Delete_message(message.ReceiptHandle);
                    n_messages_received++;
                }
            } while (n_messages_received > 0);

            onDataReceived(null);
        }

        private void Create_queue() {
            var createQueueResponse = _sqs.CreateQueue(new CreateQueueRequest { QueueName = _queueName });
            _queueUrl = createQueueResponse.CreateQueueResult.QueueUrl;
        }

        private void Delete_message(string receiptHandle) {
            var deleteRequest = new DeleteMessageRequest {
                QueueUrl = _queueUrl,
                ReceiptHandle = receiptHandle
            };
            _sqs.DeleteMessage(deleteRequest);
        }
    }
}