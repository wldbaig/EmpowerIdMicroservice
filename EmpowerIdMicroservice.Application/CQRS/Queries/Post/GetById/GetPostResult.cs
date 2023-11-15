using EmpowerIdMicroservice.Application.CQRS.Queries.Comment;

namespace EmpowerIdMicroservice.Application.CQRS.Queries.Post
{
    public record GetPostResult
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsPublished { get; set; }
        public List<CommentResult> Comments { get; set; }
    }
}
