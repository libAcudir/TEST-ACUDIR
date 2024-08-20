using System.Reflection;

namespace Acudir.Test.Apis.Extra.Extensions
{
    public static class ObjectExtensions
    {
        public static void UpdatePropertiesFrom<T>(this T target, T source)
        {
            IEnumerable<PropertyInfo> properties = typeof(T).GetProperties().Where(p => p.CanWrite);
            foreach (PropertyInfo property in properties)
            {
                object? value = property.GetValue(source);
                property.SetValue(target, value);
            }
        }
    }
}
