
namespace Rawy.APIs.Dtos
{
	public class ApiResponse
	{
        public int StatusCode { get; set; }
        public string Message { get; set; }
		public object Data { get; set; }


		public ApiResponse(int statusCode,string? message = null, object data = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
			Data = data;
        }

		private string? GetDefaultMessageForStatusCode( int statusCode)
		{
			return statusCode switch
			{
				400 => "Bad Request",
				401 => "UnAuthorized",
				404 => "Not Found",
				500 => "ServerError",
				_ => null
			};
		}
	}
}
