using EmpowerIdMicroservice.Application.Services;
using EmpowerIdMicroservice.Domain.AggregateModules.CommentAggregate;
using MediatR;

namespace EmpowerIdMicroservice.Application.CQRS.Commands.Comment
{
    public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, int>
    {
        private readonly ICommentRepository _commentRepository;
        private readonly ICacheService _cacheService;

        public CreateCommentCommandHandler(ICommentRepository commentRepository, ICacheService cacheService)
        {
            _commentRepository = commentRepository;
            _cacheService = cacheService;
        }
         
        public async Task<int> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var comment = new Domain.AggregateModules.CommentAggregate.Comment(request.Text, request.Author, request.PostId);

                int result = await _commentRepository.CreateAsync(comment);
                 
                _cacheService.RemoveData($"postById-{comment.PostId}");

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
