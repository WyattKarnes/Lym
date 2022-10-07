using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lym
{

    public class UIManager : MonoBehaviour
    {
        // Make a singleton
        public static UIManager instance;

        // Screen Object References
        public GameObject loginUI;
        public GameObject registerUI;
        public GameObject userHomepageUI;
        public GameObject messageEditorUI;
        public GameObject nearbyMessagesUI;
        public GameObject characterCustomizerUI;
        public GameObject messageDisplayUI;
        public GameObject modelsParent;
        public GameObject background;

        // View Script References
        [SerializeField]
        private HomepageView homepageView;

        [SerializeField]
        private NearbyMessageView nearbyMessageView;

        [SerializeField]
        private CharacterCustomizationView characterCustomizationView;

        [SerializeField]
        private MessageDisplayView messageDisplayView;

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
            homepageView.Init();
        }

        // Search for messages screen
        public void NearbyMessagesScreen()
        {
            CloseScreens();
            nearbyMessagesUI.SetActive(true);
            nearbyMessageView.Init();
        }

        // Create Message Button
        public void MessageEditorScreen()
        {
            CloseScreens();
            messageEditorUI.SetActive(true);
            MessageEditorView.instance.init();
        }

        public void CharacterCustomizerScreen()
        {
            CloseScreens();
            characterCustomizerUI.SetActive(true);
            characterCustomizationView.Init();
            ActivateCharacterModel();
        }

        public void ActivateCharacterModel()
        {
            modelsParent.SetActive(true);
            background.SetActive(false);
        }

        public void DeactivateCharacterModel()
        {
            modelsParent.SetActive(false);
            background.SetActive(true);
        }

        public void MessageDisplayScreen(Message message)
        {
            CloseScreens();
            messageDisplayUI.SetActive(true);
            messageDisplayView.Init(message);
            modelsParent.SetActive(true);
            background.SetActive(false);
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

            characterCustomizerUI.SetActive(false);
            DeactivateCharacterModel();
        }
    }

}
