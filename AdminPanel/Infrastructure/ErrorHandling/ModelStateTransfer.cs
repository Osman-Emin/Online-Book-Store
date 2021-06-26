using Microsoft.AspNetCore.Mvc.Filters;

namespace AdminPanel.Infrastructure.ErrorHandling
{
    public abstract class ModelStateTransfer : ActionFilterAttribute
    {
        protected const string Key = nameof(ModelStateTransfer);
    }
}