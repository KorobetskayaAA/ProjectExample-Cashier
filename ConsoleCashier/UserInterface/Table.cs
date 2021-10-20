using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleCashier.UserInterface
{
    class Table
    {
        readonly IEnumerable<TableColumn> Columns;

        public Table(IEnumerable<TableColumn> columns)
        {
            Columns = columns;
        }

        int rowsScroll = 0;

        public void Print()
        {

        }
    }
}
