using MediatR;

namespace EmpowerIdMicroservice.Application.CQRS.Queries.Post
{
    public record GetPostListQuery : IRequest<GetPostListResult>
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        // other filters 
    }
}
