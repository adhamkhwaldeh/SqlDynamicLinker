using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;

namespace ProperMan.Infrastructure.ModelBinder
{
    public class BasePagingFilterBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Metadata.ModelType == typeof(BaseQueryableFilterModelBinder))
            {
                return new BinderTypeModelBinder(typeof(BaseQueryableFilterModelBinder));
            }

            return null;
        }
    }
}
