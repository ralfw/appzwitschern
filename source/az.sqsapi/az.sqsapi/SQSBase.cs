using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;
using az.security;
using npantarhei.runtime.patterns;

namespace az.sqsapi
{
    public class SQSBase : AOperation
    {
        protected readonly AmazonSQS _sqs;
        private readonly string _queueName;
        protected string _queueUrl;

        protected SQSBase(string name, string queueName, Token awsCredentials) : base(name)
        {
            _queueName = queueName;
            _sqs = AWSClientFactory.CreateAmazonSQSClient(awsCredentials.Key, awsCredentials.Secret);
        }

        protected void Create_queue()
        {
            var createQueueResponse = _sqs.CreateQueue(new CreateQueueRequest {QueueName = _queueName});
            _queueUrl = createQueueResponse.CreateQueueResult.QueueUrl;
        }
    }
}