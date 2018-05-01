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
        All,
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
        public string Color { get; set; }
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

        public void UpdateTableScaleX(string id,int value)
        {
            Table t = tables[id];
            t.ScaleX = value;
        }

        public void UpdateTableScaleY(string id, int value)
        {
            Table t = tables[id];
            t.ScaleY = value;
        }

        public void UpdateTableRotateAngle(string id,int value)
        {
            Table t = tables[id];
            t.RotateAngle = value;
        }

        public void UpdateTableName(string id,string name)
        {
            Table t = tables[id];
            t.Text = name;
        }

        public void UpdateTableColor(string id, string color)
        {
            Table t = tables[id];
            t.Color = color;
        }

        public void UpdateTableShape(string id, TableShape shape)
        {
            Table t = tables[id];
            t.Shape = shape;
        }

        public string AddTable(Dictionary<UpdateKey, object> args)
        {
            Table newTable = new Table()
            {
                Id = "S" + id,
                Text = args[UpdateKey.Name].ToString(),
                Color = args[UpdateKey.Color].ToString(),
                ScaleX = int.Parse(args[UpdateKey.ScaleX].ToString()),
                ScaleY = int.Parse(args[UpdateKey.ScaleY].ToString()),
                RotateAngle = int.Parse(args[UpdateKey.Angle].ToString()),
                Width = double.Parse(args[UpdateKey.Width].ToString()),
                Height = double.Parse(args[UpdateKey.Height].ToString()),
                X = double.Parse(args[UpdateKey.X].ToString()),
                Y = double.Parse(args[UpdateKey.Y].ToString()),
                Shape = args[UpdateKey.Shape].ToString() == "Circle" ? TableShape.Circle : TableShape.Rectangle
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
