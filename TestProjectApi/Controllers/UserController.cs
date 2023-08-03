using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using TestProjectApi.Data;
using TestProjectApi.Models;

namespace TestProjectApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }
       
        [HttpGet("getall")]
        public IActionResult GetUser()
        {

            return Ok();
        }

        
        [HttpPost("register")]
        public IActionResult Register(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();

            
            var factory = new ConnectionFactory() { Uri = new Uri("amqps://ffyblixq:WrJErT5jwSUNkYFBmJrZgMyoUIGL1akT@stingray.rmq.cloudamqp.com/ffyblixq") };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "message_queue",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);
                var jsonUser = JsonSerializer.Serialize(user);
                var body = Encoding.UTF8.GetBytes(jsonUser);
                channel.BasicPublish(exchange: "",
                                     routingKey: "message_queue",
                                     basicProperties: null,
                                     body: body);
            }
            // RabbitMQ.Client or Masstransit
            // method for send message

            return Ok();
        }
    }
}
