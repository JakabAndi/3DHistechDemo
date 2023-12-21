using Global;

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
