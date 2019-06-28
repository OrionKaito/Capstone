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

        [HttpPost("CheckConnectionv2")]
        public ActionResult<string> CheckConnectionv2(CheckConnectionVM model)
        {
            var g = new Graph();

            foreach (var connection in model.Connections)
            {
                g.AddEdge(connection.From, connection.To);
            }

            int count = 0;
            int root = 0;

            foreach (var node in model.Nodes)
            {
                if (node.IsStart)
                {
                    count++;
                    root = node.NodeName;
                }
            }

            if (count > 1)
            {
                return Ok("Therer are more than 2 root");
            }

            g.model = model;

            g.DFSWalkWithStartNode(root);
            g.PrintNotTravelNode();

            return g.getMessage();
        }

        public class Graph
        {
            public Dictionary<int, HashSet<int>> Adj { get; private set; }

            public HashSet<int> visited { get; private set; }

            public CheckConnectionVM model { get; set; }

            string message = "";
            string errorMessage = "";

            public string getMessage()
            {
                errorMessage = string.IsNullOrEmpty(errorMessage) ? "\nCheck IsEnd : Success" : "\n Check IsEnd : Error :" + errorMessage;
                return message + errorMessage;
            }

            public Graph()
            {
                Adj = new Dictionary<int, HashSet<int>>();
                visited = new HashSet<int>();
            }

            public void AddEdge(int source, int target)
            {
                if (Adj.ContainsKey(source))
                {
                    try
                    {
                        Adj[source].Add(target);
                    }
                    catch
                    {
                        message += "This edge already exists: " + source + " to " + target;
                    }
                }
                else
                {
                    var hs = new HashSet<int>();
                    hs.Add(target);
                    Adj.Add(source, hs);
                }
            }

            public void DFSWalkWithStartNode(int vertex)
            {
                // Mark this node as visited
                visited.Add(vertex);
                // Stack for DFS
                var s = new Stack<int>();
                // Add this node to the stack
                s.Push(vertex);

                while (s.Count > 0)
                {
                    var current = s.Pop();
                    message += "[" + current + "] => ";
                    // ADD TO VISITED HERE
                    if (!visited.Contains(current))
                    {
                        visited.Add(current);
                    }
                    // Only if the node has a any adj notes
                    if (Adj.ContainsKey(current))
                    {
                        // Iterate through UNVISITED nodes
                        foreach (int neighbour in Adj[current].Where(a => !visited.Contains(a)))
                        {
                            visited.Add(neighbour);
                            s.Push(neighbour);
                        }
                    }
                    else
                    {
                        var isEnd = model.Nodes.Where(x => x.NodeName.Equals(current)).Select(x => x.IsEnd).FirstOrDefault();
                        if (!isEnd)
                        {
                            errorMessage += "This node is not end [" + current + "]";
                        }
                    }

                }
            }

            public void PrintNotTravelNode()
            {
                message += "Node can't travel : ";
                for (int i = 0; i < Adj.Count; i++)
                {
                    if (!visited.Contains(Adj.ElementAt(i).Key))
                    {
                        message += "[" + Adj.ElementAt(i).Key.ToString() + "] ";
                    }
                }
            }
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
