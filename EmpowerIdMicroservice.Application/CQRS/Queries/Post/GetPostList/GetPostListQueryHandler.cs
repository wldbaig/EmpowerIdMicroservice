using EmpowerIdMicroservice.Application.CQRS.Queries.Comment;
using EmpowerIdMicroservice.Application.Services;
using EmpowerIdMicroservice.Domain.AggregateModules.PostAggregate;
using MediatR;

namespace EmpowerIdMicroservice.Application.CQRS.Queries.Post
{
    public class GetPostListQueryHandler : IRequestHandler<GetPostListQuery, GetPostListResult>
    {
        private readonly IPostRepository _postRepository;
        private readonly ICacheService _cacheService;

        public GetPostListQueryHandler(IPostRepository postRepository, ICacheService cacheService)
        {
            _postRepository = postRepository;
            _cacheService = cacheService;
        }

        public async Task<GetPostListResult> Handle(GetPostListQuery request, CancellationToken cancellationToken)
        {
            var (Items, TotalCount) = await _postRepository.GetAllPostsAsync(request.PageSize, request.PageNumber);

            if(Items == null)
            {
                throw new Exception("No record found"); 
            }

            var result = new GetPostListResult()
            {
                Total = TotalCount,
                Posts = Items.Select(post => new GetPostResult
                {
                    PostId = post.PostId,
                    Title = post.Title,
                    Content = post.Content,
                    Author = post.Author,
                    CreatedAt = post.CreatedAt,
                    UpdatedAt = post.UpdatedAt,
                    IsPublished = post.IsPublished,
                    Comments = post.Comments.Select(comment => new CommentResult
                    {
                        CommentId = comment.CommentId,
                        Author = comment.Author,
                        Text = comment.Text
                    }).ToList()
                }).ToList()
            };
             
            return result; 
        }
    }
}
