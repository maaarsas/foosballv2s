using System.Collections.Generic;

namespace foosballv2s.WebService.Validators
{
    /// <summary>
    /// Abstract entity validator
    /// </summary>
    public abstract class AbstractValidator : IValidator
    {
        protected delegate bool ValidatePart();
        private IList<ValidatePart> validationFuncs = new List<ValidatePart>();
        
        /// <summary>
        /// Runs through all registered validation functions and validates the entity
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Registers a validation function
        /// </summary>
        /// <param name="validationFunc"></param>
        protected void RegisterValidationFunc(ValidatePart validationFunc)
        {
            validationFuncs.Add(validationFunc);
        }
    }
}