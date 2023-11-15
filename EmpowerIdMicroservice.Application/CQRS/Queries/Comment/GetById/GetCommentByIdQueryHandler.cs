using EmpowerIdMicroservice.Application.Services;
using EmpowerIdMicroservice.Domain.AggregateModules.CommentAggregate;
using MediatR;

namespace EmpowerIdMicroservice.Application.CQRS.Queries.Comment
{
    public class GetCommentByIdQueryHandler : IRequestHandler<GetCommentByIdQuery, GetCommentByIdResult>
    {
        private readonly ICommentRepository _commentRepository;
        private readonly ICacheService _cacheService;
        public GetCommentByIdQueryHandler(ICommentRepository commentRepository, ICacheService cacheService)
        {
            _commentRepository = commentRepository;
            _cacheService = cacheService;
        }

        public async Task<GetCommentByIdResult> Handle(GetCommentByIdQuery request, CancellationToken cancellationToken)
        {
            var cachedData = _cacheService.GetData<GetCommentByIdResult>($"commentById-{request.Id}");

            if (cachedData != null)
            {
                return cachedData;
            }

            var comment = await _commentRepository.GetCommentByIdAsync(request.Id);

            if (comment == null)
            {
                throw new Exception("No record found");
            }

            var result = new GetCommentByIdResult()
            {
                Author = comment.Author,
                PostId = comment.PostId,
                Text = comment.Text,
            };

            var expiryTime = DateTimeOffset.Now.AddMinutes(5);
            _cacheService.SetData<GetCommentByIdResult>($"commentById-{request.Id}", result, expiryTime); 

            return result;
        }
    }
}
