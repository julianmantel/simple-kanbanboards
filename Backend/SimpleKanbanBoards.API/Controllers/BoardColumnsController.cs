using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleKanbanBoards.Business.Models;
using SimpleKanbanBoards.Business.Models.BoardColumn;
using SimpleKanbanBoards.Business.Service.IService;

namespace SimpleKanbanBoards.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Project Manager")]
    public class BoardColumnsController : ControllerBase
    {
        private readonly IBoardColumnService _boardColumnService;

        public BoardColumnsController(IBoardColumnService boardColumnService)
        {
            _boardColumnService = boardColumnService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBoardColumnById(int id)
        {
            var boardColumn = await _boardColumnService.GetBoardColumnByIdAsync(id);
            if (boardColumn == null)
            {
                return NotFound();
            }
            return Ok(ApiResult<BoardColumnModel>.Success(boardColumn));
        }

        [HttpPost]
        public async Task<IActionResult> CreateBoardColumn([FromBody] CreateBoardColumnModel createBoardColumnModel)
        {
            await _boardColumnService.CreateBoardColumnAsync(createBoardColumnModel);
            return Ok(ApiResult<string>.Success("Board column created successfully"));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBoardColumn(int id)
        {
            await _boardColumnService.DeleteBoardColumnAsync(id);
            return Ok(ApiResult<string>.Success("Board column deleted successfully"));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateBoardColumn([FromBody] UpdateBoardColumnModel updateBoardColumnModel)
        {
            await _boardColumnService.UpdateBoardColumnAsync(updateBoardColumnModel);
            return Ok(ApiResult<string>.Success("Board column updated successfully"));
        }
    }
}
