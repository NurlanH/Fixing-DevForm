using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DevFormAz.Models
{
    public class TagFormList
    {
        public int Id { get; set; }
        public int TagListsId { get; set; }
        public int FormsId { get; set; }
        public virtual ICollection<TagList> TagLists { get; set; }
        public virtual ICollection<Form> Forms { get; set; }
    }
}