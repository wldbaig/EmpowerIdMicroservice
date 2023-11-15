using MediatR;

namespace EmpowerIdMicroservice.Application.CQRS.Commands.Post.UpdatePostCommand
{
    public record UpdatePostCommand : IRequest<int>
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public bool IsPublished { get; set; }
    }
}
