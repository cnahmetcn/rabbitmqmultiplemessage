using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;

namespace RabbitMQApp.Subscriber
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

            /*
             * globallik durumu false olursa belirttiğimiz mesaj sayısının 
             * (prefetchCount) her bir subscriber a atanması durumudur. Yani biz
             * prefetchCount a 6 verirsek her bir subscriber a 6 mesaj gider.
             * globallik durumu true olursa kaç tane subscriber varsa mesajları
             * tek seferde toplam sayısı 6 olacak şekilde gönderir. Yani 2 
             * subscriber varsa 3-3 gönderir, 3 subscriber varsa 2-2-2 şeklinde
             * gönderir.
             */
            channel.BasicQos(0, 1, false);
            var consumer = new EventingBasicConsumer(channel);


            channel.BasicConsume("work-queue",false,consumer);

            consumer.Received += (object sender, BasicDeliverEventArgs e) =>
            {
                var message = Encoding.UTF8.GetString(e.Body.ToArray());
                Thread.Sleep(1500);
                Console.WriteLine("Incoming Message - Subscriber: " + message);

                channel.BasicAck(e.DeliveryTag, false);
            };

            Console.ReadLine();
        }

        
    }
}
