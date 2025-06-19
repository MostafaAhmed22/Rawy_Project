namespace Rawy.APIs.Dtos.ModerationDtos
{
	public class ModerationApiResponse
	{
		public bool Approved { get; set; }
		public string Response { get; set; }
		public string? Reason { get; set; }
		public string Timestamp { get; set; }
		public string? Error { get; set; }
	}
}
