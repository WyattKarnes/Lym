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
            loginUI.SetActive(true);
            registerUI.SetActive(false);
        }

        // Register Button
        public void RegisterScreen()
        {
            loginUI.SetActive(false);
            registerUI.SetActive(true);
        }

        // On Successful login
        public void UserHomepageScreen()
        {
            userHomepageUI.SetActive(true);
            loginUI.SetActive(false);
            HomepageView.instance.FetchUser();
        }

        // Create Message Button
        public void MessageEditorScreen()
        {
            messageEditorUI.SetActive(true);
            userHomepageUI.SetActive(false);
        }
    }

}
