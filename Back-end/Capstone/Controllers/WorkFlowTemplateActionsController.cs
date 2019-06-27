using AutoMapper;
using Capstone.Helper;
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
    public class WorkFlowTemplateActionsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IWorkFlowTemplateActionService _workFlowTemplateActionService;

        public WorkFlowTemplateActionsController(IMapper mapper, IWorkFlowTemplateActionService workFlowTemplateActionService)
        {
            _mapper = mapper;
            _workFlowTemplateActionService = workFlowTemplateActionService;
        }

        // GET: api/WorkFlowTemplateActions
        [HttpGet]
        public ActionResult<IEnumerable<WorkFlowTemplateActionVM>> GetWorkFlowTemplateActions()
        {
            try
            {
                List<WorkFlowTemplateActionVM> result = new List<WorkFlowTemplateActionVM>();
                var workFlow = new WorkFlowTemplateVM();
                var data = _workFlowTemplateActionService.GetAll();
                foreach (var item in data)
                {
                    result.Add(_mapper.Map<WorkFlowTemplateActionVM>(item));
                }
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/WorkFlowTemplateActions/5
        [HttpGet("{id}")]
        public ActionResult<WorkFlowTemplateActionVM> GetWorkFlowTemplateAction(Guid ID)
        {
            try
            {
                var data = _workFlowTemplateActionService.GetByID(ID);
                if (data == null) return BadRequest(WebConstant.NotFound);
                WorkFlowTemplateActionVM result = _mapper.Map<WorkFlowTemplateActionVM>(data);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // PUT: api/WorkFlowTemplateActions/5
        [HttpPut("{id}")]
        public ActionResult PutWorkFlowTemplateAction(WorkFlowTemplateActionUM model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var workFlowTemplateActionInDb = _workFlowTemplateActionService.GetByID(model.ID);
                if (workFlowTemplateActionInDb == null) return BadRequest(WebConstant.NotFound);

                _mapper.Map<WorkFlowTemplateAction>(model);
                _workFlowTemplateActionService.Save();
                return Ok(WebConstant.Success);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // POST: api/WorkFlowTemplateActions
        [HttpPost]
        public ActionResult PostWorkFlowTemplateAction(WorkFlowTemplateActionCM model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                WorkFlowTemplateAction workFlowTemplateAction = new WorkFlowTemplateAction();
                if (_workFlowTemplateActionService.GetByName(model.Name) != null) return BadRequest("WorkflowTemplateAction "
                    + WebConstant.NameExisted);

                workFlowTemplateAction = _mapper.Map<WorkFlowTemplateAction>(model);
                _workFlowTemplateActionService.Create(workFlowTemplateAction);

                return StatusCode(201, workFlowTemplateAction.ID);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // DELETE: api/WorkFlowTemplateActions/5
        [HttpDelete("{id}")]
        public ActionResult DeleteWorkFlowTemplateAction(Guid ID)
        {
            try
            {
                var workFlowTemplateActionInDb = _workFlowTemplateActionService.GetByID(ID);
                if (workFlowTemplateActionInDb == null) return BadRequest(WebConstant.NotFound);

                workFlowTemplateActionInDb.IsDeleted = true;
                _workFlowTemplateActionService.Save();
                return Ok(WebConstant.Success);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
