using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;

namespace CrudLearn.Attributes
{
    public class ValidationFilterAttribute : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var res = new Dictionary<string, ModelErrorCollection>();
                foreach (var m in context.ModelState)
                {
                    if (m.Value.Errors.Any())
                    {
                        res.Add(m.Key, m.Value.Errors);
                    }
                }

                var result = new UnprocessableEntityObjectResult(res);
                result.StatusCode = 400;
                context.Result = result;
            }
        }
        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}
