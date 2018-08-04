using System;
using System.Collections.Generic;

namespace Validator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var factory = new ValidatorFactory();
            factory.CurrencyValidator = new CurrencyValidator();

            var validators = factory.GetValidators();

            foreach (var item in validators)
            {
                Console.WriteLine(item.Validate());
                
            }

            Console.WriteLine("------------done------------");
            Console.Read();
        }
    }

    public interface IValidator
    {
        bool Validate();
    }

    public interface IValidateCurrency
    {
        void Initialize(List<string> currencyName);
    }

    public class CurrencyValidator : IValidator, IValidateCurrency
    {
        private List<string> currencies;
        public void Initialize(List<string> currencyNames)
        {
            currencies = currencyNames;
        }

        public bool Validate()
        {
            return currencies.Count > 1;
        }
    }

    public class ValidatorFactory
    {
        public IValidateCurrency CurrencyValidator { get; set; }

        public IEnumerable<IValidator> GetValidators()
        {
            CurrencyValidator.Initialize(new List<string> {"CNY", "USD"});

            return new List<IValidator> { CurrencyValidator as IValidator };
        }

    }
}
