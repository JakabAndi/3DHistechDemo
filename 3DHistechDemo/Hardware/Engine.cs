using Global;

namespace _3DHistechDemo
{
    public class Engine : IEngine
    {
        private double position = 0;
        private double speed = 0;
        private AxisEnum axis;

        public Engine(AxisEnum axisEnum)
        {
            axis = axisEnum;
        }
        public double Speed
        {
            get { return speed; }
            private set { speed = value; }
        }

        public double Position
        { 
            get { return position; }
            private set { position = value; }
        }

        public AxisEnum Axis => axis;

        public double GetEngineStep() => Position;

        public double GetSpeed() => Speed;

        public bool MakeStep(bool direction)
        {
            var temp = direction ? Position + 10 : Position - 10;
            if (temp >= 10 && temp <= 90)
            {
                Position = temp;
            }
            return true;
        }

        public bool MoveTo(double engineStep, double scale)
        {
            if (engineStep != 0)
            {
                var percent = engineStep / scale;

                Position = percent * 100;
            }
            return true;
        }

        public void SetSpeed(double speed)
        {
            Speed = speed;
        }
    }
}
