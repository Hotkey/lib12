﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using lib12.Collections;
using lib12.Exceptions;
using lib12.Extensions;

namespace lib12.Mathematics
{
    /// <summary>
    /// Mathematics formulas parser and calculator using reverse polish notation
    /// </summary>
    public class Formula
    {
        /// <summary>
        /// Gets the formula's text.
        /// </summary>
        public string Text { get; private set; }

        /// <summary>
        /// Gets the reverse polish notation tokens.
        /// </summary>
        public ReadOnlyCollection<Token> Tokens { get; private set; }

        /// <summary>
        /// Gets a value indicating whether formula is vali
        /// </summary>
        public bool IsValid { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Formula"/> class.
        /// </summary>
        /// <param name="text">The formula's text.</param>
        public Formula(string text)
        {
            Text = text;
            var tokens = Parse(Text);
            if (tokens.NotNull())
            {
                Tokens = new ReadOnlyCollection<Token>(tokens);
                IsValid = true;
            }
        }

        /// <summary>
        /// Parses formula
        /// </summary>
        /// <param name="text">The formula's text.</param>
        /// <returns></returns>
        private List<Token> Parse(string text)
        {
            var token = new StringBuilder();
            var stack = new Stack<OperatorToken>();
            var output = new List<Token>();
            var negationPossible = true;//true if next minus means negation

            for (int i = 0; i < text.Length; i++)
            {
                if (char.IsDigit(text[i])
                    || text[i].ToString(Thread.CurrentThread.CurrentCulture) == Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator)
                {
                    token.Clear();
                    token.Append(text[i]);
                    while (text.Length >= i + 2 && (char.IsDigit(text[i + 1])
                        || text[i + 1].ToString(Thread.CurrentThread.CurrentCulture) == Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator))
                    {
                        token.Append(text[i + 1]);
                        ++i;
                    }

                    output.Add(new NumberToken(double.Parse(token.ToString())));
                    negationPossible = false;
                }
                else if (text[i] == '+')
                {
                    AddOperatorToStack(stack, output, new OperatorToken(OperatorType.Plus));
                    negationPossible = true;
                }
                else if (text[i] == '-')
                {
                    if (negationPossible)
                    {
                        output.Add(new NegationToken());
                        negationPossible = false;
                    }
                    else
                    {
                        AddOperatorToStack(stack, output, new OperatorToken(OperatorType.Minus));
                    }
                }
                else if (text[i] == '*')
                {
                    AddOperatorToStack(stack, output, new OperatorToken(OperatorType.Mult));
                    negationPossible = true;
                }
                else if (text[i] == '/')
                {
                    AddOperatorToStack(stack, output, new OperatorToken(OperatorType.Div));
                    negationPossible = true;
                }
                else if (text[i] == '(')
                {
                    AddOperatorToStack(stack, output, new OperatorToken(OperatorType.LeftBraket));
                    negationPossible = true;
                }
                else if (text[i] == ')')
                {
                    AddOperatorToStack(stack, output, new OperatorToken(OperatorType.RightBraket));
                    negationPossible = false;
                }
                else if (char.IsLetter(text[i]))
                {
                    token.Clear();
                    token.Append(text[i]);
                    while (text.Length >= i + 2 && char.IsLetter(text[i + 1]))
                    {
                        token.Append(text[i + 1]);
                        ++i;
                    }

                    output.Add(new VariableToken(token.ToString()));
                    negationPossible = false;
                }
                else if (char.IsWhiteSpace(text[i]))
                    continue;
                else // unknown symbol in formula
                    return null;
            }

            //clear stack from operators
            while (stack.IsNotEmpty())
                output.Add(stack.Pop());

            //left and right brackets count must be equal
            var operators = output.Where(x => x.Type.Is(TokenType.Operator)).Cast<OperatorToken>().ToArray();
            if (operators.Count(x => x.Operator.Is(OperatorType.LeftBraket)) != operators.Count(x => x.Operator.Is(OperatorType.RightBraket)))
                return null;

            //in order to evaluate reverse polish notation it must be exactly one more literal than operator
            if (operators.Count(x => x.Operator.IsNot(OperatorType.LeftBraket, OperatorType.RightBraket)) != output.Count(x => x.Type.Is(TokenType.Number, TokenType.Variable)) - 1)
                return null;

            return output;
        }

        /// <summary>
        /// Adds the RPN operator to stack.
        /// </summary>
        /// <param name="stack">The stack.</param>
        /// <param name="output">The output.</param>
        /// <param name="op">The operator</param>
        private static void AddOperatorToStack(Stack<OperatorToken> stack, ICollection<Token> output, OperatorToken op)
        {
            if (op.Operator == OperatorType.RightBraket)//move from stack to output until left braket occurs
            {
                while (true)
                {
                    var token = stack.Pop();
                    if (token.Operator != OperatorType.LeftBraket)
                        output.Add(token);
                    else
                        break;
                }
            }
            else//move from stack output until less priority found or stack is empty
            {
                while (stack.Count > 0 && op.Operator != OperatorType.LeftBraket && stack.Peek().Priority >= op.Priority)
                {
                    var token = stack.Pop();
                    if (token.Operator != OperatorType.LeftBraket)
                        output.Add(token);
                    else
                        break;
                }
            }

            if (op.Operator != OperatorType.RightBraket)
                stack.Push(op);
        }

        /// <summary>
        /// Evaluates formula
        /// </summary>
        /// <returns></returns>
        /// <exception cref="MathException">Formula is not valid, cannot evaluate it</exception>
        /// <exception cref="UnknownEnumException{TokenType}"></exception>
        public double Evaluate()
        {
            if (!IsValid)
                throw new MathException("Formula is not valid, cannot evaluate it");

            var stack = new Stack<double>();
            bool negateNextStatement = false;

            foreach (var token in Tokens)
            {
                switch (token.Type)
                {
                    case TokenType.Number:
                        stack.Push(negateNextStatement ? -((NumberToken)token).Number : ((NumberToken)token).Number);
                        negateNextStatement = false;
                        break;
                    case TokenType.Operator:
                        stack.Push(Compute(stack.Pop(), stack.Pop(), ((OperatorToken)token).Operator));
                        break;
                    case TokenType.Negation:
                        negateNextStatement = true;
                        break;
                    case TokenType.Variable:
                        //if (Adapter == null)
                        //{
                        //    //throw new FormulaParserException("No adapter given");
                        //    stack.Push(1.0);
                        //}
                        //else
                        //{
                        //    stack.Push(Adapter.GetValueForVariable(((VariableToken)token).Variable));
                        //}
                        break;
                    default:
                        throw new UnknownEnumException<TokenType>(token.Type);
                }
            }

            return stack.Pop();
        }

        /// <summary>
        /// Computes single RPN operation
        /// </summary>
        /// <param name="a">The first parameter</param>
        /// <param name="b">The second parameter</param>
        /// <param name="operatorType">Type of the operator.</param>
        /// <returns></returns>
        /// <exception cref="UnknownEnumException{OperatorType}"></exception>
        private double Compute(double a, double b, OperatorType operatorType)
        {
            switch (operatorType)
            {
                case OperatorType.Plus:
                    return b + a;
                case OperatorType.Minus:
                    return b - a;
                case OperatorType.Mult:
                    return b * a;
                case OperatorType.Div:
                    return b / a;
                default:
                    throw new UnknownEnumException<OperatorType>();
            }
        }
    }
}
