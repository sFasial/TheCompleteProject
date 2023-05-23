using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using TheCompleteProject.Api.Infrastructure.Middelware;
using TheCompleteProject.Utility;

namespace TheCompleteProject.Api.Infrastructure.Filter
{
    public class ActionFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
        }


        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception != null)
                return;
            if (context.Result.GetType() == typeof(FileContentResult))
                return;

            var result = context.Result;
            var response = new ResponseModel
            {
                Message = string.Empty,
                StatusCode = 200,
                Errors = null
            };

            switch (result)
            {

                case OkObjectResult okObject:
                    var okType = okObject.Value.GetType();
                    if (okType.Name == "String")
                        response.Message = ContentLoader.
                            ReturnLanguageData(okObject.Value.ToString());

                    if (okType.Name != "String")
                        response.Data = okObject.Value;
                    break;

                case BadRequestObjectResult badRequestObjectResult:
                    var BadType = badRequestObjectResult.Value.GetType();
                    if (BadType.Name == "String")
                        response.Message = ContentLoader.ReturnLanguageData(badRequestObjectResult.Value.ToString());

                    if (BadType.Name != "String")
                        response.Data = badRequestObjectResult.Value;
                    break;

                case ObjectResult objectResult:
                    var data = (Dictionary<string, object>)(objectResult.Value);

                    switch (data.Keys.Select(x => x).LastOrDefault())
                    {
                        case "Data":
                            response.StatusCode = (int)HttpStatusCode.OK;
                            response.Message = Convert.ToString(data[Constants.ResponseMessageField]);
                            response.Data = data[Constants.ResponseDataField];
                            break;
                        case "Error":
                            response.StatusCode = (int)HttpStatusCode.PreconditionFailed;
                            response.Message = Convert.ToString(data[Constants.ResponseMessageField]);
                            response.Data = data[Constants.ResponseErrorField];
                            break;
                        case "Unauthorized":
                            response.StatusCode = (int)HttpStatusCode.Unauthorized;
                            response.Message = Convert.ToString(data[Constants.ResponseMessageField]);
                            response.Data = data[Constants.ResponseErrorField];
                            break;
                        case "Failure":
                            response.StatusCode = (int)HttpStatusCode.BadRequest;
                            response.Message = Convert.ToString(data[Constants.ResponseMessageField]);
                            response.Data = data[Constants.ResponseErrorField];
                            break;

                        default:
                            response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            response.Message = ContentLoader.ReturnLanguageData(key: "EMP109");
                            response.Data = null;
                            break;
                    }
                    break;

                case JsonResult json:
                    response.Data = json.Value;
                    break;

                case OkResult _:

                case EmptyResult _:
                    response.Data = null;
                    break;

                case UnauthorizedResult _:
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    response.Message = ContentLoader.ReturnLanguageData(key: "EMP102");
                    response.Data = null;
                    break;

                default:
                    response.Data = result;
                    break;
            }

            context.Result = new JsonResult(response);
        }
    }


}
