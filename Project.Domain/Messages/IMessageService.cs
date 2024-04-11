using Project.Domain.Messages;
using Project.Domain.Groups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Domain.Messages;
public interface IMessageService
{
    MessageModel CreateMessage(Guid channelId, MessageModel message);
    MessageModel GetMessage(Guid messageId);
    ICollection<MessageModel> GetMessages(Guid channelId);
}
