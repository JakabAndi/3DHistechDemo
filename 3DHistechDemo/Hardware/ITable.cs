using Global;

namespace _3DHistechDemo
{
    public interface ITable
    {
        Coordinate GetPosition();
        void MoveTo(Coordinate pos);
        TableSize GetTableSize();
    }
}