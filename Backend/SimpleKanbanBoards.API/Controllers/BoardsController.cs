using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleKanbanBoards.Business.Models;
using SimpleKanbanBoards.Business.Models.Board;
using SimpleKanbanBoards.Business.Service.IService;

namespace SimpleKanbanBoards.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Project Manager, Developer")]
    public class BoardsController : ControllerBase
    {
        private readonly IBoardService _boardService;

        public BoardsController(IBoardService boardService)
        {
            _boardService = boardService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBoardById(int id)
        {
            var board = await _boardService.GetBoardByIdAsync(id);
            if (board == null)
            {
                return NotFound();
            }
            return Ok(ApiResult<BoardModel>.Success(board));
        }

        [HttpPost]
        public async Task<IActionResult> CreateBoard([FromBody] BoardModel createBoardModel)
        {
            await _boardService.CreateBoardAsync(createBoardModel);

            return Ok(ApiResult<string>.Success("Board created successfull"));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> ToggleBoard(int id)
        {
            await _boardService.ToggleBoardAsync(id);

            return Ok(ApiResult<string>.Success("Board toggled successfully"));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBoard([FromBody] UpdateBoardModel updateBoardModel)
        {
            await _boardService.UpdateBoardAsync(updateBoardModel);
            return Ok(ApiResult<string>.Success("Board updated successfully"));
        }
    }
}
