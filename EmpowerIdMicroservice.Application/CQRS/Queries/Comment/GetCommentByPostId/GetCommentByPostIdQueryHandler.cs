using EmpowerIdMicroservice.Application.Services;
using EmpowerIdMicroservice.Domain.AggregateModules.CommentAggregate;
using MediatR;

namespace EmpowerIdMicroservice.Application.CQRS.Queries.Comment
{
    public class GetCommentByPostIdQueryHandler : IRequestHandler<GetCommentByPostIdQuery, GetCommentByPostIdResult>
    {
        private readonly ICommentRepository _commentRepository;
        private readonly ICacheService _cacheService;

        public GetCommentByPostIdQueryHandler(ICommentRepository commentRepository, ICacheService cacheService)
        {
            _commentRepository = commentRepository;
            _cacheService = cacheService;
        }

        public async Task<GetCommentByPostIdResult> Handle(GetCommentByPostIdQuery request, CancellationToken cancellationToken)
        {
            var cachedData = _cacheService.GetData<GetCommentByPostIdResult>($"commentByPostId-{request.Id}");

            if (cachedData != null)
            {
                return cachedData;
            }

            var comments = await _commentRepository.GetCommentByPostIdAsync(request.Id) ?? throw new Exception("No record found");

            var result = new GetCommentByPostIdResult() {Comments =  new List<CommentResult>() };

            foreach (var item in comments)
            {
                var commentResult = new CommentResult()
                {
                    Author = item.Author,
                    CommentId = item.CommentId,
                    Text = item.Text,
                };
                result.Comments.Add(commentResult);
            }

            var expiryTime = DateTimeOffset.Now.AddMinutes(5);
            _cacheService.SetData<GetCommentByPostIdResult>($"commentByPostId-{request.Id}", result, expiryTime);

            return result;
        }
    }
}
