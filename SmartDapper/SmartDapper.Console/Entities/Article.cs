using SmartDapper.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartDapper.Console.Entities
{
    public class Article
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ArticleId { get; set; }
        public string Slug { get; set; }
        public string Headline { get; set; }
        public string Teaser { get; set; }
        public string ImageUrl { get; set; }
        public string ThumbnailUrl { get; set; }
        public DateTime DateTimeCreated { get; set; }
        public DateTime? DateTimePublished { get; set; }
        public DateTime? DateTimeModified { get; set; }
        public string Author { get; set; }
    }
}
