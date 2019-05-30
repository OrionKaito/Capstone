using AutoMapper;
using Capstone.Model;
using Capstone.Service;
using Capstone.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class RequestValuesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IRequestValueService _requestValueService;

        public RequestValuesController(IMapper mapper, IRequestValueService requestValueService)
        {
            _mapper = mapper;
            _requestValueService = requestValueService;
        }

        // POST: api/requestValues
        [HttpPost]
        public ActionResult<RequestValue> PostRequestValue(RequestValueCM model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                RequestValue requestValue = new RequestValue();
                requestValue = _mapper.Map<RequestValue>(model);
                _requestValueService.Create(requestValue);
                _requestValueService.Save();
                return StatusCode(201, "RequestValue Type Created!");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/RequestValues
        [HttpGet]
        public ActionResult<IEnumerable<RequestValue>> GetRequestValues()
        {
            try
            {
                List<RequestValueVM> result = new List<RequestValueVM>();
                var data = _requestValueService.GetAll();
                foreach (var item in data)
                {
                    result.Add(_mapper.Map<RequestValueVM>(item));
                }
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/RequestValues/GetByID
        [HttpGet("GetByID")]
        public ActionResult<RequestValue> GetRequestValue(Guid ID)
        {
            try
            {
                var rs = _requestValueService.GetByID(ID);
                if (rs == null) return BadRequest("ID not found!");
                RequestValueVM result = _mapper.Map<RequestValueVM>(rs);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // PUT: api/RequestValues
        [HttpPut]
        public IActionResult PutRequestValue(RequestValueUM model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var requestValueInDb = _requestValueService.GetByID(model.ID);
                if (requestValueInDb == null) return BadRequest("ID not found!");
                _mapper.Map(model, requestValueInDb);
                _requestValueService.Save();
                return Ok("success");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
