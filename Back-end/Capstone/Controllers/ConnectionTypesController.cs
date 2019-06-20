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
    public class ConnectionTypesController : ControllerBase
    {
        private readonly IConnectionTypeService _connectionTypeService;
        private readonly IMapper _mapper;

        public ConnectionTypesController(IConnectionTypeService connectionTypeService, IMapper mapper)
        {
            _connectionTypeService = connectionTypeService;
            _mapper = mapper;
        }

        [HttpPost]
        public ActionResult PostActionType(ConnectionTypeCM model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var nameExist = _connectionTypeService.GetByName(model.Name);
                if (nameExist != null) return BadRequest("ConnectionType" + WebConstant.NameExisted);

                ConnectionType connectionType = new ConnectionType();
                connectionType = _mapper.Map<ConnectionType>(model);
                _connectionTypeService.Create(connectionType);
                return StatusCode(201, connectionType.ID);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        public ActionResult<IEnumerable<ConnectionTypeVM>> GetActionTypes()
        {
            try
            {
                List<ConnectionTypeVM> result = new List<ConnectionTypeVM>();
                var data = _connectionTypeService.GetAll();
                foreach (var item in data)
                {
                    result.Add(_mapper.Map<ConnectionTypeVM>(item));
                }
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<ConnectionTypeVM> GetActionType(Guid ID)
        {
            try
            {
                var data = _connectionTypeService.GetByID(ID);
                if (data == null) return NotFound(WebConstant.NotFound);
                ConnectionTypeVM result = _mapper.Map<ConnectionTypeVM>(data);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        public IActionResult PutActionType(ConnectionTypeUM model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var connectionTypeInDb = _connectionTypeService.GetByID(model.ID);
                if (connectionTypeInDb == null) return NotFound(WebConstant.NotFound);

                var nameExist = _connectionTypeService.GetByName(model.Name);
                if (nameExist != null) return BadRequest("ConnectionType" + WebConstant.NameExisted);

                _mapper.Map(model, connectionTypeInDb);
                _connectionTypeService.Save();
                return Ok(WebConstant.Success);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete]
        public ActionResult DeleteActionType(Guid ID)
        {
            try
            {
                var connectionTypeInDb = _connectionTypeService.GetByID(ID);
                if (connectionTypeInDb == null) return NotFound(WebConstant.NotFound);
                connectionTypeInDb.IsDeleted = true;
                _connectionTypeService.Save();
                return Ok(WebConstant.Success);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}