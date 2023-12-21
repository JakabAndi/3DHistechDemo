using Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3DHistechDemo
{
    public interface IEngine
    {
        double GetEngineStep();
        AxisEnum Axis { get; }
        bool MoveTo(double engineStep, double scale);
        bool MakeStep(bool direction);
        double GetSpeed();
        void SetSpeed(double speed);
    }
}
