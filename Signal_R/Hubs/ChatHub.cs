using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Signal_R.DAL;
using Signal_R.Models;

namespace Signal_R.Hubs
{
    public class ChatHub: Hub
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly Context _context;

        public ChatHub(UserManager<AppUser>userManager, Context context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async  Task SendMessage(string userInput,string message)
        {
            await Clients.All.SendAsync("ReceiveMessage",userInput, message);
        }

        public override Task OnConnectedAsync()
        {
            AppUser user = _userManager.FindByNameAsync(Context.User.Identity.Name).Result;
            user.ConnectionId = Context.ConnectionId;
             _userManager.UpdateAsync(user);
            Clients.All.SendAsync("Connected", user.Id);
            _context.SaveChanges();
            return base.OnConnectedAsync();

        }
        public async Task UserKeyup(string id, string typing)
        {
            AppUser user = await _userManager.FindByIdAsync(id);
            await Clients.Client(user.ConnectionId).SendAsync("UserTyping", user.Id, typing);
        }


       

    }
}
