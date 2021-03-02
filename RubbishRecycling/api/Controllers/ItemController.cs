using Microsoft.AspNetCore.Mvc;
using RubbishRecyclingAU.ControllerModels;
using RubbishRecyclingAU.Services;
using System.Threading.Tasks;

namespace RubbishRecyclingAU.Controllers
{
    [Route("api/[controller]")]
    public class ItemController : Controller
    {
        private readonly IItemService _itemService;

        public ItemController(IItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post([FromBody] ItemDetails itemDetails)
        {
            var result = await _itemService.AddItem(itemDetails);
            if (!result.CanProceed)
            {
                return BadRequest(new { result.Message });
            }
            return Ok(result);
        }
    }
}
