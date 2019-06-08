using AutoMapper;
using Capstone.Helper;
using Capstone.Model;
using Capstone.Service;
using Capstone.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

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
        public ActionResult<WorkFlow> GetWorkflow(Guid ID)
        {
            try
            {
                var data = _workFlowService.GetByID(ID);
                if (data == null) return BadRequest(WebConstant.NotFound);
                WorkflowVM result = _mapper.Map<WorkflowVM>(data);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // PUT: api/Workflows/5
        [HttpPut("{id}")]
        public ActionResult PutWorkflow(WorkflowUM model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var workFlowInDb = _workFlowService.GetByID(model.WorkFlowID);
                if (workFlowInDb == null) return BadRequest(WebConstant.NotFound);

                var userID = HttpContext.User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier).Value;
                if (workFlowInDb.OwnerID != userID) return BadRequest(WebConstant.AccessDined);

                _mapper.Map<WorkFlow>(model);
                _workFlowService.Save();
                return Ok(WebConstant.Success);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // POST: api/Workflows
        [HttpPost]
        public ActionResult<WorkFlow> PostWorkflow(WorkflowCM model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                WorkFlow workFlow = new WorkFlow();
                if (_workFlowService.GetByName(model.Name) != null) return BadRequest("Workflow" + WebConstant.NameExisted);

                var userID = HttpContext.User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier).Value;

                workFlow = _mapper.Map<WorkFlow>(model);
                workFlow.OwnerID = userID;
                _workFlowService.Create(workFlow);

                return StatusCode(201, workFlow.WorkFlowID);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // DELETE: api/Workflows/5
        [HttpDelete("{id}")]
        public ActionResult DeleteWorkflow(Guid ID)
        {
            try
            {
                var workFlowInDb = _workFlowService.GetByID(ID);
                if (workFlowInDb == null) return BadRequest(WebConstant.NotFound);

                workFlowInDb.IsDeleted = true;
                _workFlowService.Save();
                return Ok(WebConstant.Success);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
