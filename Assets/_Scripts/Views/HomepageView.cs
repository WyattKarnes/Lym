using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Lym {

    public class HomepageView : MonoBehaviour
    {

        public TextMeshProUGUI welcomeLabel;
        public Transform messageList;

        public GameObject messageListPrefab;

        public ScrollRect scrollRect;
        
        public void Init()
        {
            SetWelcomeText();
            ClearMessageView();
            FetchMessages();
        }

        private void SetWelcomeText()
        {
            welcomeLabel.SetText("Welcome, " + FirebaseManager.instance.GetUsername());
        }

        private void ClearMessageView()
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
        private void FetchMessages()
        {
            FirebaseManager.instance.OnUserMessagesLoaded += PopulateMessageView;
            FirebaseManager.instance.UpdateUserMessages(); 
        }


        /// <summary>
        /// Once user data is retrieved, this message can be called by the FirebaseManager
        /// </summary>
        private void PopulateMessageView()
        {
            User userRef = FirebaseManager.instance.userData;

            foreach (Message m in userRef.messages)
            {
                // spawn a message prefab
                GameObject temp = Instantiate(messageListPrefab, messageList);

                // populate the pieces of the message prefab
                temp.GetComponent<MessageButton>().Init(m);
            }

            // unsubscribe from the event
            FirebaseManager.instance.OnUserMessagesLoaded -= PopulateMessageView;
        }

    }

}
