using EmpowerIdMicroservice.WebApi.Model;
using EmpowerIdMicroservice.WebApi.Proto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmpowerIdMicroservice.WebApi.Controllers
{

    [ApiController]
    [Authorize]
    public class CommentController : ControllerBase
    {
        private readonly CommentGrpc.CommentGrpcClient _commentGrpcClient;

        public CommentController(CommentGrpc.CommentGrpcClient commentGrpcClient)
        {
            _commentGrpcClient = commentGrpcClient;
        }

        [HttpPost("api/Comment")]
        public async Task<ActionResult<IdResponseMessage>> CreateComment([FromBody] CreateComment comment)
        {
            try
            {
                var result = await _commentGrpcClient.CreateCommentAsync(new CommentCreateMessage()
                {
                    Author = comment.Author,
                    PostId = comment.PostId,
                    Text = comment.Text,
                });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("api/Comment")]
        public async Task<ActionResult<IdResponseMessage>> UpdateComment([FromBody] UpdateComment comment)
        {
            try
            {
                var result = await _commentGrpcClient.UpdateCommentAsync(new CommentUpdateMessage()
                {
                    CommentId = comment.CommentId,
                    Author = comment.Author,
                    Text = comment.Text,
                });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("api/Comment/{id}")]
        public async Task<ActionResult<CommentResponseMessage>> GetCommentById(int id)
        {
            try
            {
                var result = await _commentGrpcClient.GetCommentByIdAsync(new IdRequestMessage { Id = id });
                 
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("api/Comment/Post/{postId}")]
        public async Task<ActionResult<CommentByPostResponseMessage>> GetCommentByPostId(int postId)
        {
            try
            {
                var result = await _commentGrpcClient.GetCommentByPostIdAsync(new IdRequestMessage { Id = postId });
                 
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
