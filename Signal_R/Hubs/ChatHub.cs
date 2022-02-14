using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Signal_R.Hubs
{
    public class ChatHub: Hub
    {
        public async  Task SendMessage(string userInput,string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }


    }
}
