using Exostasis.Polynomial.Extensions;
using System;

namespace Exostasis.Polynomial
{
    public class AlphaTerm : Object
    {
        public int _exponent { get; private set; }
        public Variable _variable { get; private set; }

        public AlphaTerm (int exponent)
        {
            _exponent = exponent;
            _variable = null;
        }

        public AlphaTerm (int exponent, string variable, int variableExponent) : this(exponent)
        {
            _variable = new Variable(variable, variableExponent);
        }

        public AlphaTerm (int exponent, Variable variable) : this(exponent)
        {
            _variable = variable;
        }

        public ConstantTerm ToConstantTerm ()
        {
            return new ConstantTerm(this.AntiLog(), _variable);
        }

        public static AlphaTerm operator- (AlphaTerm a1, int value)
        {
            return new AlphaTerm(a1._exponent - value);
        }

        public static AlphaTerm operator* (AlphaTerm a1, AlphaTerm a2)
        {
            return new AlphaTerm((a1._exponent + a2._exponent) ^ 256);
        }

        public static AlphaTerm operator+ (AlphaTerm a1, AlphaTerm a2)
        {
            return (a1.AntiLog() ^ a2.AntiLog()).Log();
        }

        public static AlphaTerm operator- (AlphaTerm a1, AlphaTerm a2)
        {
            return a1 + a2;
        }

        public override bool Equals (Object obj)
        {
            if (obj == null)
            {
                return false;
            }

            AlphaTerm a1 = obj as AlphaTerm;
            if (a1 == null)
            {
                return false;
            }

            return _exponent == a1._exponent && _variable.Equals(a1._variable);
        }

        public bool EqualVariables (AlphaTerm a1)
        {
            if (a1 == null)
            {
                return false;
            }

            return _variable.Equals(a1._variable);
        }

        public bool EqualExponent(AlphaTerm a1)
        {
            if (a1 == null)
            {
                return false;
            }

            return _exponent == a1._exponent;
        }
    }
}
