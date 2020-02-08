﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DevFormAz.Models
{
    public class UserDetail
    {
        public int Id { get; set; }
        public string Specialty { get; set; }
        public string Country { get; set; }
        public string GithubLink { get; set; }
        public string LinkedinLink { get; set; }
        public string  Biography { get; set; }
        public string Image { get; set; }
        public string Role { get; set; }
        public virtual List<Skill> Skills { get; set; }
    }
}