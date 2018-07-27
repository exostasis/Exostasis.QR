using System;
using Exostasis.QR.Polynomial.Extensions;

namespace Exostasis.QR.Polynomial
{
    public class AlphaTerm : Object
    {
        public int _exponent { get; }
        public Variable _variable { get; }

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

        public AlphaTerm(AlphaTerm a1)
        {
            _exponent = a1._exponent;
            _variable = new Variable(a1._variable);
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
            return new AlphaTerm((a1._exponent + a2._exponent) % 255, a1._variable * a2._variable);
        }

        public static AlphaTerm operator+ (AlphaTerm a1, AlphaTerm a2)
        {
            var temp = (a1.AntiLog() ^ a2.AntiLog());

            if (temp != 0)
            {
                return temp.Log(a1._variable._variable, a1._variable._exponent);
            }

            return null;
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

            var a1 = obj as AlphaTerm;
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

        public void DisplayAlphaTerm()
        {
            Console.Write("a^(" + _exponent + ")");
            _variable.DisplayVariable();
        }
    }
}
