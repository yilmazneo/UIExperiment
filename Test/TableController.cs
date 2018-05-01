using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public enum UpdateKey
    {
        Shape,
        X,
        Y,
        Name,
        ScaleX,
        ScaleY,
        Color,
        Angle,
        Width,
        Height
    }

    public enum TableAction
    {
        ScaleX,
        ScaleY,
        Rotate,
        UpdateName,
        UpdateCoordinates,
        Create,
        Delete,
        Save,
        RevertChanges,
        SetSelected,
        SetUnselected,
        UpdateAll
    }
    class TableController
    {
        TableRepository repository;
        ITableLayoutView view;

        public TableController(TableRepository repository,ITableLayoutView view)
        {
            this.view = view;
            this.repository = repository; 
        }

        public Table GetModel(string Id)
        {
            return repository.GetModel(Id);
        }

        public void UpdateModel(string id,TableAction action,Dictionary<UpdateKey,object> arguments)
        {
            if(action == TableAction.UpdateAll)
            {               
                double x = (double)arguments[UpdateKey.X];
                double y = (double)arguments[UpdateKey.Y];
                repository.UpdateTableCoordinates(id, x, y);
                int scaleX = (int)arguments[UpdateKey.ScaleX];
                int scaleY = (int)arguments[UpdateKey.ScaleY];
                repository.UpdateTableScaleX(id, scaleX);
                repository.UpdateTableScaleY(id, scaleY);
                int rotateAngle = (int)arguments[UpdateKey.Angle];
                repository.UpdateTableRotateAngle(id,rotateAngle);
                string name = (string)arguments[UpdateKey.Name];
                repository.UpdateTableName(id, name);
                string color = (string)arguments[UpdateKey.Color];
                repository.UpdateTableColor(id, color);
                TableShape shape = (TableShape)arguments[UpdateKey.Shape];
                repository.UpdateTableShape(id, shape);
                Table model = repository.GetModel(id);
                view.Update(model,TableLayoutUpdateMode.All);
            }
            if(action == TableAction.Create)
            {
                string newId = repository.AddTable(arguments);
                Table model = repository.GetModel(newId);
                view.Update(model,TableLayoutUpdateMode.New);
            }
            else if (action == TableAction.UpdateCoordinates)
            {
                double x = (double)arguments[UpdateKey.X];
                double y = (double)arguments[UpdateKey.Y];
                repository.UpdateTableCoordinates(id, x, y);
                Table model = repository.GetModel(id);
                view.Update(model, TableLayoutUpdateMode.Coordinates);
            }
            else if (action == TableAction.SetSelected)
            {
                Table model = repository.GetModel(id);
                view.Update(model, TableLayoutUpdateMode.SetSelected);
            }
            else if (action == TableAction.SetUnselected)
            {
                Table model = repository.GetModel(id);
                view.Update(model, TableLayoutUpdateMode.SetUnselected);
            }
            else if (action == TableAction.Delete)
            {
                Table model = repository.GetModel(id);
                repository.DeleteTable(id);
                view.Update(model, TableLayoutUpdateMode.Delete);
            }
            else if (action == TableAction.Rotate)
            {
                repository.UpdateTableRotateAngle(id,0);
                Table model = repository.GetModel(id);
                view.Update(model, TableLayoutUpdateMode.Rotate);
            }
            else if (action == TableAction.UpdateName)
            {
                string name = (string)arguments[UpdateKey.Name];
                repository.UpdateTableName(id, name);
                Table model = repository.GetModel(id);
                view.Update(model, TableLayoutUpdateMode.UpdateName);
            }
            else if (action == TableAction.Save)
            {
                repository.Save();
            }
            else if (action == TableAction.RevertChanges)
            {
                repository.RevertChanges();
            }
        }

    }
}
