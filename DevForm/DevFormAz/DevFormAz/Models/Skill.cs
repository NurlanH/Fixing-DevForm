﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DevFormAz.Models
{
    public class Skill
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? UserDetailId { get; set; }
        public virtual UserDetail UserDetail { get; set; }
    }
}