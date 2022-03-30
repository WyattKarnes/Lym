using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lym
{

    public class UIManager : MonoBehaviour
    {
        // Make a singleton
        public static UIManager instance;

        // Screen object variables
        public GameObject loginUI;
        public GameObject registerUI;
        public GameObject userHomepageUI;
        public GameObject messageEditorUI;
        public GameObject nearbyMessagesUI;

        // Set up singleton
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

        // Functions to change the login screen UI

        // Back Button
        public void LoginScreen()
        {
            CloseScreens();
            loginUI.SetActive(true);
        }

        // Register Button
        public void RegisterScreen()
        {
            CloseScreens();
            registerUI.SetActive(true);
        }

        // On Successful login
        public void UserHomepageScreen()
        {
            CloseScreens();
            userHomepageUI.SetActive(true);
            HomepageView.instance.SetWelcomeText();
            HomepageView.instance.ClearMessageView();
            HomepageView.instance.FetchMessages();
        }

        // Search for messages screen
        public void NearbyMessagesScreen()
        {
            CloseScreens();
            nearbyMessagesUI.SetActive(true);
            NearbyMessageView.instance.ClearMessageView();
            //NearbyMessageView.instance.PopulateMessageView();
        }

        // Create Message Button
        public void MessageEditorScreen()
        {
            CloseScreens();
            messageEditorUI.SetActive(true);
        }

        // This method prefaces all screen swaps. 
        private void CloseScreens()
        {        
            loginUI.SetActive(false);
            loginUI.SetActive(false);
            registerUI.SetActive(false);
            userHomepageUI.SetActive(false);
            nearbyMessagesUI.SetActive(false);
            messageEditorUI.SetActive(false);
        }
    }

}
