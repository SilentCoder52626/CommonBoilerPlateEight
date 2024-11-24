using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonBoilerPlateEight.Domain.Models
{
    public class DropdownDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class TextValueDropdownDto
    {
        public string Text { get; set; }
        public string Value { get; set; }
    }
}
