using System;

namespace Tsumo.Engine
{
    public static class Utils_regularExpressions
    {
        public static Func<string, string, int, Tsonic.CSharp.Js.JSArray<string>> findRegularExpressionMatches
        {
            get;
            private set;
        } = default(Func<string, string, int, Tsonic.CSharp.Js.JSArray<string>>)!;
        public static Func<string, string, int, Tsonic.CSharp.Js.JSArray<Tsonic.CSharp.Js.JSArray<string>>> findRegularExpressionSubmatches
        {
            get;
            private set;
        } = default(Func<string, string, int, Tsonic.CSharp.Js.JSArray<Tsonic.CSharp.Js.JSArray<string>>>)!;
        public static Func<string, string, string, int, string> replaceRegularExpression
        {
            get;
            private set;
        } = default(Func<string, string, string, int, string>)!;
        private static readonly System.Lazy<object?> __tsonic_module_initialization = new System.Lazy<object?>(() => __tsonic_module_init_core());
        private static object? __tsonic_module_init_core()
        {
            findRegularExpressionMatches = (string pattern, string input, int limit) =>
            {
                System.Text.RegularExpressions.Regex expression = new System.Text.RegularExpressions.Regex(pattern);
                System.Text.RegularExpressions.Match match = expression.Match(input);
                Tsonic.CSharp.Js.JSArray<string> result = new Tsonic.CSharp.Js.JSArray<string>(new string[] { });
                while (match.Success && (limit < 0 || result.length < limit))
                {
                    result.push(match.Value);
                    match = match.NextMatch();
                }
                return result;
            };
            findRegularExpressionSubmatches = (string pattern, string input, int limit) =>
            {
                System.Text.RegularExpressions.Regex expression = new System.Text.RegularExpressions.Regex(pattern);
                int[] groupNumbers = expression.GetGroupNumbers();
                System.Text.RegularExpressions.Match match = expression.Match(input);
                Tsonic.CSharp.Js.JSArray<Tsonic.CSharp.Js.JSArray<string>> result = new Tsonic.CSharp.Js.JSArray<Tsonic.CSharp.Js.JSArray<string>>(new Tsonic.CSharp.Js.JSArray<string>[] { });
                while (match.Success && (limit < 0 || result.length < limit))
                {
                    Tsonic.CSharp.Js.JSArray<string> row = new Tsonic.CSharp.Js.JSArray<string>(new string[] { match.Value });
                    for (int groupIndex = 1; groupIndex < groupNumbers.Length; groupIndex++)
                    {
                        row.push(match.Result("${" + groupNumbers[groupIndex] + "}"));
                    }
                    result.push(row);
                    match = match.NextMatch();
                }
                return result;
            };
            replaceRegularExpression = (string pattern, string replacement, string input, int limit) =>
            {
                System.Text.RegularExpressions.Regex expression = new System.Text.RegularExpressions.Regex(pattern);
                if (limit < 0)
                {
                    return expression.Replace(input, replacement);
                }
                return expression.Replace(input, replacement, limit);
            };
            return null;
        }
        public static void __tsonic_module_init()
        {
            _ = __tsonic_module_initialization.Value;
        }
    }
}
