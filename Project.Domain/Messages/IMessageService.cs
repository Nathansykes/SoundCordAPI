namespace Project.Domain.Messages;
public interface IMessageService
{
    MessageModel CreateMessage(Guid channelId, MessageModel message);
    MessageModel GetMessage(Guid messageId);
    ICollection<MessageModel> GetMessages(Guid channelId);
    ICollection<MessageModel> GetMessagesForSongRevision(Guid channelId, Guid songRevisionId);
}
