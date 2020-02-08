﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DevFormAz.Models
{
    public class Form
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public DateTime FormTime { get; set; }
        public bool isDeleted { get; set; } = false;
        public DateTime WhenIsDeleted { get; set; }
        public int UserId { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<TagList> TagLists { get; set; }
        public virtual ICollection<FormLike> FormLikes { get; set; }
        public virtual ICollection<FormViewCount> FormViewCounts { get; set; }
        public virtual ICollection<FormImage> FormImages { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}