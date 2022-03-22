namespace Lym {
    public class Message
    {
        
        public string id { get; private set; }

        // appraisals
        public int good { get; private set; }
        public int poor { get; private set; }

        // single sentence message w/ no gesture
        // single sentence message w/ gesture
        // double sentence message w/ no gesture
        // double sentence message w/ gesture
        public byte format { get; private set; }

        // a fill-in-the-blank style sentence
        public byte template1 { get; private set; }
        public byte category1 { get; private set; }
        public byte word1 { get; private set; }

        // the words that conjoin the two sentences
        public byte conjunction { get; private set; }

        // second fill-in-the-blank style sentence
        public byte template2 { get; private set; }
        public byte category2 { get; private set; }
        public byte word2 { get; private set; }

        // the emote attached to the message
        public byte gesture { get; private set; }

        // geo location
        public float latitude { get; private set; }
        public float longitude { get; private set; }

        public Message(int ownerID, int messageCount, byte format, byte template1, byte category1, byte word1, byte conjunction,
                       byte template2, byte category2, byte word2, byte gesture, float latitude, float longitude)
        {
            this.id = ownerID + "_" + messageCount;

            this.format = format;

            this.template1 = template1;
            this.category1 = category1;
            this.word1 = word1;

            this.conjunction = conjunction;

            this.template2 = template2;
            this.category2 = category2;
            this.word2 = word2;

            this.gesture = gesture;

            this.latitude = latitude;
            this.longitude = longitude;
        }
    }
}
