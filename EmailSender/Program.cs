using ChatAPI.Models;
using Microsoft.AspNetCore.SignalR.Client;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EmailSenderExample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            HubConnection connectionSignal = new HubConnectionBuilder().WithUrl("https://localhost:5001/messagehub").Build();
            await connectionSignal.StartAsync();

            ConnectionFactory factory = new ConnectionFactory();
            factory.Uri = new Uri("amqps://vrrsulcv:6VK-NYje94aAnVbGrD---QjosjrSr8nV@chimpanzee.rmq.cloudamqp.com/vrrsulcv");
            using IConnection connectionRabbit = factory.CreateConnection();
            using IModel channel = connectionRabbit.CreateModel();

            channel.QueueDeclare("messagequeue", false, false, false);
            
            

            EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
            channel.BasicConsume("messagequeue", true, consumer);
            consumer.Received += async  (s, e)  =>
            {
                //email operation
                //e.Body.Span
                string serializeData = Encoding.UTF8.GetString(e.Body.Span);
                User user = JsonSerializer.Deserialize<User>(serializeData);

                EmailSender.Send(user.Email, user.Message);
                Console.WriteLine($"{user.Email} mail gönderildi");
                await connectionSignal.InvokeAsync("SendMessageAsync", $"{user.Email} mail gönderildi");
                
            };

            Console.Read();
        }

        
    }
}
