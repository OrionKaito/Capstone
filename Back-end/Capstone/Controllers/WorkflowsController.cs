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
        public ActionResult<IEnumerable<WorkFlowTemplate>> GetWorkflows()
        {
            try
            {
                List<WorkFlowTemplateVM> result = new List<WorkFlowTemplateVM>();
                var workFlow = new WorkFlowTemplateVM();
                var data = _workFlowService.GetAll();
                foreach (var item in data)
                {
                    result.Add(_mapper.Map<WorkFlowTemplateVM>(item));
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
        public ActionResult<WorkFlowTemplate> GetWorkflow(Guid ID)
        {
            try
            {
                var data = _workFlowService.GetByID(ID);
                if (data == null) return BadRequest(WebConstant.NotFound);
                WorkFlowTemplateVM result = _mapper.Map<WorkFlowTemplateVM>(data);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // PUT: api/Workflows/5
        [HttpPut("{id}")]
        public ActionResult PutWorkflow(WorkFlowTemplateUM model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var workFlowInDb = _workFlowService.GetByID(model.WorkFlowTemplateID);
                if (workFlowInDb == null) return BadRequest(WebConstant.NotFound);

                var userID = HttpContext.User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier).Value;
                if (workFlowInDb.OwnerID != userID) return BadRequest(WebConstant.AccessDined);

                _mapper.Map<WorkFlowTemplate>(model);
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
        public ActionResult<WorkFlowTemplate> PostWorkflow(WorkFlowTemplateCM model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                WorkFlowTemplate workFlow = new WorkFlowTemplate();
                if (_workFlowService.GetByName(model.Name) != null) return BadRequest("Workflow" + WebConstant.NameExisted);

                var userID = HttpContext.User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier).Value;

                workFlow = _mapper.Map<WorkFlowTemplate>(model);
                workFlow.OwnerID = userID;
                _workFlowService.Create(workFlow);

                return StatusCode(201, workFlow.WorkFlowTemplateID);
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
