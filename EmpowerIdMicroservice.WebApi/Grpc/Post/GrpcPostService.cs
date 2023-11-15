using EmpowerIdMicroservice.Application.CQRS.Commands.Post;
using EmpowerIdMicroservice.Application.CQRS.Commands.Post.UpdatePostCommand;
using EmpowerIdMicroservice.Application.CQRS.Queries.Post;
using EmpowerIdMicroservice.WebApi.Proto;
using EmpowerIdMicroservice.WebApi.Proto.Common;
using Grpc.Core;
using MediatR;

namespace EmpowerIdMicroservice.WebApi.Grpc.Post
{
    public class GrpcPostService : PostGrpc.PostGrpcBase
    {
        private readonly IMediator _mediator;

        public GrpcPostService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override async Task<IdResponseMessage> CreatePost(PostCreateMessage request, ServerCallContext context)
        {
            try
            {
                var command = new CreatePostCommand
                {
                    Author = request.Author,
                    Content = request.Content,
                    IsPublished = request.IsPublished,
                    Title = request.Title,
                };
                var result = await _mediator.Send(command);

                return new IdResponseMessage { Id = result };
            }
            catch (Exception ex) { throw ex; }
        }

        public override async Task<IdResponseMessage> UpdatePost(PostUpdateMessage request, ServerCallContext context)
        {
            try
            {
                var command = new UpdatePostCommand
                {
                    PostId = request.PostId ?? 0,
                    Author = request.Author,
                    Content = request.Content,
                    IsPublished = request.IsPublished,
                    Title = request.Title,
                };
                var result = await _mediator.Send(command);

                return new IdResponseMessage { Id = result };
            }
            catch (Exception ex) { throw ex; }
        }

        public override async Task<PostResponseMessage> GetPostById(IdRequestMessage request, ServerCallContext context)
        {
            var query = new GetPostByIdQuery() { Id = request.Id ?? 0 };

            var item = await _mediator.Send(query);

            if (item == null)
                return new PostResponseMessage();

            var message = new PostResponseMessage()
            {
                Author = item.Author,
                Content = item.Content,
                IsPublished = item.IsPublished,
                Title = item.Title
            };

            foreach (var comment in item.Comments)
            {
                message.Comments.Add(new CommentMessage()
                {
                    Author = comment.Author,
                    CommentId = comment.CommentId,
                    Text = comment.Text,
                });
            }

            return message;
        }

        public override async Task<PostListResponseMessage> GetPosts(PaginationRequestMessage request, ServerCallContext context)
        {
            var query = new GetPostListQuery() { PageNumber = request.PageNumber ?? 1, PageSize = request.PageSize ?? 10 };

            var item = await _mediator.Send(query);

            if (item == null)
                return new PostListResponseMessage();

            var message = new PostListResponseMessage()
            {
                Total = item.Total,
            };

            foreach (var post in item.Posts)
            {
                var postResponse = new PostResponseMessage()
                {
                    Author = post.Author,
                    Content = post.Content,
                    IsPublished = post.IsPublished,
                    Title = post.Title,
                };

                foreach (var comment in post.Comments)
                {
                    postResponse.Comments.Add(new CommentMessage()
                    {
                        Author = comment.Author,
                        CommentId = comment.CommentId,
                        Text = comment.Text,
                    });
                }

                message.Posts.Add(postResponse);
            }

            return message;
        }
    }
}
