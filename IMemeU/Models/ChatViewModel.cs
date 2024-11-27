using System.ComponentModel.DataAnnotations;
using IMemeU.Data;

namespace IMemeU.Models;

public class ChatViewModel
{
    public List<Message> Messages { get; set; }
}