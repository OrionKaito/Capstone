using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Capstone.Data;
using Capstone.Model;

namespace Capstone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkflowsController : ControllerBase
    {
        private readonly CapstoneEntities _context;

        public WorkflowsController(CapstoneEntities context)
        {
            _context = context;
        }

        // GET: api/Workflows
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Workflow>>> GetWorkflows()
        {
            return await _context.Workflows.ToListAsync();
        }

        // GET: api/Workflows/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Workflow>> GetWorkflow(double id)
        {
            var workflow = await _context.Workflows.FindAsync(id);

            if (workflow == null)
            {
                return NotFound();
            }

            return workflow;
        }

        // PUT: api/Workflows/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWorkflow(double id, Workflow workflow)
        {
            if (id != workflow.WorkflowID)
            {
                return BadRequest();
            }

            _context.Entry(workflow).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WorkflowExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Workflows
        [HttpPost]
        public async Task<ActionResult<Workflow>> PostWorkflow(Workflow workflow)
        {
            _context.Workflows.Add(workflow);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (WorkflowExists(workflow.WorkflowID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetWorkflow", new { id = workflow.WorkflowID }, workflow);
        }

        // DELETE: api/Workflows/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Workflow>> DeleteWorkflow(double id)
        {
            var workflow = await _context.Workflows.FindAsync(id);
            if (workflow == null)
            {
                return NotFound();
            }

            _context.Workflows.Remove(workflow);
            await _context.SaveChangesAsync();

            return workflow;
        }

        private bool WorkflowExists(double id)
        {
            return _context.Workflows.Any(e => e.WorkflowID == id);
        }
    }
}
