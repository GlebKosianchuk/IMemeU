using IMemeU.Data;

namespace IMemeU.Models;

public class ChatViewModel
{
    public List<User> Users { get; set; }
    public List<Message> Messages { get; set; }
    public int SelectedUserId { get; set; }
}