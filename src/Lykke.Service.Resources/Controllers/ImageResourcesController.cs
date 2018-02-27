using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Lykke.Common.ApiLibrary.Extensions;
using Lykke.Service.Resources.Core.Domain.ImageResources;
using Lykke.Service.Resources.Core.Services;
using Lykke.Service.Resources.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Lykke.Service.Resources.Controllers
{
    [Route("api/[controller]")]
    public class ImageResourcesController : Controller
    {
        private readonly IImageResourcesService _imageResourcesService;

        public ImageResourcesController(
            IImageResourcesService imageResourcesService
            )
        {
            _imageResourcesService = imageResourcesService;
        }
        
        [HttpGet]
        [SwaggerOperation("GetAllImageResources")]
        [ProducesResponseType(typeof(IEnumerable<ImageResource>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.NotFound)]
        public IActionResult GetAllResources()
        {
            var resource = _imageResourcesService.GetAll();
            
            if (resource == null)
                return NotFound();
            
            return Ok(resource);
        }
        
        [HttpGet("{name}")]
        [SwaggerOperation("GetImageResource")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.NotFound)]
        public IActionResult GetResource(string name)
        {
            var resource = _imageResourcesService.Get(name);
            
            if (resource == null)
                return NotFound();
            
            return Ok(resource);
        }
        
        [HttpPost("add")]
        [SwaggerOperation("AddImageResource")]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddResource([FromBody]ImageResourceModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessage());

            if (model.Data.Length == 0)
                return BadRequest($"Invalid {nameof(model.Data)} value");
            
            await _imageResourcesService.AddAsync(model.Name, model.Data);
            return Ok();
        }
        
        [HttpPost("delete")]
        [SwaggerOperation("DeleteImageResource")]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteResource(string name)
        {
            if (string.IsNullOrEmpty(name))
                return BadRequest();

            await _imageResourcesService.DeleteAsync(name);
            return Ok();
        }
    }
}
