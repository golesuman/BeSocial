using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace BlogApp.ViewModel
{
    public class PostViewModel
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public IFormFile PostImage { get; set; }
        public DateTime PublishedAt { get; set; }
        public string UserId { get; set; }
        public int CategoryId { get; set; }
        
    }
}