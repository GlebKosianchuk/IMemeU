using System;
using System.ComponentModel.DataAnnotations;

namespace IMemeU.Data
{
    public class Message
    {
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public DateTime SentAt { get; set; }

        [Required]
        public int SenderId { get; set; }
        public User Sender { get; set; }

        [Required]
        public int ReceiverId { get; set; }
        public User Receiver { get; set; }
    }
}