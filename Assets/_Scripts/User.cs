using System.Collections;
using System.Collections.Generic;

namespace Lym
{

    public class User
    {
        public string id { get; private set; }
        public List<Message> messages { get; private set; }

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

    }

}
