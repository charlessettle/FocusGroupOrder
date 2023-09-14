using System;
namespace Api.Focus.ViewModels
{
	public class NewOrder
	{
		public NewOrder() { }

		public string CreatorEmail { get; set; }
        public List<string> otherUsersEmails { get; set; } = new List<string>();
    }
}