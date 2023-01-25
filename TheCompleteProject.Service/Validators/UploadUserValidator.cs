using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheCompleteProject.ModelsAndDto_s.DemoModels;
using TheCompleteProject.Utility.BulkImport;

namespace TheCompleteProject.Service.Validators
{
    public class UploadUserValidator : AbstractValidator<UploadUserErrorDto>
    {
        private char[] illegalCharacters = @"!@#$%^&*(){}[]_+<>?/\-=~`;:,".ToCharArray();
        public UploadUserValidator(IEnumerable<UploadUserErrorDto> value)
        {
            RuleFor(x => x.Password).NotNull().NotEmpty().WithMessage(LanguageContentLoader.ReturnLanguageData("EMP302"));

            RuleFor(x => x)
            .Must(x => !string.IsNullOrEmpty(x.Password) ? !IsContainsSpecialCharacters(x.Password) 
            : true)
            .WithMessage(LanguageContentLoader.ReturnLanguageData("EMP303"));
        }

        private bool IsContainsSpecialCharacters(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                foreach (var item in value.ToCharArray())
                {
                    if (!(Array.IndexOf(illegalCharacters,item) == -1))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
