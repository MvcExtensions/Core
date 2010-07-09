#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.IronRuby
{
    using System.Text;
    using System.Text.RegularExpressions;

    internal static class IronRubyViewMethodWrapper
    {
        private static readonly Regex scriptExpression = new Regex("<%.*?%>", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.Singleline);

        public static string Wrap(string methodName, string content)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine("def " + methodName);

            MatchCollection matches = scriptExpression.Matches(content);

            int currentIndex = 0;

            foreach (Match match in matches)
            {
                int blockBeginIndex = match.Index;

                IronRubyScriptBlock block = IronRubyScriptBlock.Parse(content.Substring(currentIndex, blockBeginIndex - currentIndex));

                if (!string.IsNullOrEmpty(block.Contents))
                {
                    builder.AppendLine(block.Contents);
                }

                block = IronRubyScriptBlock.Parse(match.Value);
                builder.AppendLine(block.Contents);
                currentIndex = match.Index + match.Length;
            }

            if (currentIndex < content.Length - 1)
            {
                IronRubyScriptBlock endBlock = IronRubyScriptBlock.Parse(content.Substring(currentIndex));
                builder.Append(endBlock.Contents);
            }

            builder.AppendLine();
            builder.AppendLine("end");

            return builder.ToString();
        }
    }
}