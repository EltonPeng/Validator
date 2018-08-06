using System;
using System.Collections.Generic;

namespace Validator
{
    class Program
    {
        // 消费者
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            // 假设DI解决
            var factory = new ValidatorFactory();
            // 假设DI解决
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

    public interface IValidatorFactory
    {
        IEnumerable<IValidator> GetValidators();
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
            // 假逻辑
            return currencies.Count > 1;
        }
    }

    // Factory应该有接
    public class ValidatorFactory : IValidatorFactory
    {
        public IValidateCurrency CurrencyValidator { get; set; }

        public IEnumerable<IValidator> GetValidators()
        {
            // 也可以单独提取成为一个接口方法，由消费者调用
            CurrencyValidator.Initialize(new List<string> {"CNY", "USD"});

            return new List<IValidator> { CurrencyValidator as IValidator };
        }

    }
}
