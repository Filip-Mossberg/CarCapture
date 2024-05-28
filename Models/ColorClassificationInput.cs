using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ColorClassificationInput
    {
        public string Car { get; set; }
        public byte[] Image { get; set; }
    }
}
