using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.IService
{
    public interface IImageService
    {
        public Task DrawBoundingBoxes(string imagePath, List<BoxCoordinates> boxes);
    }
}
