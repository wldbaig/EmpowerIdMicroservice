using EmpowerIdMicroservice.Application.Services;
using EmpowerIdMicroservice.Domain.AggregateModules.PostAggregate;
using MediatR;

namespace EmpowerIdMicroservice.Application.CQRS.Queries.Post
{
    public class GetPostByIdQueryHandler : IRequestHandler<GetPostByIdQuery, GetPostResult>
    {
        private readonly IPostRepository _postRepository;
        private readonly ICacheService _cacheService;

        public GetPostByIdQueryHandler(IPostRepository postRepository, ICacheService cacheService)
        {
            _postRepository = postRepository;
            _cacheService = cacheService;
        }
        public async Task<GetPostResult> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
        {
            var cachedData = _cacheService.GetData<GetPostResult>($"postById-{request.Id}");

            if (cachedData != null)
            {
                return cachedData;
            }

            var post = await _postRepository.GetPostByIdAsync(request.Id) ?? throw new Exception("No record found");

            var result = new GetPostResult()
            {
                Author = post.Author,
                Content = post.Content,
                CreatedAt = post.CreatedAt,
                UpdatedAt = post.UpdatedAt,
                IsPublished = post.IsPublished,
                PostId = post.PostId,
                Title = post.Title,
                Comments = post.Comments.Select(comment => new Comment.CommentResult
                {
                    Author = comment.Author,
                    CommentId = comment.CommentId,
                    Text = comment.Text
                }).ToList()
            };

            var expiryTime = DateTimeOffset.Now.AddMinutes(5);
            _cacheService.SetData<GetPostResult>($"postById-{request.Id}", result, expiryTime);  

            return result;
        }
    }
}
