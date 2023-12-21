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
        private Coordinate position;
        private TableSize tableSize = new TableSize();
        public Table(double width, double height) 
        {
            tableSize.Width = width;
            tableSize.Height = height;

            position = new Coordinate(tableSize.Width / 2, tableSize.Height / 2, 0);
        }

        public Coordinate GetPosition() => position;

        public TableSize GetTableSize() => tableSize;

        public void MoveTo(Coordinate pos)
        {
            if (pos.X >= tableSize.Width / 2)
            {
                position.X = pos.X;
            }
            if (pos.Y >= tableSize.Height / 2)
            {
                position.Y = pos.Y;
            }
            if (pos.Z >= 0)
            {
                position.Z = pos.Z;
            }
        }
    }
}