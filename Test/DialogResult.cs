using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public enum DialogAnswer
    {
        OK,
        Cancel,
        Yes,
        No
    }

    static class DialogResultManager
    {
        static Dictionary<DialogAnswer, bool> results = new Dictionary<DialogAnswer, bool>();
        public static DialogAnswer? Answer = null;

        public static void SetResult(DialogAnswer newAnswer)
        {
            Answer = newAnswer;
        }

        public static void ShowDialog(string title, string message, DialogAnswer[] buttons)
        {
            DialogWindow window = new DialogWindow(title, message, buttons);
            window.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            window.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            window.ShowDialog();
        }
    }
}
