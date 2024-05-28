using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class CarDetectorResult
    {
        public string Image { get; set; }
        public List<CarColorResult> ColorList { get; set; }
    }
}
