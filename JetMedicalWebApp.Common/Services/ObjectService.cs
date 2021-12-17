using System;
using System.Globalization;
using JetMedicalWebApp.Common;

namespace JetMedicalWebApp.Common.Services
{
    public class ObjectService
    {
        public static Object SetValue(Object obj, string propertyName, string value)
        {
            int intValue;
            double doubleValue;
            bool boolValue;
            decimal decimalValue;
            DateTime ngayValue;
            
            if (obj.GetType().GetProperty(propertyName).PropertyType == typeof(string))
            {
                obj.GetType().GetProperty(propertyName).SetValue(obj, value, null);
            }
            else if (obj.GetType().GetProperty(propertyName).PropertyType == typeof(int))
            {
                if (Int32.TryParse(value, out intValue))
                {
                    obj.GetType().GetProperty(propertyName).SetValue(obj, intValue, null);
                }
            }
            else if (obj.GetType().GetProperty(propertyName).PropertyType == typeof(Nullable<int>))
            {
                if (Int32.TryParse(value, out intValue))
                {
                    obj.GetType().GetProperty(propertyName).SetValue(obj, intValue, null);
                }
                else
                {
                    obj.GetType().GetProperty(propertyName).SetValue(obj, null, null);
                }
            }
            else if (obj.GetType().GetProperty(propertyName).PropertyType == typeof(double))
            {
                if (double.TryParse(value, out doubleValue))
                {
                    obj.GetType().GetProperty(propertyName).SetValue(obj, doubleValue, null);
                }
            }
            else if (obj.GetType().GetProperty(propertyName).PropertyType == typeof(Nullable<double>))
            {
                if (double.TryParse(value, out doubleValue))
                {
                    obj.GetType().GetProperty(propertyName).SetValue(obj, doubleValue, null);
                }
                else
                {
                    obj.GetType().GetProperty(propertyName).SetValue(obj, null, null);
                }
            }
            else if (obj.GetType().GetProperty(propertyName).PropertyType == typeof(bool))
            {
                if (bool.TryParse(value, out boolValue))
                {
                    obj.GetType().GetProperty(propertyName).SetValue(obj, boolValue, null);
                }
            }
            else if (obj.GetType().GetProperty(propertyName).PropertyType == typeof(Nullable<bool>))
            {
                if (bool.TryParse(value, out boolValue))
                {
                    obj.GetType().GetProperty(propertyName).SetValue(obj, boolValue, null);
                }
                else
                {
                    obj.GetType().GetProperty(propertyName).SetValue(obj, null, null);
                }
            }
            else if (obj.GetType().GetProperty(propertyName).PropertyType == typeof(DateTime))
            {
                if (DateTime.TryParseExact(value, Common.CommonConstants.DateFormat, null, DateTimeStyles.None, out ngayValue))
                {
                    obj.GetType().GetProperty(propertyName).SetValue(obj, ngayValue, null);
                }
            }
          
            else if (obj.GetType().GetProperty(propertyName).PropertyType == typeof(decimal))
            {
                if (decimal.TryParse(value, out decimalValue))
                {
                    obj.GetType().GetProperty(propertyName).SetValue(obj, decimalValue, null);
                }
            }
            else if (obj.GetType().GetProperty(propertyName).PropertyType == typeof(Nullable<decimal>))
            {
                if (decimal.TryParse(value, out decimalValue))
                {
                    obj.GetType().GetProperty(propertyName).SetValue(obj, decimalValue, null);
                }
                else
                {
                    obj.GetType().GetProperty(propertyName).SetValue(obj, null, null);
                }
            }
            if (obj.GetType().GetProperty(propertyName).PropertyType == typeof(Nullable<DateTime>))
            {
                if (DateTime.TryParseExact(value, Common.CommonConstants.DateFormat, null, DateTimeStyles.None, out ngayValue))
                {
                    obj.GetType().GetProperty(propertyName).SetValue(obj, ngayValue, null);
                }
                else
                {
                    obj.GetType().GetProperty(propertyName).SetValue(obj, null, null);
                }
            }

            return obj;
        }
    }
}
