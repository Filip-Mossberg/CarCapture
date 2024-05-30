using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.IService
{
    public interface ICarDetectorUIService
    {
        public Task<CarDetectorResult> CarDetector(byte[] imageBytes);
    }
}
