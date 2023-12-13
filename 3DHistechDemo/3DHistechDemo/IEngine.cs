using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3DHistechDemo
{
    public interface IEngine
    {
        double GetPostion();
        bool MoveTo(double coordinate);
        bool GoHome();
        double GetSpeed();
        void SetSpeed(double speed);
    }
}
