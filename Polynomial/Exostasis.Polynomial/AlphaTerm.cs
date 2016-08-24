using Exostasis.Polynomial.Extensions;

namespace Exostasis.Polynomial
{
    public class AlphaTerm
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

        public static bool operator== (AlphaTerm a1, AlphaTerm a2)
        {
            return a1._variable == a2._variable;
        }

        public static bool operator!= (AlphaTerm a1, AlphaTerm a2)
        {
            return !(a1 == a2);
        }
    }
}
