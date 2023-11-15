using MediatR;

namespace EmpowerIdMicroservice.Application.CQRS.Queries.Comment
{
    public record GetCommentByPostIdQuery :IRequest<GetCommentByPostIdResult>
    {
        public int Id { get; set; }
    }
}
