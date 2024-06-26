using System.Collections.Generic;
using System.Linq;
using Reqnroll.Bindings;
using Reqnroll.Bindings.CucumberExpressions;
using Reqnroll.Bindings.Discovery;
using Reqnroll.Configuration;

namespace Reqnroll.RuntimeTests
{
    public class BindingSourceProcessorStub : BindingSourceProcessor, IRuntimeBindingSourceProcessor
    {
        public readonly List<IStepDefinitionBinding> StepDefinitionBindings = new();
        public readonly List<IHookBinding> HookBindings = new();
        public readonly List<IStepArgumentTransformationBinding> StepArgumentTransformationBindings = new();
        public readonly List<string> GeneralErrorMessages = new();
        public readonly List<string> BindingSpecificErrorMessages = new();

        public IEnumerable<string> ValidationErrors => GeneralErrorMessages.Concat(BindingSpecificErrorMessages);

        public BindingSourceProcessorStub() : base(new BindingFactory(new StepDefinitionRegexCalculator(ConfigurationLoader.GetDefault()), new CucumberExpressionStepDefinitionBindingBuilderFactory(new CucumberExpressionParameterTypeRegistry(new BindingRegistry())), new CucumberExpressionDetector()))
        {
        }

        protected override void ProcessStepDefinitionBinding(IStepDefinitionBinding stepDefinitionBinding)
        {
            StepDefinitionBindings.Add(stepDefinitionBinding);
        }

        protected override void ProcessHookBinding(IHookBinding hookBinding)
        {
            HookBindings.Add(hookBinding);
        }

        protected override void ProcessStepArgumentTransformationBinding(IStepArgumentTransformationBinding stepArgumentTransformationBinding)
        {
            StepArgumentTransformationBindings.Add(stepArgumentTransformationBinding);
        }

        protected override void OnValidationError(BindingValidationResult validationResult, bool genericBindingError)
        {
            if (genericBindingError)
                GeneralErrorMessages.AddRange(validationResult.ErrorMessages);
            else
                BindingSpecificErrorMessages.AddRange(validationResult.ErrorMessages);
        }

        public void RegisterTypeLoadError(string errorMessage)
        {
            GeneralErrorMessages.Add(errorMessage);
        }
    }
}