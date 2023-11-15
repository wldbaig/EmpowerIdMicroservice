namespace EmpowerIdMicroservice.Domain.AggregateModules.CommentAggregate
{
    public interface ICommentRepository
    {
        Task<Comment> GetCommentByIdAsync(int id);
        Task<IEnumerable<Comment>> GetCommentByPostIdAsync(int postId);
        Task<int> CreateAsync(Comment comment);
        Task<int> UpdateAsync(Comment comment);
    }
}
