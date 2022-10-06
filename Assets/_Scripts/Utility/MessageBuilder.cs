using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Firebase.Auth;

namespace Lym {

    /// <summary>
    /// Can be called upon to generate a new message object. This is accomplished by reading the values out of the fields in the UI.
    /// </summary>
    public class MessageBuilder : MonoBehaviour
    {

        public static MessageBuilder instance;

        [SerializeField]
        private TMP_Dropdown formatDropdown, categoryDropdown;

        string templateText1 = "";
        string templateText2 = "";
        string wordText1 = "";
        string wordText2 = "";
        string conjunctionText = "";

        string gesture = "";

        Character character;

        string messageText;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != null)
            {
                Debug.Log("Instance already exists, destroying object!");
                Destroy(this);
            }
        }

        /// <summary>
        /// Replaces all blanks (****) in a template with the chosen words, then connects the templates and conjunctions. 
        /// When the message text has been fully put together, white spaces are trimmed.
        /// Upon completion, tells the message editor view to update the message display in the submission overlay.
        /// </summary>
        public void GenerateMessageText()
        {
            // inject word choice into templates
            templateText1 = templateText1.Replace("****", wordText1);
            templateText2 = templateText2.Replace("****", wordText2);

            // combine all parts
            messageText = templateText1 + " " + conjunctionText + " " + templateText2;

            // remove extra whitespaces
            messageText = messageText.Trim();

            MessageEditorView.instance.UpdateMessageDisplay(messageText);           
        }

        /// <summary>
        /// Called by the MessageEditorView whenever a piece of a message is chosen.
        /// Based on the context, will update different pieces of the message.
        /// </summary>
        /// <param name="text"> The text to store within the message. </param>
        /// <param name="context"> Which piece of the message should be affected. </param>
        public void UpdateMessage(string text, string context)
        {
            
            // populate fields based on the currently open overlay (context)
            switch (context)
            {
                case "t1":                  
                    templateText1 = text;
                    break;

                case "t2":                   
                    templateText2 = text;
                    break;

                case "w1":                              
                    wordText1 = text;
                    break;

                case "w2":
                    wordText2 = text;
                    break;

                case "c":
                    conjunctionText = text;
                    break;

                case "g":
                    gesture = text;
                    Debug.Log(gesture);
                    break;
            }       

        }

        /// <summary>
        /// Used to get the completed message from the builder
        /// </summary>
        /// <returns></returns>
        public Message GenerateMessage(User user)
        {
            if (LocationServicesUtility.instance.UpdateGPSData())
            {
                float lat = LocationServicesUtility.instance.latitude;
                float lng = LocationServicesUtility.instance.longitude;

                Character userChar = user.character;

                character = userChar == null ? userChar : new Character();

                Message message = new Message(messageText, character, gesture, lat, lng);
                return message;
            }

            return null;
        }

    }

}
