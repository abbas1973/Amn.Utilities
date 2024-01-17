using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Amn.Utilities
{
    public static class ReflectionExtension
    {

        #region آیا یک کلاس مشخص، پروپرتی با نام مشخص دارد؟
        /// <summary>
        /// آیا یک کلاس مشخص، پروپرتی با نام مشخص دارد؟
        /// typeof(MyClass).HasProperty("propname");
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static bool HasProperty(this Type obj, string propertyName)
        {
            return obj.GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance) != null;
        }
        #endregion



        #region گرفتن اتریبیوت دسکریپشن از FieldInfo
        /// <summary>
        /// گرفتن اتریبیوت دسکریپشن از FieldInfo
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public static string GetDescription(this FieldInfo field)
        {
            if (field != null)
            {
                DescriptionAttribute attr =
                       Attribute.GetCustomAttribute(field,
                         typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attr != null)
                {
                    return attr.Description;
                }
            }
            return null;
        }
        #endregion



        #region گرفتن اطلاعات فیلدهای از نوع const در یک کلاس
        public static List<FieldInfo> GetConstants(
            this Type type,
            params string[] propNames)
        {
            #region گرفتن فیلد ها
            var fieldInfos = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(fi => fi.IsLiteral && !fi.IsInitOnly);

            //IEnumerable<FieldInfo> fieldInfos = type.GetFields(flags);
            //fieldInfos = fieldInfos.Where(fi => fi.IsLiteral && !fi.IsInitOnly);
            if (propNames != null && propNames.Any())
                fieldInfos = fieldInfos.Where(x => propNames.Contains(x.Name));
            #endregion

            return fieldInfos.ToList();
        }
        #endregion



        #region گرفتن مقدار فیلدهای از نوع const در یک کلاس
        public static List<string> GetConstantValues(
            this Type type,
            params string[] propNames)
        {
            var consts = type.GetConstants(propNames);

            return consts.Select(x => x.GetValue(null).ToString()).ToList();
        }
        #endregion



        #region گرفتن مقدار و توضیحات const های پابلیک یک کلاس
        public static List<KeyValuePair<string, string>> GetConstantsWithDescription
            (this Type type,
            params string[] propNames)
        {
            var model = new List<KeyValuePair<string, string>>();
            var consts = type.GetConstants(propNames);
            foreach (var item in consts)
            {
                var val = item.GetValue(null).ToString();
                model.Add(new KeyValuePair<string, string>(val, item.GetDescription()));
            }
            return model;
        }
        #endregion




        #region اضافه کردن پراپرتی به آبجک داینامیک سی شارپ
        /// <summary>
        /// اضافه کردن پراپرتی به آبجک داینامیک سی شارپ
        /// </summary>
        /// <param name="expando">آبجکت</param>
        /// <param name="propertyName">نام پراپرتی</param>
        /// <param name="propertyValue">مقدار مورد نظر</param>
        public static void AddProperty(this ExpandoObject expando, string propertyName, object propertyValue)
        {
            // ExpandoObject supports IDictionary so we can extend it like this
            var expandoDict = expando as IDictionary<string, object>;
            if (expandoDict.ContainsKey(propertyName))
                expandoDict[propertyName] = propertyValue;
            else
                expandoDict.Add(propertyName, propertyValue);
        } 
        #endregion

    }
}
