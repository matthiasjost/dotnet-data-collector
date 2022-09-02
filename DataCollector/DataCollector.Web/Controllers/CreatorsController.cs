using DataCollector.Core;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DataCollector.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreatorsController : ControllerBase
    {
        private ICreatorListService _creatorListService;

        public CreatorsController(ICreatorListService creatorListService)
        {
            _creatorListService = creatorListService;
        }

        // GET: api/<CreatorsController>
        [HttpGet]
        public async Task<IEnumerable<CreatorDto>> GetAllCreators()
        {
            return await _creatorListService.FillDtoListByDatabase();
        }
        // GET: api/<CreatorsController>
        [HttpGet]
        [Route("search")]
        public async Task<IEnumerable<CreatorDto>> GetSearchCreators(string searchString)
        {
            return await _creatorListService.FillDtoListByDatabase(searchString);
        }
    }
}
