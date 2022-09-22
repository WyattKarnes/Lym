using System.Collections;
using System.Collections.Generic;

namespace Lym
{
    [System.Serializable]
    public class User
    {
        public string id { get; private set; }

        public List<Message> messages { get; private set; }

        // A data class to keep track of the appearance of the user's avatar
        public Character character;

        public User(string id)
        {
            this.id = id;
            messages = new List<Message>();
        }

        public void AddMessage(Message newMessage)
        {
            messages.Add(newMessage);
        }

        public void RemoveMessage(Message toDelete)
        {
            for (int i = 0; i < messages.Count; i++)
            {
                if(messages[i].id == toDelete.id)
                {
                    messages.RemoveAt(i);
                }
            }
        }

        public void ClearMessages()
        {
            messages = new List<Message>();
        }

    }

}
