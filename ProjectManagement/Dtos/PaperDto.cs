using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectManagement.Dtos
{
    public class PaperDto
    {
        public Publication Publication { get; set; }
        public Journal Journal { get; set; }
        public Conference Conference { get; set; }
        public List<int> Members { get; set; }
        public List<int> Grants { get; set; }
        public string Creator { get; set; }

    }
}