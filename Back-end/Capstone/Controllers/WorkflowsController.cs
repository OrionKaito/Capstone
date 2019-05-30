using AutoMapper;
using Capstone.Model;
using Capstone.Service;
using Capstone.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Capstone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkflowsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IWorkFlowService _workFlowService;

        public WorkflowsController(IMapper mapper, IWorkFlowService workFlowService)
        {
            _mapper = mapper;
            _workFlowService = workFlowService;
        }

        // GET: api/Workflows
        [HttpGet]
        public ActionResult<IEnumerable<WorkFlow>> GetWorkflows()
        {
            try
            {
                List<WorkflowVM> result = new List<WorkflowVM>();
                var workFlow = new WorkflowVM();
                var data = _workFlowService.GetAll();
                foreach (var item in data)
                {
                    result.Add(_mapper.Map<WorkflowVM>(item));
                }
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
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
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                WorkFlow workFlow = new WorkFlow();
                try
                {
                    workFlow = _mapper.Map<WorkFlow>(model);
                    _workFlowService.Create(workFlow);
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
                return CreatedAtRoute("GetWorkflow", workFlow);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
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
