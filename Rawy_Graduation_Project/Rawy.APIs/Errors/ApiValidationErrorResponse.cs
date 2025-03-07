namespace Rawy.APIs.Dtos
{
	public class ApiValidationErrorResponse : ApiResponse
	{
        public IEnumerable<string> Errors { get; set; } 
        public ApiValidationErrorResponse():base(400)
        {
            
        }
    }
}
