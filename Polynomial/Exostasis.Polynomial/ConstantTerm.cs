namespace Exostasis.Polynomial
{ 
    public class ConstantTerm
    {
        public int _constant { get; private set; }
        public Variable _variable { get; private set; }

        public ConstantTerm (int constant)
        {
            _constant = constant;
        }

        public ConstantTerm (int constant, string variable, int exponent) : this(constant)
        {
            _variable = new Variable(variable, exponent);
        }

        public ConstantTerm (int constant, Variable variable) : this(constant)
        {
            _variable = variable;
        }
    }
}
