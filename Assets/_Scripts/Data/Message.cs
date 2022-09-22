using UnityEngine;
using System;


namespace Lym
{
    /// <summary>
    /// A data class to compile all of the information about a message.
    /// </summary>
    [System.Serializable]
    public class Message : MonoBehaviour, IEquatable<Message>
    {
        public string id;

        public string messageText;

        public int goodRatings;
        public int badRatings;

        public float latitude;
        public float longitude;

        public Character character;

        public string gesture;

        // For creating NEW messages
        public Message(string messageText, Character character, string gesture, float latitude, float longitude)
        {
            // the id should be userID + the number of messages the user has.
            this.id = FirebaseManager.instance.GetUserID() + "-MSG-" + FirebaseManager.instance.userData.messages.Count;
            this.messageText = messageText;
            this.character = character;
            this.gesture = gesture;

            // set the latitude and longitude using the User's phone data
            this.latitude = latitude;
            this.longitude = longitude;
        }

        // For populating a User's message list when they log in
        public Message(string id, string messageText, Character character, string gesture, int goodRatings, int badRatings, float latitude, float longitude)
        {
            this.id = id;
            this.messageText = messageText;
            this.goodRatings = goodRatings;
            this.badRatings = badRatings;
            this.latitude = latitude;
            this.longitude = longitude;
            this.character = character;
            this.gesture = gesture;
        }

        public bool Equals(Message other)
        {
            if(other == null)
            {
                return false;
            }

            return (this.id.Equals(other.id));
        }

        public override string ToString()
        {
            string json = JsonUtility.ToJson(this);
            return json;
        }

    }

}
