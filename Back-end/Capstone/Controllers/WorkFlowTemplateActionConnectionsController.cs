using AutoMapper;
using Capstone.Helper;
using Capstone.Model;
using Capstone.Service;
using Capstone.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Capstone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkFlowTemplateActionConnectionsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IWorkFlowTemplateActionConnectionService _workFlowTemplateActionConnectionService;

        public WorkFlowTemplateActionConnectionsController(IMapper mapper,
            IWorkFlowTemplateActionConnectionService workFlowTemplateActionConnectionService)
        {
            _mapper = mapper;
            _workFlowTemplateActionConnectionService = workFlowTemplateActionConnectionService;
        }

        // GET: api/WorkFlowTemplateActionConnection
        [HttpGet]
        public ActionResult<IEnumerable<WorkFlowTemplateActionConnectionVM>> GetWorkFlowTemplateActionConnections()
        {
            try
            {
                List<WorkFlowTemplateActionConnectionVM> result = new List<WorkFlowTemplateActionConnectionVM>();
                var workFlow = new WorkFlowTemplateVM();
                var data = _workFlowTemplateActionConnectionService.GetAll();
                foreach (var item in data)
                {
                    result.Add(_mapper.Map<WorkFlowTemplateActionConnectionVM>(item));
                }
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/WorkFlowTemplateActionConnection/5
        [HttpGet("{id}")]
        public ActionResult<WorkFlowTemplateActionConnectionVM> GetWorkFlowTemplateActionConnection(Guid ID)
        {
            try
            {
                var data = _workFlowTemplateActionConnectionService.GetByID(ID);
                if (data == null) return BadRequest(WebConstant.NotFound);
                WorkFlowTemplateActionConnectionVM result = _mapper.Map<WorkFlowTemplateActionConnectionVM>(data);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // PUT: api/WorkFlowTemplateActionConnection/5
        [HttpPut("{id}")]
        public ActionResult PutWorkflowTemplate(WorkFlowTemplateActionConnectionUM model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var workFlowInDb = _workFlowTemplateActionConnectionService.GetByID(model.ID);
                if (workFlowInDb == null) return BadRequest(WebConstant.NotFound);

                _mapper.Map<WorkFlowTemplateActionConnection>(model);
                _workFlowTemplateActionConnectionService.Save();
                return Ok(WebConstant.Success);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // POST: api/WorkFlowTemplateActionConnection
        [HttpPost]
        public ActionResult PostWorkflowTemplate(WorkFlowTemplateActionConnectionCM model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                WorkFlowTemplateActionConnection workFlowTemplateActionConnection = new WorkFlowTemplateActionConnection();
                workFlowTemplateActionConnection = _mapper.Map<WorkFlowTemplateActionConnection>(model);
                _workFlowTemplateActionConnectionService.Create(workFlowTemplateActionConnection);

                return StatusCode(201, workFlowTemplateActionConnection.ID);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // DELETE: api/WorkFlowTemplateActionConnection/5
        [HttpDelete("{id}")]
        public ActionResult DeleteWorkflowTemplate(Guid ID)
        {
            try
            {
                var workFlowTemplateActionConnectionInDb = _workFlowTemplateActionConnectionService.GetByID(ID);
                if (workFlowTemplateActionConnectionInDb == null) return BadRequest(WebConstant.NotFound);

                workFlowTemplateActionConnectionInDb.IsDeleted = true;
                _workFlowTemplateActionConnectionService.Save();
                return Ok(WebConstant.Success);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("CheckConnectionv2")]
        public ActionResult<string> CheckConnectionv2(CheckConnectionVM model)
        {
            var g = new Graph();

            //Check connection
            if (!model.Connections.Any())
            {
                return Ok("There are no connection!");
            }

            foreach (var connection in model.Connections)
            {
                g.AddEdge(connection.From, connection.To);
            }

            //Check root && nodes   
            int countRoot = 0;
            bool chkConnection = false;
            string errNode = "";
            int root = 0;

            foreach (var node in model.Nodes)
            {
                if (node.IsStart)
                {
                    countRoot++;
                    root = node.NodeName;

                    if (node.IsEnd)
                    {
                        return Ok(node.NodeName + " can't be start and end in the same time!");
                    }
                }

                //Check connection contain node
                //    foreach (var connection in model.Connections)
                //    {
                //        if (connection.From.Equals(node.NodeName) && connection.To.Equals(node.NodeName))
                //        {
                //            chkConnection = true;
                //        } else
                //        {
                //            errNode = node.NodeName.ToString();
                //        }
                //    }
            }

            //if (!chkConnection)
            //{
            //    return Ok(errNode + " is not has connection!");
            //}

            if (countRoot > 1)
            {
                return BadRequest("Therer are more than 2 root!");
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

            public CheckConnectionVM model { get; set; } // model for check node is end or not

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
                            if (visited.Contains(neighbour))
                            {
                                errorMessage += "This node is not end [" + current + "]";
                            }
                            else
                            {
                                visited.Add(neighbour);
                                s.Push(neighbour);
                            }
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
    }
}