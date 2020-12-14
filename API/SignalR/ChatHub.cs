using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Application.Comments;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR
{
    public class ChatHub : Hub
    {
        private readonly IMediator _mediator;
        public ChatHub(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task SendComment(Create.Command cmd)
        {
            string username = GetUsername();

            cmd.Username = username;

            var comment = await _mediator.Send(cmd);

            await Clients.Group(cmd.ActivityId.ToString()).SendAsync("ReceiveComment", comment);
        }

        private string GetUsername()
        {
            return Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public async Task AddToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            var username = GetUsername();

            await Clients.Group(groupName).SendAsync("Send", $"{username} has joined the group");
        }

        public async Task RemoveFromGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);

            var username = GetUsername();

            await Clients.Group(groupName).SendAsync("Send", $"{username} has left the group");
        }
    }
}