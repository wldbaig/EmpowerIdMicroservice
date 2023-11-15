using MediatR;

namespace EmpowerIdMicroservice.Application.CQRS.Queries.Comment
{
    public record GetCommentByIdQuery :IRequest<GetCommentByIdResult>
    {
        public int Id { get; set; }
    }
}
