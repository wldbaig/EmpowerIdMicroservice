using MediatR;

namespace EmpowerIdMicroservice.Application.CQRS.Commands.Comment
{
    public record CreateCommentCommand : IRequest<int>
    {
        public string Text { get; set; }
        public string Author { get; set; } 
        public int PostId { get; set; }
    }
}
