using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cats.Rest.Models
{
    public class Program
    {
        public int ProgramID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string LongName { get; set; }
        public string ShortCode { get; set; }

        public Program(int programId, string name, string description, string longName, string shortCode)
        {
            ProgramID = programId;
            Name = name;
            Description = description;
            LongName = longName;
            ShortCode = shortCode;
        }
    }
}