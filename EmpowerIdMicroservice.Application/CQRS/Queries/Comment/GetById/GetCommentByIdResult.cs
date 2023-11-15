namespace EmpowerIdMicroservice.Application.CQRS.Queries.Comment
{
    public record GetCommentByIdResult
    {
        public int PostId { get; set; }
        public string Author { get; set; }
        public string Text { get; set; }
    }
}
