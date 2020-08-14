using DataParser.Infrastructure.Interfaces;
using DataParser.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LocationTracker.Client.Infrastructure.Visitor
{
    public class TransposedPacketDataVisitor : IVisitor
    {
        private DataRow _currentRow;
        public TransposedPacketDataVisitor()
        {
            DataTable = new DataTable();
        }

        public DataTable DataTable { get; }

        public void Visit(BaseData componentData)
        {
            switch (componentData.Name)
            {
                case "Timestamp":
                    CreateColumn(componentData.Name);

                    _currentRow = DataTable.NewRow();
                    DataTable.Rows.Add(_currentRow);

                    _currentRow[componentData.Name] = componentData.Value;

                    break;
                case "Priority":
                case "Longitude":
                case "Latitude":
                case "Angle":
                case "Speed":
                case "Altitude":
                case "Satellites":
                    CreateColumn(componentData.Name);
                    _currentRow[componentData.Name] = componentData.Value;
                    break;
                default:
                    break;
            }
        }
        private void CreateColumn(string columnName)
        {
            DataColumnCollection columns = DataTable.Columns;
            if (columns.Contains(columnName) == false)
            {
                var column = new DataColumn
                {
                    ColumnName = columnName,
                    Caption = columnName
                };

                DataTable.Columns.Add(column);
            }
        }
    }
}
