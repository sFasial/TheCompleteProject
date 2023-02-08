using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheCompleteProject.Utility;

namespace TheCompleteProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {

        protected int UserId => HttpContext.User.Identity.IsAuthenticated ? Convert.ToInt32(HttpContext.User.Claims.First(x => x.Type == "Id").Value) : 0 ;
        protected string UserName => HttpContext.User.Identity.IsAuthenticated ? HttpContext.User.Claims.First(x => x.Type == "UserName").Value : "";

        protected Dictionary<string, object> ApiResponse(string msgCode, object result, string languageCode = "")
        {
            var response = new Dictionary<string, object>();
            response.Add(Constants.ResponseMessageField, ContentLoader.ReturnLanguageData(msgCode, languageCode));
            response.Add(Constants.ResponseDataField, result);
            return response;
        }
        protected Dictionary<string, object> UnauthorizedResponse(string msgCode, string languageCode = "")
        {
            var response = new Dictionary<string, object>();
            response.Add(Constants.ResponseMessageField, ContentLoader.ReturnLanguageData(msgCode, languageCode));
            response.Add(Constants.UnAuthorizedResponseField,"");
            return response;
        }

        protected Dictionary<string, object> FailureResponse(string msgCode, object result, string languageCode = "")
        {
            var response = new Dictionary<string, object>();
            response.Add(Constants.ResponseMessageField, ContentLoader.ReturnLanguageData(msgCode, languageCode));
            response.Add(Constants.ResponseFailureField, result);
            return response;
        }

        protected Dictionary<string, object> FailureResponse(string msgCode, ModelStateDictionary.ValueEnumerable /*ModelErrorCollection*/ result , string languageCode = "")
        {
            var response = new Dictionary<string, object>();
            var data = new List<string>();
            var errorCollection = result.Select(x => x.Errors);

            foreach (var error in errorCollection)
            {
                data.AddRange(error.Select(x => x.ErrorMessage));
            }
            response.Add(Constants.ResponseMessageField, ContentLoader.ReturnLanguageData(msgCode, languageCode));
            response.Add(Constants.ResponseErrorField, data);
            return response;
        }

    }
}
