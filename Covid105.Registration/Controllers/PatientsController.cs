using Covid105.Registration.Model;
using Covid105.Registration.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Covid105.Registration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PatientsController : ControllerBase
    {
        private readonly PatientsContext _context;
        private readonly MessagesSender _sender;
        public PatientsController(PatientsContext context, MessagesSender sender)
        {
            _context = context;
            _sender = sender;
        }

        [HttpGet]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(List<Patient>))]
        public IActionResult GetAllPatients()
        {
            return Ok(_context.Patients.ToList());
        }

        [HttpPost]
        [SwaggerResponse(StatusCodes.Status201Created, Type = typeof(Patient))]
        public async Task<IActionResult> CreatePatient(Patient patient)
        {
            _context.Patients.Add(patient);
            _context.SaveChanges();

            await _sender.SendMessage(new MessagePayload() 
            { EventName = "PatientCreated", PatientEmail = "email@email.email"});

            return Created($"/api/patients/{patient.Id}", patient);
        }

        [HttpGet("/invalid")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(string))]
        public IActionResult Invalid()
        {
            throw new InvalidOperationException("Problem");
        }
    }
}
