namespace Exostasis.Polynomial
{
    public class Variable
    {
        public int _exponent { get; private set; }
        public string _variable { get; private set; }

        public Variable (string variable)
        {
            _exponent = 1;
            _variable = variable;
        }

        public Variable (string variable, int exponent)
        {
            _exponent = exponent;
            _variable = variable;
        }
    }
}
