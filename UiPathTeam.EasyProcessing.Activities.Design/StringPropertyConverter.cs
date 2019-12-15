using System;
using System.Activities;
using System.Activities.Expressions;
using System.Activities.Presentation.Model;
using System.Globalization;
using System.Windows.Data;
using Microsoft.VisualBasic.Activities;

namespace UiPathTeam.EasyProcessing.Activities.Design
{
    public class StringPropertyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ModelItem modelItem = value as ModelItem;
            if (modelItem != null)
            {
                InArgument<string> inArgument = modelItem.GetCurrentValue() as InArgument<string>;
                if (inArgument != null)
                {
                    Activity<string> expression = inArgument.Expression;
                    VisualBasicValue<string> vbexpression = expression as VisualBasicValue<string>;
                    Literal<string> literal = expression as Literal<string>;
                    if (literal != null)
                    {
                        return "\"" + literal.Value + "\"";
                    }
                    else if (vbexpression != null)
                    {
                        return vbexpression.ExpressionText;
                    }
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            VisualBasicValue<string> vbArgument = new VisualBasicValue<string>((string)value);
            InArgument<string> inArgument = new InArgument<string>(vbArgument);
            return inArgument;
        }
    }
}
