using UnityEngine;

namespace Lym {
    public class Message
    {
        public string id;

        public string messageText;

        public int goodRatings;
        public int badRatings;

        public float latitude;
        public float longitude;

        public int gesture;
        
        // For creating NEW messages
        public Message(string messageText, int gesture, float latitude, float longitude)
        {
            // the id should be userID + the number of messages the user has.
            this.id = FirebaseManager.instance.GetUserID() + "-MSG-" + 0;
            this.messageText = messageText;
            this.gesture = gesture;

            // set the latitude and longitude using the User's phone data
            this.latitude = latitude;
            this.longitude = longitude;
        }

        // For populating a User's message list when they log in
        public Message(string id, string messageText, int goodRatings, int badRatings, float latitude, float longitude, int gesture)
        {
            this.id=id;
            this.messageText=messageText;
            this.goodRatings=goodRatings;
            this.badRatings=badRatings;
            this.latitude=latitude;
            this.longitude=longitude;
            this.gesture=gesture;
        }

        public override string ToString()
        {
            string json = JsonUtility.ToJson(this);
            return json;
        }

    }

}
