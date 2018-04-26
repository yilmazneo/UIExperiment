using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Test
{
    public enum TableLayoutUpdateMode
    {
        New,
        Delete,
        Coordinates,
        Rotate,
        SetSelected,
        SetUnselected,
        ScaleX,
        ScaleY,
        UpdateName
    }

    public class Table
    {
        public string Id { get; set; }
        public TableShape Shape { get; set; }
        public string Text { get; set; }
        public int RotateAngle { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public int ScaleX { get; set; }
        public int ScaleY { get; set; }
        public SolidColorBrush Color { get; set; }
    }

    public class TableRepository
    {
        private Dictionary<string, Table> tables = new Dictionary<string, Table>();
        int id;
        int angle = 45;
        int scaleRate = 10;
        int width = 40;
        int height = 40;

        public TableRepository()
        {
            PopulateTablesFromDB();
            id = 0;
        }

        public void PopulateTablesFromDB()
        {
            tables.Clear();
        }

        public Dictionary<string, Table> GetTables()
        {
            return tables;
        }

        public Table GetModel(string Id)
        {
            return tables[Id];
        }

        public void UpdateTableCoordinates(string id,double x,double y)
        {
            Table t = tables[id];
            t.X = x;
            t.Y = y;
        }

        public void UpdateTableScaleX(string id)
        {
            Table t = tables[id];
            t.ScaleX += scaleRate;
        }

        public void UpdateTableScaleY(string id)
        {
            Table t = tables[id];
            t.ScaleY += scaleRate;
        }

        public void UpdateTableRotateAngle(string id)
        {
            Table t = tables[id];
            t.RotateAngle += angle;
        }

        public void UpdateTableName(string id,string name)
        {
            Table t = tables[id];
            t.Text = name;
        }

        public string AddTable(TableShape shape)
        {
            Table newTable = new Table()
            {
                Id = "S" + id,
                Text = "Table " + id,
                Color = Brushes.LightGreen,
                ScaleX = 0,
                ScaleY = 0,
                RotateAngle = 0,
                Width = width,
                Height = height,
                X = 50,
                Y = 50,
                Shape = shape
            };

            id++;

            tables.Add(newTable.Id, newTable);

            return newTable.Id;
        }

        public void DeleteTable(string id)
        {
            tables.Remove(id);
        }

        public void Save()
        {

        }

        public void RevertChanges()
        {
            PopulateTablesFromDB();
        }

    }
}
