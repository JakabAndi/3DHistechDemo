using Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace _3DHistechDemo
{
    internal class Table : ITable
    {
        private Coordinate position = new Coordinate(0,0,0);
        private TableSize tableSize = new TableSize();
        public Table(double width, double height) 
        {
            tableSize.Width = width;
            tableSize.Height = height;
        }

        public Coordinate GetPosition() => position;

        public TableSize GetTableSize() => tableSize;

        public void MoveTo(Coordinate pos)
        {
            position.X = pos.X;
            position.Y = pos.Y;
            position.Z = pos.Z;
        }
    }
}