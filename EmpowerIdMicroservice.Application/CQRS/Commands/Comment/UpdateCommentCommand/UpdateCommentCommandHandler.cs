using EmpowerIdMicroservice.Application.Services;
using EmpowerIdMicroservice.Domain.AggregateModules.CommentAggregate;
using MediatR;

namespace EmpowerIdMicroservice.Application.CQRS.Commands.Comment
{
    public class UpdateCommentCommandHandler : IRequestHandler<UpdateCommentCommand, int>
    {
        private readonly ICommentRepository _commentRepository;
        private readonly ICacheService _cacheService;
        public UpdateCommentCommandHandler(ICommentRepository commentRepository, ICacheService cacheService)
        {
            _commentRepository = commentRepository;
            _cacheService = cacheService;
        }
         
        public async Task<int> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
        {
            try
            { 
                var comment = await _commentRepository.GetCommentByIdAsync(request.CommentId);
                comment.Update(request.Text, request.Author);
                int result = await _commentRepository.UpdateAsync(comment);

                _cacheService.RemoveData($"commentByPostId-{comment.PostId}");
                _cacheService.RemoveData($"commentById-{comment.CommentId}"); 
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
