using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Common;
using Lykke.Common.ApiLibrary.Extensions;
using Lykke.Service.Resources.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using DeleteGroupResourceItemModel = Lykke.Service.Resources.Models.DeleteGroupResourceItemModel;
using DeleteGroupResourceModel = Lykke.Service.Resources.Models.DeleteGroupResourceModel;
using ErrorResponse = Lykke.Service.Resources.Models.ErrorResponse;
using GroupResource = Lykke.Service.Resources.Core.Domain.GroupResources.GroupResource;
using GroupResourceModel = Lykke.Service.Resources.Models.GroupResourceModel;
using GroupResourcesModel = Lykke.Service.Resources.Models.GroupResourcesModel;

namespace Lykke.Service.Resources.Controllers
{
    [Route("api/groupresources")]
    public class GroupResourcesController : Controller
    {
        private readonly IGroupResourcesService _groupResourcesService;

        public GroupResourcesController(
            IGroupResourcesService groupResourcesService
            )
        {
            _groupResourcesService = groupResourcesService;
        }
        
        /// <summary>
        /// Gets all group resources
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation("GetAllGroupResources")]
        [ProducesResponseType(typeof(IEnumerable<GroupResource>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.NotFound)]
        public IActionResult GetAllResources()
        {
            var resource = _groupResourcesService.GetAll();
            
            if (resource == null)
                return NotFound();
            
            return Ok(resource);
        }
        
        /// <summary>
        /// Gets group resource by name
        /// </summary>
        /// <param name="name">full name of the resource</param>
        /// <returns></returns>
        [HttpGet("{name}/resource")]
        [SwaggerOperation("GetGroupResource")]
        [ProducesResponseType(typeof(GroupResource), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.NotFound)]
        public IActionResult GetResource(string name)
        {
            if (!name.IsValidPartitionOrRowKey())
                return BadRequest(ErrorResponse.Create($"Invalid {nameof(name)} value"));

            var resource = _groupResourcesService.Get(name);
            
            if (resource == null)
                return NotFound();
            
            return Ok(resource);
        }
        
        /// <summary>
        /// Gets section of group resources
        /// </summary>
        /// <remarks>Returns group resources in the specified section, for example: assetDetails will return all group resources under this section (assetDetails.iconLinks, assetDetails.headerLinks etc.)</remarks>
        /// <param name="name">full name of the section</param>
        /// <returns></returns>
        [HttpGet("{name}/section")]
        [SwaggerOperation("GetGroupResourceSection")]
        [ProducesResponseType(typeof(IEnumerable<GroupResource>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.NotFound)]
        public IActionResult GetGroupResourceSection(string name)
        {
            if (!name.IsValidPartitionOrRowKey())
                return BadRequest(ErrorResponse.Create($"Invalid {nameof(name)} value"));

            var resources = _groupResourcesService.GetGroup(name).ToList();

            if (resources.Count == 0)
                return NotFound();
            
            return Ok(resources);
        }
        
        /// <summary>
        /// Adds group resources
        /// </summary>
        /// <param name="model">list of group resources</param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerOperation("AddGroupResources")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddResources([FromBody]GroupResourcesModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ErrorResponse.Create(ModelState.GetErrorMessage()));

            if (!model.Name.IsValidPartitionOrRowKey())
                return BadRequest(ErrorResponse.Create($"Invalid {nameof(model.Name)} value"));

            var group = _groupResourcesService.Get(model.Name);

            if (group != null)
                return BadRequest(ErrorResponse.Create($"'{model.Name}' group is already exists"));

            if (model.Values.Length == 0)
                return BadRequest(ErrorResponse.Create($"{nameof(model.Values)} is empty"));
            
            if (model.Values.Any(item => string.IsNullOrWhiteSpace(item.Id)))
                return BadRequest(ErrorResponse.Create("Id can't be empty"));
            
            if (model.Values.Any(item => string.IsNullOrWhiteSpace(item.Value)))
                return BadRequest(ErrorResponse.Create("Value can't be empty")); 

            var ids = model.Values.Select(item => item.Id).Distinct().ToArray();
            
            if (ids.Length != model.Values.Length)
                return BadRequest(ErrorResponse.Create("Id must be unique"));

            await _groupResourcesService.AddAsync(model.Name, model.Values);
            return Ok();
        }
        
        /// <summary>
        /// Adds group resource item
        /// </summary>
        /// <param name="model">group resource item</param>
        /// <returns></returns>
        [HttpPost("item")]
        [SwaggerOperation("AddGroupResourceItem")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddResourceItem([FromBody]GroupResourceModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ErrorResponse.Create(ModelState.GetErrorMessage()));

            if (!model.Name.IsValidPartitionOrRowKey())
                return BadRequest(ErrorResponse.Create($"Invalid {nameof(model.Name)} value"));
            
            if (string.IsNullOrWhiteSpace(model.Value.Id))
                return BadRequest(ErrorResponse.Create("Id can't be empty"));
            
            if (string.IsNullOrWhiteSpace(model.Value.Value))
                return BadRequest(ErrorResponse.Create("Value can't be empty"));

            var group = _groupResourcesService.Get(model.Name);
            
            if (group != null && group.Value.Any(item => item.Id == model.Value.Id))
                return BadRequest(ErrorResponse.Create($"Item with Id '{model.Value.Id}' is already added to the group '{model.Name}'"));

            await _groupResourcesService.AddItemAsync(model.Name, model.Value);
            return Ok();
        }
        
        /// <summary>
        /// Deletes group resource
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpDelete]
        [SwaggerOperation("DeleteGroupResource")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteResource([FromBody]DeleteGroupResourceModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ErrorResponse.Create(ModelState.GetErrorMessage()));

            if (!model.Name.IsValidPartitionOrRowKey())
                return BadRequest(ErrorResponse.Create($"Invalid {nameof(model.Name)} value"));

            var res = _groupResourcesService.Get(model.Name);

            if (res == null)
                return BadRequest($"Group resource with name '{model.Name}' not found");
            
            await _groupResourcesService.DeleteAsync(model.Name);
            
            return Ok();
        }
        
        /// <summary>
        /// Deletes group resource item
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpDelete("item")]
        [SwaggerOperation("DeleteGroupResourceItem")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteResourceItem([FromBody]DeleteGroupResourceItemModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ErrorResponse.Create(ModelState.GetErrorMessage()));

            if (!model.Name.IsValidPartitionOrRowKey())
                return BadRequest(ErrorResponse.Create($"Invalid {nameof(model.Name)} value"));

            var res = _groupResourcesService.Get(model.Name);

            if (res == null)
                return BadRequest($"Group resource with name '{model.Name}' not found");
            
            await _groupResourcesService.DeleteItemAsync(model.Name, model.Id);
            
            return Ok();
        }
    }
}
