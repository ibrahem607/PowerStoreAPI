using Microsoft.AspNetCore.Mvc;
using PowerStore.Core.Contract.Errors;
#nullable enable
namespace PowerStore.APIs.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorsController : BaseApiController
    {

        [HttpGet]
        public async Task<IActionResult> NotFound()
        {
            return NotFound(new ApiResponse(404));
        }


    }
}
