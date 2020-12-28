using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RazorPagesMovie.Models
{
    public class Movie
    {
        public int ID { get; set; }

        [DisplayName("标题")]
        public string Title { get; set; }

        [DisplayName("发布日期")]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }

        [DisplayName("题材")]
        public string Genre { get; set; }

        [DisplayName("票价")]
        public decimal Price { get; set; }
    }
}
