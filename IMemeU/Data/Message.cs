namespace IMemeU.Data;

public class Message
{
    public int Id { get; init; }
    public string UserName { get; init; }
    public string Text { get; init; }
    public DateTimeOffset Timestamp { get; init; }
}