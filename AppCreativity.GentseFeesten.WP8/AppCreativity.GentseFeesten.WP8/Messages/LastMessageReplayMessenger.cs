using System;
using System.Collections.Generic;
using GalaSoft.MvvmLight.Messaging;

namespace AppCreativity.GentseFeesten.WP8.Messages
{
    /// <summary>
    /// The last message replay messenger - this uses composition of the MVVMlight Messenger, the reason we created this was
    /// to get round a 'chicken and egg' situation when you send a message to a receiver before the receiver has registered
    /// to receieve the message.
    /// So this implementation remembers the last message for a particular message type and when ever a subscriber for that
    /// message type registers it automatically gets the last message.
    /// http://blogs.xamlninja.com/xaml/wp7-contrib-the-last-messenger
    /// </summary>
    public sealed class LastMessageReplayMessenger : IMessenger, IDisposable
    {
        /// <summary>
        /// The standard MVVMLight messenger.
        /// </summary>
        private readonly Messenger messenger = new Messenger();

        /// <summary>
        /// The last messages dictionary.
        /// </summary>
        private readonly IDictionary<object, object> lastMessages = new Dictionary<object, object>();

        /// <summary>
        /// The sync object to make accessing the dictionary thread safe.
        /// </summary>
        private readonly object sync = new object();

        /// <summary>
        /// The dispose.
        /// </summary>
        public void Dispose()
        {
            ClearLastMessages();
        }

        /// <summary>
        /// Registers a recipient to receive messages.
        /// </summary>
        /// <param name="recipient">The registering recipient.</param>
        /// <param name="action">The action to be executed when a message arrives.</param>
        /// <typeparam name="TMessage">The type of message.</typeparam>
        public void Register<TMessage>(object recipient, Action<TMessage> action)
        {
            messenger.Register(recipient, action);
            SendLastMessage(action);
        }

        /// <summary>
        /// Registers a recipient to receive messages.
        /// </summary>
        /// <param name="recipient">The registering recipient.</param>
        /// <param name="receiveDerivedMessagesToo">Indicates if the recipient receives derived types of the message.</param>
        /// <param name="action">The action to be executed when a message arrives.</param>
        /// <typeparam name="TMessage">The type of message.</typeparam>
        public void Register<TMessage>(object recipient, bool receiveDerivedMessagesToo, Action<TMessage> action)
        {
            messenger.Register(recipient, receiveDerivedMessagesToo, action);
            SendLastMessage(action);
        }

        /// <summary>
        /// Sends a message.
        /// </summary>
        /// <param name="message">The message to be sent.</param>
        /// <typeparam name="TMessage">The type of message.</typeparam>
        public void Send<TMessage>(TMessage message)
        {
            UpdateLastMessage(message);
            messenger.Send(message);
        }

        /// <summary>
        /// Sends a message.
        /// </summary>
        /// <param name="message">The message to be sent.</param>
        /// <typeparam name="TMessage">The type of message.</typeparam>
        /// <typeparam name="TTarget">The type of message target.</typeparam>
        public void Send<TMessage, TTarget>(TMessage message)
        {
            UpdateLastMessage(message);
            messenger.Send<TMessage, TTarget>(message);
        }

        /// <summary>
        /// The unregister a recipients for messages.
        /// </summary>
        /// <param name="recipient">The registered recipient.</param>
        public void Unregister(object recipient)
        {
            messenger.Unregister(recipient);
        }

        /// <summary>
        /// The unregister.
        /// </summary>
        /// <param name="recipient">The recipient.</param>
        /// <typeparam name="TMessage">The type of message target.</typeparam>
        public void Unregister<TMessage>(object recipient)
        {
            messenger.Unregister<TMessage>(recipient);
        }

        /// <summary>
        /// Unregisters a particular message for a recipient.
        /// </summary>
        /// <param name="recipient">The recipient.</param>
        /// <param name="action">The action.</param>
        /// <typeparam name="TMessage">The type of message target.</typeparam>
        public void Unregister<TMessage>(object recipient, Action<TMessage> action)
        {
            messenger.Unregister(recipient, action);
        }

        /// <summary>
        /// Clears last message for a particular message type.
        /// </summary>
        /// <typeparam name="TMessage">The type of message target.</typeparam>
        public void ClearLastMessage<TMessage>()
        {
            var messageType = typeof(TMessage);
            lock (sync)
            {
                if (lastMessages.ContainsKey(messageType))
                {
                    lastMessages.Remove(messageType);
                }
            }
        }

        /// <summary>
        /// Clears all the last messages.
        /// </summary>
        public void ClearLastMessages()
        {
            lock (sync)
            {
                lastMessages.Clear();
            }
        }

        /// <summary>
        /// Updates the last message for a particular message type.
        /// </summary>
        /// <param name="message">The last message.</param>
        /// <typeparam name="TMessage">The type of message target.</typeparam>
        private void UpdateLastMessage<TMessage>(TMessage message)
        {
            if (message is IgnoreReplayMessageBase)
            {
                return;
            }

            var messageType = typeof(TMessage);
            lock (sync)
            {
                if (lastMessages.ContainsKey(messageType))
                {
                    lastMessages[message.GetType()] = message;
                }
                else
                {
                    lastMessages.Add(messageType, message);
                }
            }
        }

        /// <summary>
        /// Sends the last message for a particular type to the action.
        /// </summary>
        /// <param name="action">The action to b executed for the last action.</param>
        /// <typeparam name="TMessage">The type of message target.</typeparam>
        private void SendLastMessage<TMessage>(Action<TMessage> action)
        {
            var messageType = typeof(TMessage);
            object lastMessage;
            lock (sync)
            {
                if (!lastMessages.TryGetValue(messageType, out lastMessage))
                {
                    return;
                }
            }

            action((TMessage)lastMessage);
        }

        public void Register<TMessage>(object recipient, object token, bool receiveDerivedMessagesToo, Action<TMessage> action)
        {
            messenger.Register(recipient, token, receiveDerivedMessagesToo, action);
            SendLastMessage(action);
        }

        public void Register<TMessage>(object recipient, object token, Action<TMessage> action)
        {
            messenger.Register(recipient, token, action);
            SendLastMessage(action);
        }

        public void Send<TMessage>(TMessage message, object token)
        {
            UpdateLastMessage(message);
            messenger.Send<TMessage>(message, token);
        }

        public void Unregister<TMessage>(object recipient, object token, Action<TMessage> action)
        {
            messenger.Unregister(recipient, token, action);
        }

        public void Unregister<TMessage>(object recipient, object token)
        {
            messenger.Unregister<TMessage>(recipient, token);
        }
    }
}