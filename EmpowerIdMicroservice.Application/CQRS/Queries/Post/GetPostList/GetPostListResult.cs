namespace EmpowerIdMicroservice.Application.CQRS.Queries.Post
{
    public record GetPostListResult
    {
        public int Total { get; set; }
        public List<GetPostResult> Posts { get; set; }
    }
}
