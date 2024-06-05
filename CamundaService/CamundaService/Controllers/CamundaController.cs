using CamundaApp.Interfaces;
using CamundaApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CamundaApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CamundaController : ControllerBase
    {
        private ICamundaService _camundaService;
        public CamundaController(ICamundaService camundaService)
        {
            _camundaService = camundaService;
        }

        [HttpPost("/start")]
        public async Task<ActionResult> Start([FromBody]StartProcess variables)
        {
            long processInstanceId = await _camundaService.Start(JsonSerializer.Serialize(variables));

            return Ok(processInstanceId); 
        }
    }
}
