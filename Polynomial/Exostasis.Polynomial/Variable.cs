using System;

namespace Exostasis.Polynomial
{
    public class Variable : Object
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
            if (v1._variable.ToLower() != v2._variable.ToLower())
            {
                throw new Exception("Cannot times to variables that don't have the same base");
            }
            return new Variable(v1._variable, (v1._exponent + v2._exponent) % 256);
        }

        public override bool Equals (Object obj)
        {
            if (obj == null)
            {
                return false;
            }

            Variable v1 = obj as Variable;
            if (v1 == null)
            {
                return false;
            }

            return _exponent == v1._exponent && _variable.ToLower() == v1._variable.ToLower();
        }

        public void DisplayVariable()
        {
            Console.Write(_variable + " ^(" + _exponent + ")");
        }
    }
}
