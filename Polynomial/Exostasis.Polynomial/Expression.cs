using Exostasis.Polynomial.Extensions;
using System;
using System.Collections.Generic;

namespace Exostasis.Polynomial
{
    public class Expression
    {
        public List<AlphaTerm> _terms { get; private set; }

        public Expression ()
        {
            _terms = new List<AlphaTerm>();
        }

        public Expression (Expression e1) : this()
        {
            for (int i = 0; i < e1._terms.Count; ++i)
            {
                _terms.Add(new AlphaTerm(e1._terms[i]));
            }
        }

        public void AddTerm(AlphaTerm term)
        {
            if (term != null)
            {
                _terms.Add(term);
                if (_terms.Count > 1)
                {
                    Reduce();
                }
            }
        }

        public List<ConstantTerm> GetConstantExpression ()
        {
            List<ConstantTerm> expression = new List<ConstantTerm>();
            foreach (AlphaTerm a in _terms)
            {
                expression.Add(new ConstantTerm(a.AntiLog()));
            }

            return expression;
        }

        public void Reduce()
        {
            for (int i = 0; i < _terms.Count; ++i)
            {
                for (int j = i + 1; j < _terms.Count; ++j)
                {
                    if (_terms[i].EqualVariables(_terms[j]))
                    {
                        _terms[i] =  _terms[i] + _terms[j];
                        _terms.RemoveAt(j--);
                        if (_terms[i] == null)
                        {
                            _terms.RemoveAt(i--);
                        }
                    }
                }
            }
        }

        public static Expression operator* (Expression e1, AlphaTerm a1)
        {
            Expression result = new Expression();
            foreach (AlphaTerm a2 in e1._terms)
            {              
                result.AddTerm(a1 * a2);
            }

            return result;
        }

        public static Expression operator* (Expression e1, Expression e2)
        {
            Expression result = new Expression();
            foreach (AlphaTerm a1 in e1._terms)
            {
                foreach (AlphaTerm a2 in e2._terms)
                {
                    result.AddTerm(a1 * a2);
                }
            }

            return result;
        }

        public static Expression operator/ (Expression dividen, Expression divisor)
        {
            Expression results = new Expression(dividen);
            for (int i = 0; i < dividen._terms.Count; ++i)
            {
                AlphaTerm multiplier = new AlphaTerm(results._terms[0]._exponent, results._terms[0]._variable._variable, results._terms[0]._variable._exponent - divisor._terms[0]._variable._exponent);
                results = (divisor * multiplier) ^ results;
            }

            return results;
        }

        public static Expression operator^ (Expression e1, Expression e2)
        {
            Expression results = new Expression();

            int lowestCount = e1._terms.Count > e2._terms.Count ? e2._terms.Count : e1._terms.Count;

            for (int i = 0; i < lowestCount; ++i)
            {
                results.AddTerm(e1._terms[i] + e2._terms[i]);
            }

            if (e1._terms.Count == lowestCount)
            {
                for (int i = lowestCount; i < e2._terms.Count; ++ i)
                {
                    results.AddTerm(e2._terms[i]);
                }
            }
            else
            {
                for (int i = lowestCount; i < e1._terms.Count; ++i)
                {
                    results.AddTerm(e1._terms[i]);
                }
            }

            return results;
        }

        public void DisplayExpression()
        {
            foreach(AlphaTerm a in _terms)
            {
                a.DisplayAlphaTerm();
                if (a != _terms[_terms.Count - 1])
                {
                    Console.Write(" + ");
                }
            }
        }
    }
}
