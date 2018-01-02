using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ParaMower.Extensions;
using ParaMower.Supports;

namespace ParaMower
{
    /// <summary>
    /// Delegate to convert string array parameter to param dictionary
    /// </summary>
    /// <param name="args">incomming parameter</param>
    /// <param name="destInstanceType">instance type</param>
    /// <returns>The supported key value param dictionary</returns>
    public delegate IDictionary<string, string> ArgsConvertDel(IReadOnlyList<string> args, Type destInstanceType);

    /// <summary>
    /// Param Converter entry class
    /// </summary>
    public static class ParamConvertor
    {
        public static readonly ArgsConvertDel DashParamVpConvertDel = GetParamPairs;

        private static IDictionary<string, string> GetParamPairs(IReadOnlyList<string> args, Type destInstanceType)
        {
            if (args.Count <= 0)
            {
                throw new ArgumentException("Arguments is empty.");
            }
            if (args.Count % 2 != 0)
            {
                throw new ArgumentException("Argument length should be even.");
            }

            var dict = new Dictionary<string, string>();
            var aliasList = GetParamAliasList(destInstanceType);

            for (var i = 0; i < args.Count;)
            {
                if (aliasList.Contains(args[i].Trim('-')))
                {
                    dict[args[i].Trim('-')] = args[i + 1];
                    i += 2;
                }
                else
                {
                    throw new ArgumentException($"Argument parameter {args[i]} is invalid. ");
                }
            }

            return dict;
        }

        /// <summary>
        /// Get param alias list of the given type. 
        /// </summary>
        /// <param name="instanceType">config type which has the CommandParam attribute in its property declaration</param>
        /// <returns>The list of the available alias</returns>
        public static List<string> GetParamAliasList(Type instanceType)
        {
            var props = instanceType.GetProperties(BindingFlags.NonPublic | BindingFlags.Public |
                                                            BindingFlags.Instance);

            return (from prop in props
                    select prop.GetCustomAttributes().OfType<CommandParamAttribute>()
                into attrs
                    select attrs as CommandParamAttribute[] ?? attrs.ToArray()
                into commandParamAttributes
                    where commandParamAttributes.Any()
                    select commandParamAttributes.First().ParamAlias).ToList();
        }

        /// <summary>
        /// Load args with dash parameter pair to the object. The parameter array eg. "-o output.txt -f format"
        /// </summary>
        /// <typeparam name="T">Destination wrapped param type</typeparam>
        /// <param name="args">string arguments from the CLI</param>
        /// <returns>instance of the T</returns>
        public static T Load<T>(string[] args) where T : new()
        {
            return Load<T>(args, DashParamVpConvertDel);
        }

        /// <summary>
        /// Load args with given arguments and arguments convertion delegate
        /// </summary>
        /// <typeparam name="T">Destination wrapped param type</typeparam>
        /// <param name="args">string arguments from the CLI</param>
        /// <param name="paramConvertDel">Param convertion delegate</param>
        /// <returns>instance of the T</returns>
        public static T Load<T>(string[] args, ArgsConvertDel paramConvertDel) where T : new()
        {
            var ret = new T();

            var paramPair = paramConvertDel(args, typeof(T));
            var props = ret.GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                .Where(prop => prop.GetCustomAttributes().OfType<CommandParamAttribute>().Any());
            foreach (var prop in props)
            {
                var attr = prop.GetCustomAttributes().OfType<CommandParamAttribute>().First();
                var propType = prop.PropertyType;
                if (propType != typeof(string) || !paramPair.ContainsKey(attr.ParamAlias)) continue;
                prop.SetValue(ret, paramPair[attr.ParamAlias], null);
            }

            return ret;
        }

        public static T InteractiveLoad<T>(ParamLoadingDel loadingDel) where T : new()
        {
            var ret = new T();
            ret.LoadExternal(loadingDel);
            return ret;
        }
    }
}
