using InGo.Models.Links;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InGo.Models
{
    public class Faq
    {
        public int Id { get; set; }
        [Required]
        public string Question { get; set; }
        [Required]
        public string Answer { get; set; }
        public bool IsDeleted { get; set; }
        public virtual FaqCategory FaqCategory { get; set; }
        public int FaqCategoryId { get; set; }
        public DateTime PublishDate { get; set; }
    }
}