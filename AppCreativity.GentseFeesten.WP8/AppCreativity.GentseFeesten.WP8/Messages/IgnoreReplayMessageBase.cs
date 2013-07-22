using GalaSoft.MvvmLight.Messaging;

namespace AppCreativity.GentseFeesten.WP8.Messages
{
    /// <summary>
    /// If you want to use the last message replay messenger but for a message you don't want to replay the last message when registering a handler then derive of this class...
    /// </summary>
    public class IgnoreReplayMessageBase : MessageBase
    {
    }
}