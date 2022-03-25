namespace Lym {
    public class Message
    {
        public string id { get; set; }

        public string messageText;

        public int goodRatings;
        public int badRatings;

        public float latitude;
        public float longitude;

        public int gesture;
        
        public Message(string messageText, int gesture)
        {
            // the id should be userID + the number of messages the user has.
            id = FirebaseManager.instance.GetUserID() + "-MSG-" + 0;
            this.messageText = messageText;
            this.gesture = gesture;
        }


        public override string ToString()
        {
            return messageText;
        }
    }

}
