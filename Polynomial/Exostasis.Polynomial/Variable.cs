namespace Exostasis.Polynomial
{
    public class Variable
    {
        public int _exponent { get; private set; }
        public string _variable { get; private set; }

        public Variable (string variable)
        {
            _exponent = 1;
            _variable = variable.ToLower();
        }

        public Variable (string variable, int exponent)
        {
            _exponent = exponent;
            _variable = variable;
        }

        public static Variable operator* (Variable v1, Variable v2)
        {
            return new Variable(v1._variable, (v1._exponent + v2._exponent) ^ 256);
        }

        public static bool operator== (Variable v1, Variable v2)
        {
            return v1._exponent == v2._exponent && v1._variable.ToLower() == v2._variable.ToLower();
        }

        public static bool operator!= (Variable v1, Variable v2)
        {
            return !(v1 == v2);
        }
    }
}
