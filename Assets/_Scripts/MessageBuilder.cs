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
        int gesture = 0;


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

            Message message = new Message(messageText, gesture);

            Debug.Log(messageText);
        }

        public void UpdateMessage(string text, string context)
        {
            
            // populate fields based on the currently open overlay (context)
            switch (context)
            {
                case "t1":
                    Debug.Log("The context is template 1, which has been updated to say " + text);
                    templateText1 = text;
                    break;

                case "t2":
                    Debug.Log("The context is template 2, which has been updated to say " + text);
                    templateText2 = text;
                    break;

                case "w1":
                    Debug.Log("The context is words 1, which has been updated to say " + text);                                 
                    wordText1 = text;
                    break;

                case "w2":
                    Debug.Log("The context is words 2, which has been updated to say " + text);
                    wordText2 = text;
                    break;

                case "c":
                    Debug.Log("The context is conjunction, which has been updated to say " + text);
                    conjunctionText = text;
                    break;
            }       

        }
    }

}
