using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace api.Dtos.Comment
{
    public class UpdateCommentRequestDto
    {
        [Required]
        [MinLength(6, ErrorMessage ="Title must be at least 6  characters!")]
        [MaxLength(280, ErrorMessage ="Title cant be over 280  characters!")]
        public string Title { get; set; }= string.Empty;

        [Required]
        [MinLength(6, ErrorMessage ="Content must be at least 6 characters!")]
        [MaxLength(280, ErrorMessage ="Content cant be over 280  characters!")]
        public string Content { get; set; }= string.Empty;
       
    }
}