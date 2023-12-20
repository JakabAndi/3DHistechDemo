using Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3DHistechDemo
{
    public interface ITable
    {
        Coordinate GetPosition();
        void MoveTo(Coordinate pos);
        TableSize GetTableSize();
    }
}