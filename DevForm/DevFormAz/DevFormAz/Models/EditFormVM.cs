﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DevFormAz.Models
{
    public class EditFormVM
    {
        public Form Form { get; set; }
        public ICollection<TagList> TagLists { get; set; }
    }
}