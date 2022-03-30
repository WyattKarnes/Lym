using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lym
{

    public class NearbyMessageView : MonoBehaviour
    {

        public static NearbyMessageView instance;

        public Transform messageList;

        public GameObject messageListPrefab;

        public List<Message> messages = new List<Message>();

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this);
            }
        }

        public void ClearMessages()
        {
            messages.Clear();
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

        public void AddMessage(Message msg)
        {
            messages.Add(msg);
            PopulateMessageView();
        }

        public void PopulateMessageView()
        {
            // go over messages data, generating MessageButtons on the way
            foreach (Message m in messages)
            {
                // spawn a message prefab
                GameObject temp = Instantiate(messageListPrefab, messageList);

                // populate the pieces of the message prefab
                temp.GetComponent<MessageButton>().Init(m);
            }
        }

    }

}
