using Microsoft.AspNetCore.Mvc;

namespace Backend_Final.Application.Common.Extensions
{
    public static class ResultExtensions
    {
        public static IActionResult ToActionResult<T>(this Result<T> result)
        {
            if (result.Success)
                return new OkObjectResult(result.Data);

            return result.Type switch
            {
                ResultType.NotFound =>
                    new NotFoundObjectResult(result.ErrorMessage),

                ResultType.BadRequest =>
                    new BadRequestObjectResult(result.ErrorMessage),

                _ =>
                    new ObjectResult(result.ErrorMessage)
                    {
                        StatusCode = 500
                    }
            };
        }
    }
}
