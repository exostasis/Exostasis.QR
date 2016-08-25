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

        public void AddTerm(AlphaTerm term)
        {
            _terms.Add(term);
            if (_terms.Count > 1)
            {
                Reduce();
            }
        }

        public List<ConstantTerm> GetConstantExpression ()
        {
            List<ConstantTerm> expression = new List<ConstantTerm>();

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
                    }
                }
            }
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
            throw new NotImplementedException();
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
