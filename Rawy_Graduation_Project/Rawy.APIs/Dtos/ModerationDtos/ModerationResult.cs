namespace Rawy.APIs.Dtos.ModerationDtos
{
	public class ModerationResult
	{
		public bool IsApproved { get; set; }
		public string Response { get; set; }
		public string? Reason { get; set; }
		public string Timestamp { get; set; }
	}
}
