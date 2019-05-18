using Capstone.Model;
using Capstone.Service;
using Capstone.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Capstone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkflowsController : ControllerBase
    {
        private readonly IWorkFlowService _workFlowService;

        public WorkflowsController(IWorkFlowService workFlowService)
        {
            _workFlowService = workFlowService;
        }

        // GET: api/Workflows
        [HttpGet]
        public ActionResult<IEnumerable<WorkFlow>> GetWorkflows()
        {
            List<WorkflowVM> result = new List<WorkflowVM>();
            var workFlow = new WorkflowVM();
            var data = _workFlowService.GetAll();
            foreach (var item in data)
            {
                workFlow.WorkflowId = item.WorkFlowID;
                workFlow.Name = item.Name;
                result.Add(workFlow);
            }
            return Ok(result);
        }

        // GET: api/Workflows/5
        [HttpGet("{id}")]
        public ActionResult<WorkFlow> GetWorkflow(Guid id)
        {
            return null;
        }

        // PUT: api/Workflows/5
        [HttpPut("{id}")]
        public IActionResult PutWorkflow(Guid id, WorkFlow workflow)
        {
            return null;
        }

        // POST: api/Workflows
        [HttpPost]
        public ActionResult<WorkFlow> PostWorkflow(WorkflowCM model)
        {
            return null;
        }

        // DELETE: api/Workflows/5
        [HttpDelete("{id}")]
        public ActionResult<WorkFlow> DeleteWorkflow(Guid id)
        {
            return null;
        }

        //private bool WorkflowExists(Guid id)
        //{
        //    return _context.Workflows.Any(e => e.WorkFlowID == id);
        //}
    }
}
