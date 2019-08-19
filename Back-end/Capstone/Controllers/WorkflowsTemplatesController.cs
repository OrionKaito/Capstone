using AutoMapper;
using Capstone.Model;
using Capstone.Service;
using Capstone.Service.Helper;
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
    public class WorkflowsTemplatesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IWorkFlowTemplateActionService _workFlowTemplateActionService;
        private readonly IWorkFlowTemplateActionConnectionService _workFlowTemplateActionConnectionService;
        private readonly IConnectionTypeService _connectionTypeService;
        private readonly IWorkFlowTemplateService _workFlowService;
        private readonly IPermissionService _permissionService;
        private readonly IRoleService _roleService;

        public WorkflowsTemplatesController(IMapper mapper
            , IWorkFlowTemplateService workFlowService
            , IPermissionService permissionService
            , IWorkFlowTemplateActionService workFlowTemplateActionService
            , IWorkFlowTemplateActionConnectionService workFlowTemplateActionConnectionService
            , IRoleService roleService
            , IConnectionTypeService connectionTypeService)
        {
            _mapper = mapper;
            _workFlowService = workFlowService;
            _permissionService = permissionService;
            _workFlowTemplateActionService = workFlowTemplateActionService;
            _workFlowTemplateActionConnectionService = workFlowTemplateActionConnectionService;
            _roleService = roleService;
            _connectionTypeService = connectionTypeService;
        }

        // GET: api/Workflows/5
        [HttpGet("GetByID")]
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

        [HttpGet("GetWorkflowToUse")]
        public ActionResult<IEnumerable<WorkFlowTemplatePaginVM>> GetWorkflowToUse(int? numberOfPage, int? NumberOfRecord)
        {
            try
            {
                var page = numberOfPage ?? 1;
                var count = NumberOfRecord ?? WebConstant.DefaultPageRecordCount;

                var userID = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
                var permissionsOfUser = _permissionService.GetByUserID(userID);

                List<WorkFlowTemplateVM> workFlowTemplates = new List<WorkFlowTemplateVM>();

                foreach (var item in permissionsOfUser)
                {
                    var workflows = _workFlowService.GetByPermissionToUse(item.ID);

                    foreach (var workflow in workflows)
                    {
                        workFlowTemplates.Add(_mapper.Map<WorkFlowTemplateVM>(workflow));
                    }
                }

                workFlowTemplates.OrderByDescending(w => w.CreateDate);

                WorkFlowTemplatePaginVM result = new WorkFlowTemplatePaginVM
                {
                    TotalRecord = workFlowTemplates.Count(),
                    WorkFlowTemplates = workFlowTemplates.Skip((page - 1) * count).Take(count),
                };

                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("SearchWorkflowToUse")]
        public ActionResult<IEnumerable<WorkFlowTemplatePaginVM>> SearchWorkflowToUse(int? numberOfPage, int? NumberOfRecord, string search)
        {
            try
            {
                var page = numberOfPage ?? 1;
                var count = NumberOfRecord ?? WebConstant.DefaultPageRecordCount;

                var userID = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
                var permissionsOfUser = _permissionService.GetByUserID(userID);

                List<WorkFlowTemplateVM> workFlowTemplates = new List<WorkFlowTemplateVM>();

                foreach (var item in permissionsOfUser)
                {
                    var workflows = _workFlowService.GetByPermissionToUse(item.ID);

                    foreach (var workflow in workflows)
                    {
                        workFlowTemplates.Add(_mapper.Map<WorkFlowTemplateVM>(workflow));
                    }
                }

                workFlowTemplates.OrderByDescending(w => w.CreateDate);

                WorkFlowTemplatePaginVM result = new WorkFlowTemplatePaginVM
                {
                    TotalRecord = workFlowTemplates.Count(),
                    WorkFlowTemplates = workFlowTemplates.Skip((page - 1) * count).Take(count),
                };

                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("GetWorkflowToEdit")]
        public ActionResult<WorkFlowTemplatePaginVM> GetWorkflowToEdit(int? numberOfPage, int? NumberOfRecord)
        {
            try
            {
                var page = numberOfPage ?? 1;
                var count = NumberOfRecord ?? WebConstant.DefaultPageRecordCount;

                var userID = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
                var role = _roleService.GetByUserID(userID);

                List<WorkFlowTemplateVM> workFlowTemplates = new List<WorkFlowTemplateVM>();
                if (role.Name.Equals(WebConstant.Staff))
                {
                    var workflows = _workFlowService.GetAll();

                    foreach (var workflow in workflows)
                    {
                        workFlowTemplates.Add(_mapper.Map<WorkFlowTemplateVM>(workflow));
                    }
                }
                else
                {
                    return StatusCode(403, WebConstant.AccessDined);
                }

                var workflowTemplatesToEdit = workFlowTemplates.OrderByDescending(w => w.CreateDate);

                WorkFlowTemplatePaginVM result = new WorkFlowTemplatePaginVM
                {
                    TotalRecord = _workFlowService.CountByIsDeleted(),
                    WorkFlowTemplates = workflowTemplatesToEdit.Skip((page - 1) * count).Take(count),
                };

                return Ok(result);
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
                workFlow.CreateDate = DateTime.Now;
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

            if (workFlowInDb.IsCheckConnection == false)
            {
                return BadRequest(WebConstant.ToggleWorkflowFail);
            }

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
                _workFlowService.Save();
                return Ok(WebConstant.Success);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("SaveWorkflow")]
        public ActionResult SaveWorkflow(SaveWowkFlowTemplateUM model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                _workFlowService.BeginTransaction();
                bool checkUpdate = false;
                var workFlowInDB = _workFlowService.GetByID(model.WorkFlowTemplateID);

                var actionsInDb = _workFlowTemplateActionService.GetByWorkFlowID(model.WorkFlowTemplateID);
                if (actionsInDb.Any()) //Kiểm tra wokrflow có action nào không
                {
                    checkUpdate = true;
                    //Disable workflow cũ
                    var workFlowInDb = _workFlowService.GetByID(model.WorkFlowTemplateID);
                    workFlowInDb.IsDeleted = true;
                    workFlowInDb.IsEnabled = false;
                    _workFlowService.Save();

                    //Tạo workflow mới
                    WorkFlowTemplate workFlow = new WorkFlowTemplate
                    {
                        Data = model.Data,
                        Description = workFlowInDB.Description,
                        Name = workFlowInDB.Name,
                        OwnerID = workFlowInDB.OwnerID,
                        PermissionToUseID = workFlowInDB.PermissionToUseID,
                        CreateDate = DateTime.Now,
                        IsCheckConnection = true
                    };
                    _workFlowService.Create(workFlow);

                    model.WorkFlowTemplateID = workFlow.ID; //cập nhật id là của workflow mới
                }
                else
                {
                    workFlowInDB.IsCheckConnection = true;
                    workFlowInDB.Data = model.Data;
                }

                foreach (var action in model.Actions)
                {
                    var workFlowTemplateAction = _mapper.Map<WorkFlowTemplateAction>(action);
                    workFlowTemplateAction.WorkFlowTemplateID = model.WorkFlowTemplateID;
                    _workFlowTemplateActionService.Create(workFlowTemplateAction);
                }

                foreach (var connection in model.Connections)
                {
                    var connectionTypeInDb = _connectionTypeService.GetByName(connection.Name);

                    if (connectionTypeInDb == null) // kiểm tra connection type này có chưa
                    {
                        ConnectionType connectionType = new ConnectionType
                        {
                            Name = connection.Name,
                        };
                        _connectionTypeService.Create(connectionType);
                        connectionTypeInDb = connectionType;
                    }

                    if (connection.TimeInterval < 0)
                    {
                        return BadRequest(WebConstant.InvalidTimeInterval);
                    }
                    else
                    {
                        WorkFlowTemplateActionConnection workflowConnection = new WorkFlowTemplateActionConnection
                        {
                            ConnectionTypeID = connectionTypeInDb.ID,
                            FromWorkFlowTemplateActionID = connection.FromWorkFlowTemplateActionID,
                            ToWorkFlowTemplateActionID = connection.ToWorkFlowTemplateActionID,
                            TimeInterval = connection.TimeInterval,
                            Type = connection.Type
                        };

                        _workFlowTemplateActionConnectionService.Create(workflowConnection);
                    }
                }

                _workFlowService.CommitTransaction();

                if (checkUpdate)
                {
                    return StatusCode(201, "Update " + WebConstant.Success);
                }
                else
                {
                    return Ok(WebConstant.Success);

                }
            }
            catch (Exception e)
            {
                _workFlowService.RollBack();
                return BadRequest(e.Message);
            }
        }

        [HttpPut("SaveDraft")]
        public ActionResult SaveDraft(SaveCraftTemplateUM model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var workFlowInDb = _workFlowService.GetByID(model.ID);
                if (workFlowInDb == null) return BadRequest(WebConstant.NotFound);

                workFlowInDb.Data = model.Data;
                workFlowInDb.Name = model.Name;
                workFlowInDb.Description = model.Description;
                workFlowInDb.PermissionToUseID = model.PermissionToUseID;
                workFlowInDb.Icon = model.Icon;
                workFlowInDb.IsViewDetail = model.IsViewDetail;
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
