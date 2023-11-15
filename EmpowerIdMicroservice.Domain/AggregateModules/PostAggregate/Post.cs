using EmpowerIdMicroservice.Domain.AggregateModules.CommentAggregate;

namespace EmpowerIdMicroservice.Domain.AggregateModules.PostAggregate
{
    public class Post
    {
        private List<Comment> _comments;
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string Author { get; set; }
        public bool IsPublished { get; set; }

        public IEnumerable<Comment> Comments => _comments.AsReadOnly();

        public Post()
        {
            _comments = new List<Comment>();
        }

        public Post(string title, string content, DateTime createdAt, string author, bool isPublished)
        { 
            Title = title;
            Content = content;
            CreatedAt = createdAt; 
            Author = author;
            IsPublished = isPublished;
        }

        public void Update(string title, string content, DateTime? updatedAt, string author, bool isPublished)
        { 
            Title = title;
            Content = content; 
            UpdatedAt = updatedAt;
            Author = author;
            IsPublished = isPublished;
        }

        public void Published()
        {
            IsPublished = true;
        }
    }
}
