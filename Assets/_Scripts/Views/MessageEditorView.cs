using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Lym
{
    /// <summary>
    /// This script controls all aspects of the message editor, once it is open. 
    /// </summary>
    public class MessageEditorView : View
    {
        public static MessageEditorView instance;

        // GameObjects that are active/not active based on template choice
        [Header("Optional UI References")]
        public GameObject gestureButton;
        public GameObject gestureLabel;

        public GameObject conjunctionButton;
        public GameObject conjunctionLabel;

        public GameObject template2Button;
        public GameObject template2Label;

        public GameObject words2Button;
        public GameObject words2Label;

        // Temporary selection screens that will 'overlay' instead of replace the current UI
        [Header("Overlay References")]
        public GameObject templateSelectionUI;
        public GameObject wordSelectionUI;
        public GameObject conjunctionSelectionUI;
        public GameObject gestureSelectionUI;
        public GameObject confirmationOverlay;

        [Header("Changeable Text References")]
        public TextMeshProUGUI messageDisplay;

        [Header("Gesture UI References")]
        private User user;
        public GameObject modelsParent;
        public GameObject background;
        public GameObject maleModel;
        public GameObject femaleModel;

        // The button label to modify when a selection is made.
        private TextMeshProUGUI labelToChange;


        /// <summary>
        /// Will be used to tell the message builder what data to fill out. 
        /// Needed because Unity Buttons only pass one argument per call. 
        /// </summary>
        private string choiceContext;

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

        // Activate or deactivate the correct character model 
        public override void Init()
        {
            base.Init();

            user = FirebaseManager.instance.userData;

            // Make sure the display shows what the user has saved
            if (user.character.gender)
            {
                maleModel.SetActive(true);
                femaleModel.SetActive(false);
            }
            else
            {
                maleModel.SetActive(false);
                femaleModel.SetActive(true);
            }
        }

        public override void Deinit()
        {
            base.Deinit();
            DeactivateCharacterModel();
        }


        /// <summary>
        /// Called from a dropdown in the scene.
        /// Activates or deactivates the relevant GameObjects based on the chosen format of a message.
        /// </summary>
        /// <param name="choice"></param>
        public void ChangeFormat(int choice)
        {
            switch (choice)
            {

                case 0:
                    // Single phrase, no gesture
                    gestureButton.SetActive(false);
                    gestureLabel.SetActive(false);

                    conjunctionButton.SetActive(false);
                    conjunctionLabel.SetActive(false);

                    template2Button.SetActive(false);
                    template2Label.SetActive(false);

                    words2Button.SetActive(false);
                    words2Label.SetActive(false);

                    DeactivateCharacterModel();
                    break;

                case 1:
                    // single phrase, with gesture
                    gestureButton.SetActive(true);
                    gestureLabel.SetActive(true);

                    conjunctionButton.SetActive(false);
                    conjunctionLabel.SetActive(false);

                    template2Button.SetActive(false);
                    template2Label.SetActive(false);

                    words2Button.SetActive(false);
                    words2Label.SetActive(false);

                    ActivateCharacterModel();
                    break;

                case 2:
                    // dual phrase, no gesture
                    gestureButton.SetActive(false);
                    gestureLabel.SetActive(false);

                    conjunctionButton.SetActive(true);
                    conjunctionLabel.SetActive(true);

                    template2Button.SetActive(true);
                    template2Label.SetActive(true);

                    words2Button.SetActive(true);
                    words2Label.SetActive(true);

                    DeactivateCharacterModel();
                    break;

                case 3:
                    // dual phrase, with gesture
                    gestureButton.SetActive(true);
                    gestureLabel.SetActive(true);

                    conjunctionButton.SetActive(true);
                    conjunctionLabel.SetActive(true);

                    template2Button.SetActive(true);
                    template2Label.SetActive(true);

                    words2Button.SetActive(true);
                    words2Label.SetActive(true);

                    ActivateCharacterModel();
                    break;
            }
        }

        #region Overlay Handling

        public void OpenTemplateOverlay(TextMeshProUGUI label)
        {
            templateSelectionUI.SetActive(true);
            labelToChange = label;
        }

  
        public void CloseTemplateOverlay()
        {
            templateSelectionUI.SetActive(false);
        }



        public void OpenConjunctionOverlay(TextMeshProUGUI label)
        {
            conjunctionSelectionUI.SetActive(true);
            labelToChange = label;
        }

        public void CloseConjunctionOverlay()
        {
            conjunctionSelectionUI.SetActive(false);
        }



        public void OpenWordsOverlay(TextMeshProUGUI label)
        {
            wordSelectionUI.SetActive(true);
            labelToChange = label;
        }

        public void CloseWordsOverlay()
        {
            wordSelectionUI.SetActive(false);
        }



        public void OpenGesturesOverlay(TextMeshProUGUI label)
        {
            gestureSelectionUI.SetActive(true);
            labelToChange = label;
        }

        public void CloseGestureOverlay()
        {
            gestureSelectionUI.SetActive(false);
        }



        public void OpenConfirmationOverlay()
        {
            confirmationOverlay.SetActive(true);
        }

        public void CloseConfirmationOverlay()
        {
            confirmationOverlay.SetActive(false);
        }

        public void CloseOverlays()
        {
            CloseTemplateOverlay();
            CloseConjunctionOverlay();
            CloseWordsOverlay();
            CloseGestureOverlay();
            CloseConfirmationOverlay();
        }

        #endregion

        /// <summary>
        /// Results in storing a string which will be used to tell the message builder what aspect 
        /// of the message it should be updating. This will be called whenever a choice button is selected. 
        /// </summary>
        /// <param name="context"> A very short string that represents the relevant piece of a message that should be changed. </param>
        public void SetContext(string context)
        {
            choiceContext = context;
        }

        /// <summary>
        /// Used in the Message Dropdown Populator to give generated buttons
        /// something to do.
        /// </summary>
        /// <param name="label"></param>
        public void UpdateLabel(TextMeshProUGUI label)
        {
            // update the text of the label to change
            labelToChange.text = label.text;

            // dereference the label to change
            labelToChange = null;

            // notify the message builder to update its holdings
            MessageBuilder.instance.UpdateMessage(label.text, choiceContext);

            CloseOverlays();
        }

        /// <summary>
        /// Used in the Message Dropdown Populator to give generated buttons something to do.
        /// </summary>
        /// <param name="label"></param>
        public void UpdateGesture(TextMeshProUGUI label)
        {

            labelToChange.text = label.text;

            labelToChange = null;

            MessageBuilder.instance.UpdateMessage(label.text, choiceContext);



            Debug.Log(user.character.gender);

            if (user.character.gender)
            {
                maleModel.GetComponent<Animator>().Play(label.text);
            } else
            {
                femaleModel.GetComponent<Animator>().Play(label.text);
            }

            CloseOverlays();
        }

        /// <summary>
        /// Used by the message builder utility to update the text on the confirmation message
        /// </summary>
        /// <param name="text"></param>
        public void UpdateMessageDisplay(string text)
        {
            messageDisplay.SetText(text);
        }

     
        private void ActivateCharacterModel()
        {
            modelsParent.SetActive(true);
            background.SetActive(false);
        }

        private void DeactivateCharacterModel()
        {
            modelsParent.SetActive(false);
            background.SetActive(true);
        }

    }

}
