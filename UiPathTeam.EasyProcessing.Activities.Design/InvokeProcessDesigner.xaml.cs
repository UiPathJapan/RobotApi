using System.Activities;
using System.Activities.Expressions;
using System.Activities.Presentation.Model;
using System.Windows;
using Microsoft.VisualBasic.Activities;

namespace UiPathTeam.EasyProcessing.Activities.Design
{
    public partial class InvokeProcessDesigner
    {
        public InvokeProcessDesigner()
        {
            InitializeComponent();
        }

        private void Browse(object sender, RoutedEventArgs e)
        {
            var dlg = new ProcessNameDialogBox();
            dlg.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            dlg.SelectedProcessName = CurrentProcessName;
            if (dlg.ShowDialog() == true)
            {
                var selected = dlg.SelectedProcessName;
                if (!string.IsNullOrEmpty(selected))
                {
                    CurrentProcessName = dlg.SelectedProcessName;
                }
            }
        }

        private string CurrentProcessName
        {
            get
            {
                var inArgument = ModelItem.Properties["ProcessName"].Value?.GetCurrentValue() as InArgument<string>;
                if (inArgument != null)
                {
                    Activity<string> expression = inArgument.Expression;
                    VisualBasicValue<string> vbexpression = expression as VisualBasicValue<string>;
                    Literal<string> literal = expression as Literal<string>;
                    if (literal != null)
                    {
                        return literal.Value;
                    }
                    else if (vbexpression != null)
                    {
                        if (vbexpression.ExpressionText.StartsWith("\"") && vbexpression.ExpressionText.EndsWith("\""))
                        {
                            return vbexpression.ExpressionText.Substring(1, vbexpression.ExpressionText.Length - 2);
                        }
                    }
                }
                return null;
            }
            set
            {
                VisualBasicValue<string> vbArgument = new VisualBasicValue<string>("\"" + value + "\"");
                InArgument<string> inArgument = new InArgument<string>(vbArgument);
                ModelItem.Properties["ProcessName"].SetValue(inArgument);
            }
        }
    }
}
