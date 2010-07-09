#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions.IronRuby
{
    using System;

    internal class IronRubyScriptBlock
    {
        [ThreadStatic]
        private static bool ignoreNextNewLine;

        private IronRubyScriptBlock(string block)
        {
            bool ignoreNewLine = ignoreNextNewLine;

            if (string.IsNullOrEmpty(block))
            {
                Contents = string.Empty;
                return;
            }

            int endOffset = 4;

            if (block.EndsWith("-%>", StringComparison.OrdinalIgnoreCase))
            {
                endOffset = 5;
                ignoreNextNewLine = true;
            }
            else
            {
                ignoreNextNewLine = false;
            }

            if (block.StartsWith("<%=", StringComparison.OrdinalIgnoreCase))
            {
                int outputLength = block.Length - endOffset - 1;

                if (outputLength < 1)
                {
                    throw new InvalidOperationException("Started a '<%=' block without ending it.");
                }

                string output = block.Substring(3, outputLength).Trim();

                Contents = "response.write(" + output + ")";

                return;
            }

            if (block.StartsWith("<%", StringComparison.OrdinalIgnoreCase))
            {
                Contents = block.Substring(2, block.Length - endOffset).Trim();
                return;
            }

            if (ignoreNewLine)
            {
                block = block.Trim();
            }

            block = block.Replace(@"\", @"\\");
            block = block.Replace(Environment.NewLine, "\\r\\n");
            block = block.Replace(@"""", @"\""");

            if (block.Length > 0)
            {
                Contents = "response.write(\"" + block + "\")";
            }
        }

        public string Contents
        {
            get;
            private set;
        }

        public static IronRubyScriptBlock Parse(string block)
        {
            return new IronRubyScriptBlock(block);
        }
    }
}