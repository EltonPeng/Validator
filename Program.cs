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
            AFeatureToggle aFeatureToggle = new AFeatureToggle();
            aFeatureToggle.CurrencyValidator = new CurrencyValidator();
            aFeatureToggle.DaysLimitValidator = new DaysLimitValidator();

            Console.WriteLine("------------done------------");
            Console.Read();
        }
    }

    public class AFeatureToggle
    {
        public IValidateCurrency CurrencyValidator { get; set; }

        public IValidateDaysLimit DaysLimitValidator { get; set; }

        public bool ValidateAdditionalConditions()
        {
            CurrencyValidator.Initialize(new List<string> { "USD", "CNY" });
            DaysLimitValidator.Initialize(2);

            return (CurrencyValidator as IValidator).Validate() && (DaysLimitValidator as IValidator).Validate();
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

    public interface IValidateDaysLimit
    {
        void Initialize(decimal daysCount);
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

    public class DaysLimitValidator : IValidator, IValidateDaysLimit
    {
        private decimal daysLimit;
        public void Initialize(decimal daysCount)
        {
            daysLimit = daysCount;
        }

        public bool Validate()
        {
            // 假逻辑
            return daysLimit > 1;
        }
    }

}
