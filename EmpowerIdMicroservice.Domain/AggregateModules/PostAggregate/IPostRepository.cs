namespace EmpowerIdMicroservice.Domain.AggregateModules.PostAggregate
{
    public interface IPostRepository
    {
        Task<(IEnumerable<Post> Items, int TotalCount)> GetAllPostsAsync(int pageSize, int pageNumber); 
        Task<Post> GetPostByIdAsync(int postId);
        Task<int> CreateAsync(Post post);
        Task<int> UpdateAsync(Post post);
    }
}
