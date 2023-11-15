using MediatR; 

namespace EmpowerIdMicroservice.Application.CQRS.Queries.Post
{
    public record GetPostByIdQuery : IRequest<GetPostResult>
    {
        public int Id { get; set; }
    }
}
