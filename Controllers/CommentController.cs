using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using api.Dtos.Comment;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{

    [Route("api/comment")]
    [ApiController]
    public class CommentController: ControllerBase
    {
        private readonly ICommentRepository _commentRepo;
        private readonly IStockRepository _stockRepo;

        public CommentController(ICommentRepository commentRepo,IStockRepository stockRepo)
        {
            _commentRepo= commentRepo;     
            _stockRepo= stockRepo;
        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllComments()
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var comments=await _commentRepo.GetAllAsync();
            var commentDto= comments.Select(c=>c.ToCommentDto());
            //ovde treba da se pozove toCommentDto da bismo izdvojili 
            return Ok(commentDto);
        }

        [HttpPost("{stockId:int}")]
        public async Task<IActionResult>Create([FromRoute] int stockId, CreateCommentDto commentDto)
        {
            //ovde hocemo da proverimo da li Stock postoji, ako ne postoji break, ako postoji idemo dalje
            //we want to check does Stock exist, if not , there is not sense to create Comment cuz we 
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            //can not attach it to stock
            Console.WriteLine("Stock id ima vrednost", stockId);
            if( !await _stockRepo.DoesExist(stockId))
            {
                return BadRequest("Stock is not found!");
            }

            var commentModel=commentDto.ToCommentFromCreate(stockId);
            await _commentRepo.SaveChangesAsync(commentModel);
            return Ok(commentModel.ToCommentDto());
           
        }

        [HttpPut]
        [Route("{commentID:int}")]
        public async Task<IActionResult> UpdateComment([FromRoute] int commentID, [FromBody] UpdateCommentRequestDto commentModel )
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var comment= await _commentRepo.UpdateCommentAsync(commentID, commentModel.ToCommentFromUpdate());
           if( comment == null )
           {
            return NotFound("Comment is not found!");
           }
            return Ok(comment.ToCommentDto());

        }
        [HttpDelete]
        [Route("{commentId:int}")]
        public async Task<IActionResult> ActionResult([FromRoute] int commentId)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
                
            var comment= await _commentRepo.DeleteCommentAsync(commentId);
            
            if(comment==null)
            {
                return NotFound("Comment is not found!");
            }

            return Ok(comment.ToCommentDto());
        }




    }
}