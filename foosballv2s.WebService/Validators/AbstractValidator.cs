using System.Collections.Generic;

namespace foosballv2s.WebService.Validators
{
    public abstract class AbstractValidator : IValidator
    {
        protected delegate bool ValidatePart();
        private IList<ValidatePart> validationFuncs = new List<ValidatePart>();
        
        public bool Validate()
        {
            foreach (ValidatePart validationFunc in validationFuncs)
            {
                if (!validationFunc())
                {
                    return false;
                }
            }
            return true;
        }

        protected void RegisterValidationFunc(ValidatePart validationFunc)
        {
            validationFuncs.Add(validationFunc);
        }
    }
}