using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3DHistechDemo
{
    internal class MainWindowViewModel
    {
        private List<IEngine> engineList;
        private ITable myTable;

        internal MainWindowViewModel(List<IEngine> engines, ITable table)
        {
            engineList = engines;
            myTable = table;
        }

        
    }
}
