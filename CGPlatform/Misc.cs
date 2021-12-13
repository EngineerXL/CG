using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace CGPlatform
{
    /// <summary>
    /// Реализация ExpandableObjectConverter для нессылочных типов <para/>
    /// Предназначенно в основном для контрола PropertyGrid
    /// </summary>
    /// <typeparam name="T">Тип структуры</typeparam>
    public class ValueTypeTypeConverter<T> : ExpandableObjectConverter where T : struct
    {
        public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
        {
            if (propertyValues == null)
                throw new ArgumentNullException("propertyValues");

            T ret = default(T);
            object boxed = ret;
            foreach (DictionaryEntry entry in propertyValues) {
                PropertyInfo pi = ret.GetType().GetProperty(entry.Key.ToString());
                if (pi != null && pi.CanWrite)
                    pi.SetValue(boxed, Convert.ChangeType(entry.Value, pi.PropertyType), null);
            }
            return (T)boxed;
        }
    }

    public static class HelpUtils
    {
        public static string ReadFromRes(this Assembly assembly, string resource_name)
        {
            var stream = (assembly ?? Assembly.GetEntryAssembly()).GetManifestResourceStream(resource_name);
            int length = (int) stream.Length;
            byte[] numArray = new byte[length];
            stream.Read(numArray, 0, length);
            stream.Close();
            int index = 0;
            if (length >= 3 && numArray[0] == (byte) 239 && numArray[1] == (byte) 187 && numArray[2] == (byte) 191)
                index = 3;
            return Encoding.UTF8.GetString(numArray, index, length - index);
        }
        
        public static string ReadFromRes(string resource_name) {
            return ReadFromRes(null, resource_name);
        }
    }
    
    
}