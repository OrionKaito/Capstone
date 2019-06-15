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
    public class WorkflowsTemplateController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IWorkFlowTemplateService _workFlowService;

        public WorkflowsTemplateController(IMapper mapper, IWorkFlowTemplateService workFlowService)
        {
            _mapper = mapper;
            _workFlowService = workFlowService;
        }

        // GET: api/Workflows
        [HttpGet]
        public ActionResult<IEnumerable<WorkFlowTemplateVM>> GetWorkflowsTemplate()
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
        public ActionResult<WorkFlowTemplateVM> GetWorkflowTemplate(Guid ID)
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
        public ActionResult PutWorkflowTemplate(WorkFlowTemplateUM model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var workFlowInDb = _workFlowService.GetByID(model.ID);
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
        public ActionResult PostWorkflowTemplate(WorkFlowTemplateCM model)
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
                
                return StatusCode(201, workFlow.ID);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // DELETE: api/Workflows/5
        [HttpDelete("{id}")]
        public ActionResult DeleteWorkflowTemplate(Guid ID)
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

        [HttpPut("ToggleEnable")]
        public ActionResult ToggleEnable(Guid ID)
        {
            var workFlowInDb = _workFlowService.GetByID(ID);
            if (workFlowInDb == null) return BadRequest(WebConstant.NotFound);

            try
            {
                if (workFlowInDb.IsEnabled == true)
                {
                    workFlowInDb.IsEnabled = false;
                }
                else
                {
                    workFlowInDb.IsEnabled = true;
                }

                return Ok(WebConstant.Success);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
