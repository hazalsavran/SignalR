using ChatAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System;
using System.Text;
using System.Text.Json;

namespace ChatAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        [HttpPost()]
        public IActionResult Post(User model)
        {

            ConnectionFactory factory = new ConnectionFactory();
            factory.Uri = new Uri("amqps://vrrsulcv:6VK-NYje94aAnVbGrD---QjosjrSr8nV@chimpanzee.rmq.cloudamqp.com/vrrsulcv");
            using  IConnection connection = factory.CreateConnection();
            using IModel channel = connection.CreateModel();

            channel.QueueDeclare("messagequeue",false,false,false);

            string serializeData = JsonSerializer.Serialize(model);

            byte[] data = Encoding.UTF8.GetBytes(serializeData);
            channel.BasicPublish("", "messagequeue",body: data);
            return Ok();
        }
    }
}
