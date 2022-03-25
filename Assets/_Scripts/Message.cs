namespace Lym {
    public class Message
    {
        public int Id { get; set; }

        public string messageText;

        public int goodRatings;
        public int badRatings;

        public float latitude;
        public float longitude;

        public int gesture;
        
        public Message(string messageText, int gesture)
        {
            this.messageText = messageText;
            this.gesture = gesture;
        }


        public override string ToString()
        {
            return messageText;
        }
    }

}
