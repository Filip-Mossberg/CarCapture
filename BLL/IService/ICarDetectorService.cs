using Microsoft.Extensions.ML;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.IService
{
    public interface ICarDetectorService
    {
        public Task<List<Rectangle>> CarDetectorModel(string imagePath);
    }
}
