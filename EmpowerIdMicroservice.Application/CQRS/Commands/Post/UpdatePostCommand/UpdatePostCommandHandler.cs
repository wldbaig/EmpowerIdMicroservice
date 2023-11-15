using EmpowerIdMicroservice.Application.Services;
using EmpowerIdMicroservice.Domain.AggregateModules.PostAggregate;
using MediatR;

namespace EmpowerIdMicroservice.Application.CQRS.Commands.Post.UpdatePostCommand
{
    public class UpdatePostCommandHandler : IRequestHandler<UpdatePostCommand, int>
    {
        private readonly IPostRepository _postRepository;
        private readonly ICacheService _cacheService;
        public UpdatePostCommandHandler(IPostRepository postRepository, ICacheService cacheService)
        {
            _postRepository = postRepository;
            _cacheService = cacheService;
        }

        public async Task<int> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var post = await _postRepository.GetPostByIdAsync(request.PostId);
                post.Update(request.Title, request.Content, DateTime.UtcNow, request.Author, request.IsPublished);
                int result = await _postRepository.UpdateAsync(post);

                _cacheService.RemoveData($"postById-{post.PostId}");

                return result;
            }
            catch (Exception ex)
            {
                // todo: log exception
                throw ex;
            }
        }
    }
}
