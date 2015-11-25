using SmartDapper.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartDapper.Console.Entities
{
    public class Page
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PageId { get; set; }
        public int ArticleId { get; set; }
        public int PageNumber { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Body { get; set; }
        public string ImageUrl { get; set; }
    }
}
