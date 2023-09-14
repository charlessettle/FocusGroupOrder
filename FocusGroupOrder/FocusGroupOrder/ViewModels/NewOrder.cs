using System;
using System.Collections.Generic;

namespace FocusGroupOrder.ViewModels
{
    public class NewOrder
    {
        public NewOrder() { }

        public string CreatorEmail { get; set; }
        public List<string> otherUsersEmails { get; set; } = new List<string>();
    }
}