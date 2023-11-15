using EmpowerIdMicroservice.Application.Services;
using EmpowerIdMicroservice.Domain.AggregateModules.PostAggregate;
using MediatR;

namespace EmpowerIdMicroservice.Application.CQRS.Commands.Post
{
    public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, int>
    {
        private readonly IPostRepository _postRepository;
        private readonly ICacheService _cacheService;
        public CreatePostCommandHandler(IPostRepository postRepository, ICacheService cacheService)
        {
            _postRepository = postRepository;
            _cacheService = cacheService;
        }

        public async Task<int> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var post = new Domain.AggregateModules.PostAggregate.Post(request.Title, request.Content, DateTime.UtcNow, request.Author, request.IsPublished);

                int result = await _postRepository.CreateAsync(post);
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
