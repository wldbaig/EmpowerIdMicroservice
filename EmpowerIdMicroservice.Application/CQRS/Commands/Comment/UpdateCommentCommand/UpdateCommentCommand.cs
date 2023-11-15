using MediatR;

namespace EmpowerIdMicroservice.Application.CQRS.Commands.Comment
{
    public record UpdateCommentCommand : IRequest<int>
    {
        public string Text { get; set; }
        public string Author { get; set; } 
        public int CommentId { get; set; }
    }
}
