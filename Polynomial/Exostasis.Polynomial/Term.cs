namespace Exostasis.Polynomial
{
    public class Term
    {
        public int _constant { get; private set; }
        public Variable _variable { get; private set; }

        public Term (int constant)
        {
            _constant = constant;
        }
    }
}
