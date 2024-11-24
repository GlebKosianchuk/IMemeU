namespace IMemeU.Models;

public class MessageViewModel
{
    public int Id { get; init; }
    public string UserName { get; init; }
    public string Text { get; init; }
    public DateTime Timestamp { get; init; }
}