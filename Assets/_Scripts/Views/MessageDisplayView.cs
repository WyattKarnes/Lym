using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace Lym
{

    public class MessageDisplayView : View
    {

        // Reference to the text display of the message
        [SerializeField]
        private TextMeshProUGUI display;

        // References to character parts
        [SerializeField]
        private GameObject modelsParent;

        [SerializeField]
        private GameObject background;        

        [SerializeField]
        private GameObject maleModel;

        [SerializeField]
        private GameObject femaleModel;

        // A reference to the gesture list
        [SerializeField]
        private GestureList gestureList;

        public override void Init()
        {
            base.Init();

            // enable character models
            modelsParent.SetActive(true);
            // disable background
            background.SetActive(false);
        }

        public override void Deinit()
        {
            base.Deinit();

            // disable character models
            modelsParent.SetActive(false);
            // enable background
            background.SetActive(true);
        }

        public void LoadMessage(Message message)
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
