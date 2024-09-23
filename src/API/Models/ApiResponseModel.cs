namespace API.Models;

public class ApiResponseModel<T>
{
    public required string Status { get; set; }
    public T? Data { get; set; }
    public required string Message { get; set; }
    public Dictionary<string, string[]>? Errors { get; set; }  // Only used for error responses
    public int StatusCode { get; set; } 
}
