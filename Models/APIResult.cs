namespace TimeBasedPreventiveMeasures.Models
{
	public class APIResult<TPayload>
	{
        public TPayload? Payload { get; set; }
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
    }
}
