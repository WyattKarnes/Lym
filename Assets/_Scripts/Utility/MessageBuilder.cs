using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Firebase.Auth;

namespace Lym {

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
        int gesture = -1;


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

        public void ConfirmMessage()
        {
            templateText1 = templateText1.Replace("****", wordText1);
            templateText2 = templateText2.Replace("****", wordText2);

            messageText = templateText1 + " " + conjunctionText + " " + templateText2;

            // remove all carriage return chars so that the text displays correctly
            for (int i = 0; i < messageText.Length; i++)
            {
                if (messageText[i].Equals('\r'))
                {               
                    messageText = messageText.Remove(i, 1);
                }
            }

            MessageEditorView.instance.UpdateMessageDisplay(messageText);           
        }

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
            }       

        }

        public Message GenerateMessage()
        {
            Message message = new Message(messageText, gesture);

            return message;
        }
    }

}
