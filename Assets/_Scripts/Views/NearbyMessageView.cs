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

        public void ClearMessageView()
        {
            foreach (Transform temp in messageList)
            {
                // reactivate this when buttons are completed
                //temp.GetComponent<Button>().onClick.RemoveAllListeners();
                Destroy(temp.gameObject);
            }
        }

        public void PopulateMessageView()
        {
            // request the necessary data from Firebase

            // go over that data, generating MessageButtons on the way
        }

    }

}
