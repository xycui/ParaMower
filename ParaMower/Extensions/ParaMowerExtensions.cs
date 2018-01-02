using System.Linq;
using System.Reflection;
using ParaMower.Supports;

namespace ParaMower.Extensions
{
    public delegate string ParamLoadingDel(string paramAlias, string paramDesc);

    public static class ParaMowerExtensions
    {
        public static void LoadExternal(this object loadDest, ParamLoadingDel loadingDel)
        {
            var props = loadDest.GetType()
                .GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                .Where(prop => prop.GetCustomAttributes().OfType<CommandParamAttribute>().Any());

            foreach (var prop in props)
            {
                var attr = prop.GetCustomAttributes().OfType<CommandParamAttribute>().First();
                var propType = prop.PropertyType;
                if (propType != typeof(string)) continue;
                var data = loadingDel(attr.ParamAlias, attr.Description);
                if (string.IsNullOrEmpty(data)) continue;
                prop.SetValue(loadDest, data, null);
            }
        }
    }
}
