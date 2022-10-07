using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lym
{

    public class UIManager : MonoBehaviour
    {
        // Make a singleton
        public static UIManager instance;

        //? To Be Deleted Pending Testing
        /* Old Object References.
         * Can be removed because each view is now aware of its own objects. 
         * Much better encapsulation.
         * 
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
        */

        /* Old references to different views. No longer needed.
         * 
        // View Script References
        

        [SerializeField]
        private RegistrationView registrationView;

        [SerializeField]
        private HomepageView homepageView;

        [SerializeField]
        private NearbyMessageView nearbyMessageView;

        [SerializeField]
        private CharacterCustomizationView characterCustomizationView;

        [SerializeField]
        private MessageDisplayView messageDisplayView;
        */

        // keeping this view ref for now because it is pushed onto the stack immediately. 
        [SerializeField]
        private LoginView loginView;

        [SerializeField]
        private Stack<View> viewStack;

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

            viewStack = new Stack<View>();

            loginView.Init();
            viewStack.Push(loginView);
        }

        // Add a new view to the stack and turn it on
        public void LoadView(View view)
        {
            viewStack.Peek().Deinit();
            view.Init();
            viewStack.Push(view);
            //PrintStack(viewStack);
        }

        // This removes a view from the stack while turning it off. 
        // TODO: Should be paired with back buttons that need to be added to almost all Views
        // TODO: Should be called when android hardware return button is pressed?
        public void BackView()
        {
            viewStack.Pop().Deinit();
            //PrintStack(viewStack);
        }

        //! A simple utility for checking the contents of the view stack
        public void PrintStack(Stack<View> stack)
        {
            if (stack.Count == 0)
            {
                Debug.Log("-------------------------------------------------");
                return;
            }
                

            View v = stack.Peek();

            stack.Pop();

            Debug.Log(v);

            PrintStack(stack);

            stack.Push(v);

        }

        //? To Be Deleted Pending Testing
        #region Old Methods

        /* Old Login Loading
         * 
        public void LoginScreen()
        {
            CloseScreens();
            loginUI.SetActive(true);
        }
        */

        /* Old Registration Loading
         * 
        // Register Button
        public void RegisterScreen()
        {
            CloseScreens();
            registerUI.SetActive(true);
        }
        */

        /* Old Homepage Loading
         * 
        public void UserHomepageScreen()
        {
            CloseScreens();
            userHomepageUI.SetActive(true);
            homepageView.Init();
        }
        */

        /* Old Nearby Message Loading
         * 
        // Search for messages screen
        public void NearbyMessagesScreen()
        {
            CloseScreens();
            nearbyMessagesUI.SetActive(true);
            nearbyMessageView.Init();
        }
        */

        /* Old Message Editor Loading
         *
        // Create Message Button
        public void MessageEditorScreen()
        {
            CloseScreens();
            messageEditorUI.SetActive(true);
            MessageEditorView.instance.Init();
        }
        */

        /* Old Character Customizer Loading
         * 
        public void CharacterCustomizerScreen()
        {
            CloseScreens();
            characterCustomizerUI.SetActive(true);
            characterCustomizationView.Init();
            ActivateCharacterModel();
        }
        */

        /* Old Character Model Controls
         * 
        // These two methods should eventually be removed
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
        */

        /* Old Message Display Loading
         * 
        public void MessageDisplayScreen(Message message)
        {
            CloseScreens();
            messageDisplayUI.SetActive(true);
            messageDisplayView.Init(message);
            modelsParent.SetActive(true);
            background.SetActive(false);
        }
        */

        /* Old Screen Closing
         *
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
        }
        */

        #endregion
    }

}
