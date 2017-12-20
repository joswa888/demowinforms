using System.Linq;
using System.Text;

namespace DemoHelpers
{
    public static class StringHelpers
    {
        public static string ObjectToString(object obj)
        {
            StringBuilder result = new StringBuilder();

            obj = obj ?? Enumerable.Empty<object>();

            var fields = obj.GetType().GetProperties();

            foreach (var field in fields)
            {
                result.Append(string.Format(field.Name + ": {0}\n", field.GetValue(obj)));
            }

            return result.ToString();
        }
    }
}
