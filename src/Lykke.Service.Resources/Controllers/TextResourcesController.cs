using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Common;
using Lykke.Common.ApiLibrary.Extensions;
using Lykke.Service.Resources.Core.Domain.TextResources;
using Lykke.Service.Resources.Core.Services;
using Lykke.Service.Resources.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Lykke.Service.Resources.Controllers
{
    [Route("api/[controller]")]
    public class TextResourcesController : Controller
    {
        private readonly ITextResourcesService _textResourcesService;

        public TextResourcesController(
            ITextResourcesService textResourcesService
            )
        {
            _textResourcesService = textResourcesService;
        }
        
        [HttpGet("{lang}/{name}")]
        [SwaggerOperation("GetTextResource")]
        [ProducesResponseType(typeof(TextResource), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.NotFound)]
        public IActionResult GetResource(string lang, string name)
        {
            var resource = _textResourcesService.Get(lang, name);
            
            if (resource == null)
                return NotFound();
            
            return Ok(resource);
        }
        
        [HttpGet("all/{lang}/{name}")]
        [SwaggerOperation("GetTextResources")]
        [ProducesResponseType(typeof(IEnumerable<TextResource>), (int)HttpStatusCode.OK)]
        public IActionResult GetAll(string lang, string name)
        {
            var resources = _textResourcesService.GetAll(lang, name);
            return Ok(resources);
        }
        
        [HttpPost("add")]
        [SwaggerOperation("AddTextResource")]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddResource([FromBody]TextResourceModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessage());

            if (!model.Lang.IsValidPartitionOrRowKey())
                return BadRequest($"Invalid {nameof(model.Lang)} value");
            
            if (!model.Name.IsValidPartitionOrRowKey())
                return BadRequest($"Invalid {nameof(model.Name)} value");
            
            await _textResourcesService.AddAsync(model.Lang, model.Name, model.Value);
            return Ok();
        }
        
        [HttpPost("delete")]
        [SwaggerOperation("DeleteTextResource")]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteResource([FromBody]DeleteTextResourceModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessage());

            if (!model.Lang.IsValidPartitionOrRowKey())
                return BadRequest($"Invalid {nameof(model.Lang)} value");
            
            if (!model.Name.IsValidPartitionOrRowKey())
                return BadRequest($"Invalid {nameof(model.Name)} value");
            
            await _textResourcesService.DeleteAsync(model.Lang, model.Name);
            return Ok();
        }
    }
}
