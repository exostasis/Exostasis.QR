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
            Reduce();
        }

        public List<ConstantTerm> GetConstantExpression ()
        {
            List<ConstantTerm> expression = new List<ConstantTerm>();

            return expression;
        }

        private void Reduce()
        {
            List<AlphaTerm> newList = new List<AlphaTerm>();

            for (int i = 0; i < _terms.Count - 2; ++i)
            {
                for (int j = i + 1; j < _terms.Count; ++j)
                {
                    if (_terms[i].Equals(_terms[j]))
                    {
                        newList.Add(_terms[i] + _terms[j]);
                    }
                }
            }
        }

        public static Expression operator/ (Expression dividen, Expression divisor)
        {
            throw new NotImplementedException();
        }
    }
}
