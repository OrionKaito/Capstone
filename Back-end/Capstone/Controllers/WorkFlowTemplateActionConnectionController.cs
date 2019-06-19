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
    public class WorkFlowTemplateActionConnectionController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IWorkFlowTemplateActionConnectionService _workFlowTemplateActionConnectionService;

        public WorkFlowTemplateActionConnectionController(IMapper mapper,
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
    }
}