using System.ComponentModel.DataAnnotations;

namespace EmpowerIdMicroservice.WebApi.Model
{
    public class PostModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }
        
        [Required]
        public string Author { get; set; }
        
        public bool IsPublished { get; set; }
    }

    public class CreatePost : PostModel
    { 
    }

    public class UpdatePost : PostModel
    {
        public int PostId { get; set; }
    }
}
