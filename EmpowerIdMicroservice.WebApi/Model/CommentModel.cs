using System.ComponentModel.DataAnnotations;

namespace EmpowerIdMicroservice.WebApi.Model
{
    public class CommentModel
    {
        [Required]
        public string Text { get; set; }
        
        [Required]
        public string Author { get; set; }
    }

    public class CreateComment : CommentModel
    {
        [Required]
        public int PostId { get; set; }
    }

    public class UpdateComment : CommentModel
    {
        [Required]
        public int CommentId { get; set; }
    }
}
