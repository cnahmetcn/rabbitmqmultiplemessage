﻿using RabbitMQ.Client;
using System;
using System.Linq;
using System.Text;

namespace RabbitMQApp.Publisher
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqps://myntuobw:GYL7dAkJfH19crSe7jAVI9fjKBwbfrWK@clam.rmq.cloudamqp.com/myntuobw"); // Normalde bu bilgiyi appsetting.json da tutmalıyız

            using var connection = factory.CreateConnection();

            var channel = connection.CreateModel(); // Bu kanal üzerinden RabbitMQ ile haberleşebiliriz

            channel.QueueDeclare("work-queue", true, false, false);

            Enumerable.Range(1, 50).ToList().ForEach(x =>
             {
                 string message = $"Message {x}";

                 var messageBody = Encoding.UTF8.GetBytes(message);

                 channel.BasicPublish(string.Empty, "work-queue", null, messageBody);

                 Console.WriteLine($"Message Sent - Publisher: { message}");
             });


            Console.ReadLine();
        }
    }
}
