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
    public class RequestsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IRequestService _requestService;

        public RequestsController(IMapper mapper, IRequestService requestService)
        {
            _mapper = mapper;
            _requestService = requestService;
        }

        // POST: api/Requests
        [HttpPost]
        public ActionResult<Request> PostRequest(RequestCM model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                Request request = new Request();
                request = _mapper.Map<Request>(model);
                _requestService.Create(request);
                _requestService.Save();
                return StatusCode(201, request.ID);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/Requests
        [HttpGet]
        public ActionResult<IEnumerable<Request>> GetRequests()
        {
            try
            {
                List<RequestVM> result = new List<RequestVM>();
                var data = _requestService.GetAll();
                foreach (var item in data)
                {
                    result.Add(_mapper.Map<RequestVM>(item));
                }
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/Requests/GetByID
        [HttpGet("GetByID")]
        public ActionResult<Request> GetRequest(Guid ID)
        {
            try
            {
                var rs = _requestService.GetByID(ID);
                if (rs == null) return BadRequest("ID not found!");
                RequestVM result = _mapper.Map<RequestVM>(rs);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // PUT: api/Requests
        [HttpPut]
        public IActionResult PutRequest(RequestUM model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var requestInDb = _requestService.GetByID(model.ID);
                if (requestInDb == null) return BadRequest("ID not found!");
                _mapper.Map(model, requestInDb);
                _requestService.Save();
                return Ok("success");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


    }
}
