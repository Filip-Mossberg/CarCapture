using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ModelFiltrationResult
    {
        public List<Rectangle> BoxList { get; set; }
        public List<string> LabelList { get; set; }
        public List<string> ScoreList { get; set; }
    }
}
