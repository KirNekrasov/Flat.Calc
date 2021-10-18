using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FLAT.LexicalAnalysis;
using FLAT.SyntaxAnalisys;
using FLAT.SyntaxAnalisys.FormalGrammar;

namespace Calc.Models
{
    internal class Calculator
    {
        private IDictionary<String, Func<double, double, double>> binaryOperations;

        private IContextFreeGrammar grammar;

        private IParser parser;

        private IScanner scanner;


        public Calculator()
        {
            this.Init();
        }


        public double Calculate(String input)
        {
            var tree = this.parser.Parse(this.scanner.Scan(input));
            var stack = new Stack<double>();
            this.Traverse(tree.Root, stack);

            return stack.Pop(); ;
        }

        private void Init()
        {
            var axiom = new Nonterminal("axiom");
            var expression = new Nonterminal("expression");
            var term = new Nonterminal("term");
            var factor = new Nonterminal("factor");

            var prods = new List<IContextFreeProduction>()
            {
                new ContextFreeProduction(axiom, expression),

                new ContextFreeProduction(expression, term, new Terminal("+"), expression),
                new ContextFreeProduction(expression, term, new Terminal("-"), expression),
                new ContextFreeProduction(expression, term),

                new ContextFreeProduction(term, factor, new Terminal("*"), term),
                new ContextFreeProduction(term, factor, new Terminal("/"), term),
                new ContextFreeProduction(term, factor),

                new ContextFreeProduction(factor, new Terminal("("), expression, new Terminal(")")),
                new ContextFreeProduction(factor, new Terminal("double"))

            };

            this.grammar = new ContextFreeGrammar(axiom, prods);

            this.parser = new LRParser(grammar);

            this.scanner = new MathScanner();

            this.binaryOperations = new Dictionary<String, Func<double, double, double>>();
            this.binaryOperations[prods[1].ToString()] = (first, second) => first + second;
            this.binaryOperations[prods[2].ToString()] = (first, second) => first - second;
            this.binaryOperations[prods[4].ToString()] = (first, second) => first * second;
            this.binaryOperations[prods[5].ToString()] = (first, second) => first / second;
        }

        private void Traverse(ParseTreeNode node, Stack<double> stack)
        {
            if (node.Name == "double")
            {
                stack.Push(Double.Parse(node.Attribute));
            }
            else
            {
                foreach (var descendant in node.Descendants)
                {
                    this.Traverse(descendant, stack);
                }

                if (this.binaryOperations.Keys.Contains(node.Name))
                {
                    var second = stack.Pop();
                    var first = stack.Pop();
                    stack.Push(this.binaryOperations[node.Name](first, second));
                }
            }
        }
    }
}
