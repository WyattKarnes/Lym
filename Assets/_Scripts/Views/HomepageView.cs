using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Lym {

    public class HomepageView : MonoBehaviour
    {
        public static HomepageView instance;

        public TextMeshProUGUI welcomeLabel;
        public Transform messageList;

        public GameObject messageListPrefab;

        public ScrollRect scrollRect;
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

        public void SetWelcomeText()
        {
            welcomeLabel.SetText("Welcome, " + FirebaseManager.instance.GetUsername());
        }

        public void ClearMessageView()
        {
            foreach (Transform temp in messageList)
            {
                // reactivate this when buttons are completed
                //temp.GetComponent<Button>().onClick.RemoveAllListeners();
                Destroy(temp.gameObject);
            }
        }

        /// <summary>
        ///  Starts a chain: FirebaseManager is told to request a user's messages. Once the messages are retrieved, 
        ///  Firebase manager tells HomepageView to populate itself.
        /// </summary>
        public void FetchMessages()
        {
            FirebaseManager.instance.UpdateUserMessages();
        }

        /// <summary>
        /// Once user data is retrieved, this message can be called by the FirebaseManager
        /// </summary>
        public void PopulateMessageView()
        {
            User userRef = FirebaseManager.instance.userData;

            foreach (Message m in userRef.messages)
            {
                // spawn a message prefab
                GameObject temp = Instantiate(messageListPrefab, messageList);

                // populate the pieces of the message prefab
                temp.GetComponent<MessageButton>().Init(m);
            }
        }

    }

}
