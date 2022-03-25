using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Lym {

    public class HomepageView : MonoBehaviour
    {
        public static HomepageView instance;

        public TextMeshProUGUI welcomeLabel;

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

        public void FetchUser()
        {
            welcomeLabel.SetText("Welcome, " + FirebaseManager.instance.GetUsername());
        }
    }

}
