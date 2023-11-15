using MediatR;

namespace EmpowerIdMicroservice.Application.CQRS.Commands.Post
{
    public record CreatePostCommand : IRequest<int>
    { 
        public string Title { get; set; }
        public string Content { get; set; } 
        public string Author { get; set; }
        public bool IsPublished { get; set; }
    }
}
