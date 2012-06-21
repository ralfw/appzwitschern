﻿using System;
using Amazon.SQS.Model;
using az.security;
using npantarhei.runtime.contract;

namespace az.sqsapi
{
    public class SQSEnqueue : SQSBase
    {
        public SQSEnqueue(string queueName, Token credentials) : this("SQSEnqueue", queueName, credentials) {}
        public SQSEnqueue(string name, string queueName, Token credentials) : base(name, queueName, credentials) {}

        protected override void Process(IMessage input, Action<IMessage> continueWith, Action<FlowRuntimeException> unhandledException)
        {
            if (_queueUrl == null) Create_queue();
            Enqueue_message((string)input.Data);
        }

        private void Enqueue_message(string data)
        {
            var sendMessageRequest = new SendMessageRequest {
                QueueUrl = _queueUrl, 
                MessageBody = data 
            };
            _sqs.SendMessage(sendMessageRequest);
        }
    }
}