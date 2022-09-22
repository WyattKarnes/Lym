using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Lym
{
    /// <summary>
    /// This script controls all aspects of the message editor, once it is open. 
    /// </summary>
    public class MessageEditorView : MonoBehaviour
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

        // Temporary selection screens that will overlay instead of replace the current UI
        [Header("Overlay References")]
        public GameObject templateSelectionUI;
        public GameObject wordSelectionUI;
        public GameObject conjunctionSelectionUI;
        public GameObject gestureSelectionUI;
        public GameObject confirmationOverlay;

        [Header("Changeable Text References")]
        public TextMeshProUGUI messageDisplay;

        [Header("Gesture UI References")]

        // The button label to modify when a selection is made.
        private TextMeshProUGUI labelToChange;

        // Will be used to tell the message builder what data to fill out
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

        // For swapping formats
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
                    break;
            }
        }



        /// <summary>
        ///  Open the template choice overlay
        /// </summary>
        /// <param name="obj">Used to differentiate which button the event call came from, so the proper text can be changed. </param>
        public void OpenTemplateOverlay(TextMeshProUGUI label)
        {
            templateSelectionUI.SetActive(true);
            labelToChange = label;
        }

        /// <summary>
        /// Close the template overlay
        /// </summary>
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



        public void OpenGesturesOverlay()
        {
            gestureSelectionUI.SetActive(true);
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

        /// <summary>
        /// Results in storing a string which will be used to tell the message builder what aspect 
        /// of the message it should be updating.
        /// </summary>
        /// <param name="context"> A very short string that represents the relevant piece of a message that should be changed. </param>
        public void SetContext(string context)
        {
            choiceContext = context;
        }

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

        public void UpdateGesture(TextMeshProUGUI label)
        {
            MessageBuilder.instance.UpdateMessage(label.text, choiceContext);

            //TODO: start animating the character to preview the gesture
        }

        public void UpdateMessageDisplay(string text)
        {
            messageDisplay.SetText(text);
        }

        public void CloseOverlays()
        {
            CloseTemplateOverlay();
            CloseConjunctionOverlay();
            CloseWordsOverlay();
            CloseGestureOverlay();
            CloseConfirmationOverlay();
        }

    }

}
