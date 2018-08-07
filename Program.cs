using System;
using System.Collections.Generic;

using Moq;

namespace Validator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            // Production Code, DI will resolve object creation.
            AFeatureToggle aFeatureToggle = new AFeatureToggle();
            aFeatureToggle.CurrencyValidator = new CurrencyValidator();
            aFeatureToggle.DaysLimitValidator = new DaysLimitValidator();

            Console.WriteLine(aFeatureToggle.ValidateAdditionalConditions());

            Console.WriteLine("------------done------------");

            Mock<IValidateCurrency> mockValidateCurrency = new Mock<IValidateCurrency>();
            Mock<IValidateDaysLimit> mockValidateDaysLimit = new Mock<IValidateDaysLimit>();

            mockValidateCurrency.As<IValidator>().Setup(x => x.Validate()).Returns(true);
            mockValidateDaysLimit.As<IValidator>().Setup(x => x.Validate()).Returns(false);

            AFeatureToggle testObject = new AFeatureToggle();
            testObject.CurrencyValidator = mockValidateCurrency.Object;
            testObject.DaysLimitValidator = mockValidateDaysLimit.Object;

            var result = testObject.ValidateAdditionalConditions();

            Console.WriteLine("UT:" + result.ToString());

            Console.Read();
        }
    }

    public class AFeatureToggle
    {
        public IValidateCurrency CurrencyValidator { get; set; }

        public IValidateDaysLimit DaysLimitValidator { get; set; }

        public bool ValidateAdditionalConditions()
        {
            var AFeatureCurrencies = new List<string> { "USD", "CNY" };
            CurrencyValidator.Initialize(AFeatureCurrencies);
            decimal AFeatureDaysLimit = 2m;
            DaysLimitValidator.Initialize(AFeatureDaysLimit);
            
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

    public interface IFeatureToggleValidatorManager
    {
        List<IValidator> GetValidators();
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
            // fake
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
            // fake
            return daysLimit > 1;
        }
    }

    public class AFeatureToggleValidatorsManager : IFeatureToggleValidatorManager
    {
        private List<IValidator> validators;

        public IValidateCurrency CurrencyValidator { get; set; }

        public IValidateDaysLimit DaysLimitValidator { get; set; }

        public List<IValidator> GetValidators()
        {
            validators = new List<IValidator>();

            var AFeatureCurrencies = new List<string> { "USD", "CNY" };
            CurrencyValidator.Initialize(AFeatureCurrencies);
            decimal AFeatureDaysLimit = 2m;
            DaysLimitValidator.Initialize(AFeatureDaysLimit);

            validators.Add(CurrencyValidator as IValidator);
            validators.Add(DaysLimitValidator as IValidator);

            return validators;
        }
    }

}
