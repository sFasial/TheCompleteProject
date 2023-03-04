
namespace TheCompleteProject.Utility.Response
{
    public interface IGenericResponse
    {
    }

    public class GenericResponse : IGenericResponse
    {
        public string Message { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public object Model { get; set; }

        public void Ok<TModel>(TModel model) where TModel : class
        {
            Model = model;
            Success = true;
            Message = "Success";
        }
        public void Ok()
        {
            Success = true;
            Message = "Success";
        }
        public void WriteCustomErrormessage(string error)
        {
            Success = false;
            Message = error;
        }
    }
}
