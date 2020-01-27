using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gymit.Models
{
    public class TestTag
    {
        [ForeignKey(nameof(TagName))]
        public virtual Tag Tag { get; set; }

        public string TagName { get; set; }

        public Test Test { get; set; }

        public Guid TestId { get; set; }
    }
}
