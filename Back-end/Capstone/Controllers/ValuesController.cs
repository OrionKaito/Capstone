using Capstone.Service;
using Capstone.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Capstone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public ValuesController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        // GET api/values
        [HttpGet]
        [Authorize]
        public ActionResult<IEnumerable<string>> Get()
        {
            var currentUSer = HttpContext.User;
            var permissions = currentUSer.Claims.FirstOrDefault(c => c.Type == Helper.WebConstant.Permissions).Value;
            var userId = currentUSer.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            return new string[] { permissions, userId };
        }

        // GET api/values/5
        [HttpGet("SendEmail")]
        public ActionResult<string> SendEmail()
        {
            _emailService.SendMail("orionkaito@gmail.com", "Hello", "Hello world");
            return "success";
        }

        // GET api/values/5
        [HttpPost("CheckConnection")]
        public ActionResult<string> CheckConnection(CheckConnectionVM model)
        {
            var vertices = model.TotalVertice; //Tổng số đỉnh
            List<int>[] adjency; //Các cạnh của các đỉnh

            adjency = new List<int>[vertices];
            for (int i = 0; i < vertices; i++)
            {
                adjency[i] = new List<int>();
            }

            foreach (var connection in model.Connections)
            {
                adjency[connection.From].Add(connection.To);
            }

            bool[] visited = new bool[vertices];

            //For DFS use stack
            Stack<int> stack = new Stack<int>();
            visited[model.Root] = true;
            stack.Push(model.Root);

            string message = "";

            for (int i = 0; i < vertices; i++)
            {
                message += i + ":[";
                string str = "";
                foreach (var k in adjency[i])
                {
                    str = str + (k + ",");
                }
                str = str.Substring(0, str.Length - 1);
                str = str + "]\n";
                message += str;
            }

            message += "\n";

            while (stack.Count != 0)
            {
                var root = stack.Pop();
                message += "next -> " + root + "\n";
                foreach (int i in adjency[root]) //lấy tất cả các cạnh của đỉnh hiện tại mà chưa visit
                {
                    if (!visited[i])
                    {
                        visited[i] = true;
                        stack.Push(i);
                    }
                }
            }

            //Check vertices that not connect

            for (int i = 0; i < visited.Length; i++)
            {
                if (!visited[i])
                {
                    message += "Error: \n";
                    message += i;
                    foreach (var item in adjency[i])
                    {
                        message += "[" + item + "]";
                    }
                }
            }

            return message;
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
