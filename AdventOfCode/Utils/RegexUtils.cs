using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Reflection;

namespace AdventOfCode.Utils
{
    public static partial class RegexUtils
    {
        // regex mapper
        // takes a regex with named capture groups, and maps it to properties on a class
        public static T MapTo<T>(this MatchCollection matches) where T : new()
        {
            var instance = new T();

            var groups = matches.First().Groups;

            foreach (Group group in groups)
            {
                instance = group.MapTo(instance);
            }

            return instance;
        }

        public static T MapTo<T>(this Group match, T instance)
        {
            // check if the Match Name is just a numbered match, not a named capture group:
            if (int.TryParse(match.Name, out _))
                return instance;

            // try and find a matching Property
            var property = typeof(T).GetProperty(
                match.Name,
                BindingFlags.Instance
                | BindingFlags.Public
                | BindingFlags.NonPublic
                | BindingFlags.IgnoreCase);

            if (property != null)
            {
                var propType = property.PropertyType;

                if (propType == typeof(string))
                {
                    property.SetValue(instance, match.Value);
                }
                else if (propType == typeof(int))
                {
                    var val = int.Parse(match.Value);
                    property.SetValue(instance, val);
                }
                else if (propType == typeof(double))
                {
                    var val = double.Parse(match.Value);
                    property.SetValue(instance, val);
                }
                else if (propType.IsAssignableFrom(typeof(List<>)))
                {
                    // work out what to do here :(
                    throw new NotImplementedException();
                }
                else if (propType.IsAssignableTo(typeof(Enum)))
                {
                    Enum.Parse(propType, match.Value, true);
                }
            }

            return instance;
        }

        public static List<int> GetInts(this string s)
        {
            return intsRegex().Matches(s).Select(x => int.Parse(x.Value)).ToList();
        }

        public static List<long> GetLongs(this string s)
        {
            return intsRegex().Matches(s).Select(x => long.Parse(x.Value)).ToList();
        }

        public static List<int> GetSignedInts(this string s)
        {
            return signedIntsRegex().Matches(s).Select(x => int.Parse(x.Value)).ToList();
        }

        [GeneratedRegex(@"\d+")]
        private static partial Regex intsRegex();

        [GeneratedRegex(@"\+?\-?\d+")]
        private static partial Regex signedIntsRegex();
    }
}
