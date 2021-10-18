using Calc.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLAT.LexicalAnalysis;

namespace Calc.Models
{
    internal class MathScanner : IScanner
    {
        public IEnumerable<Token> Scan(String input)
        {
            var tokens = new List<Token>();

            var buffer = new List<char>();

            for (var i = 0; i < input.Length; ++i)
            {
                if (input[i] == '+' || input[i] == '-' || input[i] == '*' ||
                    input[i] == '/' || input[i] == '(' || input[i] == ')')
                {
                    if (buffer.Any())
                    {
                        var temp = buffer.SkipWhile(с => input[i] == '0');
                        var attribute = temp.Any() ? new String(temp.ToArray()) : "0";
                        tokens.Add(new Token("double", attribute));
                        buffer.Clear();
                    }

                    tokens.Add(new Token(input[i].ToString()));
                }
                else if (Char.IsNumber(input[i]))
                {
                    buffer.Add(input[i]);
                    if (i == (input.Length - 1))
                    {
                        var temp = buffer.SkipWhile(с => input[i] == '0');
                        var attribute = temp.Any() ? new String(temp.ToArray()) : "0";
                        tokens.Add(new Token("double", attribute));
                    }
                }
                else if (input[i] == ' ')
                {

                }
                else
                {
                    throw new ArgumentException(Strings.ScannerException + input[i]);
                }
            }

            tokens.Add(new StringEndMarker());

            return tokens;
        }
    }
}
