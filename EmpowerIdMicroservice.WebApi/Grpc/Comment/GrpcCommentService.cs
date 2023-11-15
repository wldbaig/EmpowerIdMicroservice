using EmpowerIdMicroservice.WebApi.Proto;
using EmpowerIdMicroservice.WebApi.Proto.Common;
using Grpc.Core;
using MediatR;
using EmpowerIdMicroservice.Application.CQRS.Commands.Comment;
using EmpowerIdMicroservice.Application.CQRS.Queries.Comment;

namespace EmpowerIdMicroservice.WebApi.Grpc.Comment
{
    public class GrpcCommentService : CommentGrpc.CommentGrpcBase
    {
        private readonly IMediator _mediator;

        public GrpcCommentService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override async Task<IdResponseMessage> CreateComment(CommentCreateMessage request, ServerCallContext context)
        {
            try
            {
                var command = new CreateCommentCommand
                {
                    Author = request.Author,
                    PostId = request.PostId ?? 0,
                    Text = request.Text
                };
                var result = await _mediator.Send(command);

                return new IdResponseMessage { Id = result };
            }
            catch (Exception ex) { throw; }
        }

        public override async Task<IdResponseMessage> UpdateComment(CommentUpdateMessage request, ServerCallContext context)
        {
            try
            {
                var command = new UpdateCommentCommand
                {
                    CommentId = request.CommentId ?? 0,
                    Author = request.Author,
                    Text = request.Text
                };
                var result = await _mediator.Send(command);

                return new IdResponseMessage { Id = result };
            }
            catch (Exception ex) { throw; }
        }

        public override async Task<CommentResponseMessage> GetCommentById(IdRequestMessage request, ServerCallContext context)
        {
            var query = new GetCommentByIdQuery()
            {
                Id = request.Id ?? 0
            };

            var item = await _mediator.Send(query);

            if (item == null)
                return new CommentResponseMessage();

            var message = new CommentResponseMessage()
            {
                Author = item.Author,
                Text = item.Text,
                PostId = item.PostId,
            };

            return message;
        }

        public override async Task<CommentByPostResponseMessage> GetCommentByPostId(IdRequestMessage request, ServerCallContext context)
        {
            var query = new GetCommentByPostIdQuery()
            {
                Id = request.Id ?? 0,
            };

            var item = await _mediator.Send(query);

            if (item == null)
                return new CommentByPostResponseMessage();

            var message = new CommentByPostResponseMessage();

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
    }
}
