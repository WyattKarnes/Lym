using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Lym
{

    public class NearbyMessageView : MonoBehaviour
    {
        public Transform messageList;

        public GameObject messageListPrefab;

        public List<Message> messages = new List<Message>();

        public void Init()
        {
            // delete all message buttons that may have been added before
            ClearMessageView();

            // subscribe to FirebaseManager for nearby messages
            FirebaseManager.instance.OnNearbyMessagesLoaded += AddMessage;
            FirebaseManager.instance.OnNearbyMessageExited += RemoveMessage;

            // tell the firebase manager to start GeoFire queries
            FirebaseManager.instance.BeginGeofireQuery();
        }

        /// <summary>
        /// Called whenever this screen is open and the FirebaseManager finds a nearby message
        /// </summary>
        /// <param name="msg"></param>
        private void AddMessage(Message msg)
        {
            // ensure duplicates are not added
            if (!messages.Contains(msg))
            {
                messages.Add(msg);
                ClearMessageView();
                PopulateMessageView();
            }
        }

        private void RemoveMessage(string msgID)
        {
            messages.Find(msg =>
            {
                if (msg.id.Equals(msgID))
                {
                    messages.Remove(msg);
                    ClearMessageView();
                    PopulateMessageView();
                    return true;
                }

                return false;
            });
        }

        private void PopulateMessageView()
        {
            // go over messages data, generating MessageButtons on the way
            foreach (Message m in messages)
            {
                // spawn a message prefab
                GameObject temp = Instantiate(messageListPrefab, messageList);

                temp.GetComponent<Button>().onClick.AddListener(() =>
                {
                    UIManager.instance.MessageDisplayScreen(temp.GetComponent<MessageButton>().message);
                });


                // populate the pieces of the message prefab
                temp.GetComponent<MessageButton>().Init(m);
            }
        }

        private void ClearMessageView()
        {
            foreach (Transform temp in messageList)
            {
                // reactivate this when buttons are completed
                temp.GetComponent<Button>().onClick.RemoveAllListeners();
                Destroy(temp.gameObject);
            }
        }

        /// <summary>
        /// Unsubscribes the now inactive screen from FirebaseManager events
        /// </summary>
        private void OnDisable()
        {
            FirebaseManager.instance.OnNearbyMessagesLoaded -= AddMessage;
            FirebaseManager.instance.OnNearbyMessageExited -= RemoveMessage;
        }
    }

}
