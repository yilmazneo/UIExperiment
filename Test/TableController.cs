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
        Y
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
        SetUnselected
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
            if(action == TableAction.Create)
            {
                string newId = repository.AddTable((TableShape)arguments[UpdateKey.Shape]);
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

            }
            else if (action == TableAction.ScaleX)
            {

            }
            else if (action == TableAction.ScaleY)
            {

            }
            else if (action == TableAction.Rotate)
            {

            }
            else if (action == TableAction.UpdateName)
            {

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
