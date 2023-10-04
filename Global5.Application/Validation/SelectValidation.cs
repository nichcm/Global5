using Global5.Application.ViewModels.Requests;
using FluentValidation;

namespace Global5.Application.Validation
{
    public class SelectValidation : AbstractValidator<BaseRequest>
    {
        public SelectValidation()
        {

            #region General

            RuleFor(x => x.Id).NotEmpty().WithMessage("Id é obrigatório");

            #endregion

        }
    }
}