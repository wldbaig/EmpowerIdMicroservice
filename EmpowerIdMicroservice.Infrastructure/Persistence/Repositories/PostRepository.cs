using EmpowerIdMicroservice.Domain.AggregateModules.PostAggregate;
using Microsoft.EntityFrameworkCore;

namespace EmpowerIdMicroservice.Infrastructure.Persistence.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly ApplicationDbContext _context;

        public PostRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> CreateAsync(Post post)
        {
            _context.Post.Add(post);
            await _context.SaveChangesAsync();
            return post.PostId;
        }

        public async Task<(IEnumerable<Post> Items, int TotalCount)> GetAllPostsAsync(int pageSize, int pageNumber)
        {
            // Validate input parameters
            if (pageSize <= 0 || pageNumber <= 0)
            {
                // Handle invalid input, perhaps by throwing an exception
                throw new ArgumentException("Invalid pageSize or pageNumber");
            }

            // Calculate the number of items to skip based on the page number and page size
            int itemsToSkip = (pageNumber - 1) * pageSize;

            // Query posts with pagination
            var postsQuery = _context.Post.OrderByDescending(x => x.CreatedAt).AsQueryable();

            // Calculate total count before pagination
            int totalCount = await postsQuery.CountAsync();

            // Apply pagination
            var paginatedPosts = await postsQuery
                .Include(x => x.Comments)
                .Skip(itemsToSkip)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();

            // Return a tuple with the paginated posts and the total count
            return (paginatedPosts, totalCount);
        }

        public async Task<Post> GetPostByIdAsync(int postId)
        {
            return await _context.Post.AsNoTracking()
                .Include(x => x.Comments).FirstOrDefaultAsync(c => c.PostId == postId);
        }

        public async Task<int> UpdateAsync(Post post)
        {
            _context.Post.Update(post);
            await _context.SaveChangesAsync();
            return post.PostId;
        }
    }
}
