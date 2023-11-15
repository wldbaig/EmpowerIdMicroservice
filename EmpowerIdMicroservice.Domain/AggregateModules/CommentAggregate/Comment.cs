using EmpowerIdMicroservice.Domain.AggregateModules.PostAggregate;

namespace EmpowerIdMicroservice.Domain.AggregateModules.CommentAggregate
{
    public class Comment
    {
        public int CommentId { get; private set; }
        public string Text { get; private set; }
        public string Author { get; private set; }
        public int PostId { get; private set; }
        public Post Post { get; private set; }

        public Comment(string text, string author, int postId)
        {
            Text = text;
            Author = author;
            PostId = postId;
        }

        public void Update(string text, string author)
        {
            Text = text;
            Author = author;
        }
    }
}
