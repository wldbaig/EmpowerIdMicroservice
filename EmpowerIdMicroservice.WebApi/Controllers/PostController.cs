using EmpowerIdMicroservice.WebApi.Model;
using EmpowerIdMicroservice.WebApi.Proto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmpowerIdMicroservice.WebApi.Controllers
{
    [ApiController]
    [Authorize]
    public class PostController : ControllerBase
    {
        private readonly PostGrpc.PostGrpcClient _postGrpcClient;

        public PostController(PostGrpc.PostGrpcClient postGrpcClient)
        {
            _postGrpcClient = postGrpcClient;
        }

        [HttpPost("api/Post")]
        public async Task<ActionResult<IdResponseMessage>> CreatePost([FromBody] CreatePost post)
        {
            try
            {
                var result = await _postGrpcClient.CreatePostAsync(new PostCreateMessage()
                {
                    Author = post.Author,
                    Content = post.Content,
                    IsPublished = post.IsPublished,
                    Title = post.Title,
                });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("api/Post")]
        public async Task<ActionResult<IdResponseMessage>> UpdatePost([FromBody] UpdatePost post)
        {
            try
            {
                var result = await _postGrpcClient.UpdatePostAsync(new PostUpdateMessage()
                {
                    PostId = post.PostId,
                    Author = post.Author,
                    Content = post.Content,
                    IsPublished = post.IsPublished,
                    Title = post.Title,
                });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("api/Post/{id}")]
        public async Task<ActionResult<CommentResponseMessage>> GetPostById(int id)
        {
            try
            {
                var result = await _postGrpcClient.GetPostByIdAsync(new IdRequestMessage { Id = id });
                 
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("api/Post/")]
        public async Task<ActionResult<CommentByPostResponseMessage>> GetCommentByPostId(int pageSize, int pageNumber)
        {
            try
            {
                var result = await _postGrpcClient.GetPostsAsync(new PaginationRequestMessage { PageNumber = pageNumber, PageSize = pageSize });
                 
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
