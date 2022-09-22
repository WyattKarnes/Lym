using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Lym
{

    public class MessageDisplayView : MonoBehaviour
    {

        // Reference to the text display of the message
        [SerializeField]
        private TextMeshProUGUI display;

        // References to character parts
        [SerializeField]
        private GameObject maleModel;

        [SerializeField]
        private GameObject femaleModel;

        // A reference to the gesture list
        [SerializeField]
        private GestureList gestureList;

        public void Init(Message message)
        {
            display.text = message.messageText;

            if(message.character == null)
            {
                return;
            }

            SetGender(message.character);
        }

        private void SetGender(Character character)
        {
            if (character.gender)
            {
                maleModel.SetActive(true);
                femaleModel.SetActive(false);
            } else
            {
                maleModel.SetActive(false);
                femaleModel.SetActive(true);
            }
        }
    }

}
