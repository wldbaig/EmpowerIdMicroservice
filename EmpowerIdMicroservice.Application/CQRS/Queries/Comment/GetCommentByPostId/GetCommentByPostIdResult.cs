namespace EmpowerIdMicroservice.Application.CQRS.Queries.Comment
{
    public record GetCommentByPostIdResult
    {
        public List<CommentResult> Comments { get; set; }
    }

    public record CommentResult
    {
        public int CommentId { get; set; }
        public string Author { get; set; }
        public string Text { get; set; }
    }
}
