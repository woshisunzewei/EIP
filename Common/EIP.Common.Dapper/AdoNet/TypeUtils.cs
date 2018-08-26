using System;

namespace EIP.Common.Dapper.AdoNet
{
    public class TypeUtils
    {
        public static object ConvertForType(object value, Type type)
        {
            switch (type.FullName)
            {
                case "System.String":
                    value = value.ToString();
                    break;
                case "System.Boolean":
                    value = bool.Parse(value.ToString());
                    break;
                case "System.Int16":
                case "System.Int32":
                case "System.Int64":
                    value = int.Parse(value.ToString());
                    break;
                case "System.Double":
                    value = double.Parse(value.ToString());
                    break;
                case "System.Decimal":
                    value = new decimal(double.Parse(value.ToString()));
                    break;
            }

            return value;
        }
    }
}