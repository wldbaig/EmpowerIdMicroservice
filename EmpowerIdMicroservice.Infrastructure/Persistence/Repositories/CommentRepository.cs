using EmpowerIdMicroservice.Domain.AggregateModules.CommentAggregate;
using Microsoft.EntityFrameworkCore;

namespace EmpowerIdMicroservice.Infrastructure.Persistence.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDbContext _context;

        public CommentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> CreateAsync(Comment comment)
        {
            _context.Comment.Add(comment);
            await _context.SaveChangesAsync();
            return comment.CommentId;
        }

        public async Task<Comment> GetCommentByIdAsync(int id)
        {
            return await _context.Comment.FirstOrDefaultAsync(c => c.CommentId == id);
        }

        public async Task<IEnumerable<Comment>> GetCommentByPostIdAsync(int postId)
        {
            return await _context.Comment.Where(c => c.PostId == postId).ToListAsync();
        }

        public async Task<int> UpdateAsync(Comment comment)
        {
            _context.Comment.Update(comment);
            await _context.SaveChangesAsync();
            return comment.CommentId;
        }
    }
}
