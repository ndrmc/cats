using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cats.Rest.Models
{
    public class ProjectCode
    {
        public int ProjectCodeID { get; set; }
        public string Value { get; set; }

        public ProjectCode(int projectCodeId, string value)
        {
            ProjectCodeID = projectCodeId;
            Value = value;
        }
    }
}