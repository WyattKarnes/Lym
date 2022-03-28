using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Lym
{

    public class MessageButton : MonoBehaviour
    {

        public Message message;

        [SerializeField]
        private TextMeshProUGUI messageText, messageRating;


        public void Init(Message msg)
        {
            message = msg;
            messageText.text = msg.messageText;
            messageRating.text = "Good: " + msg.goodRatings + " Poor: " + msg.badRatings;
        }


    }

}
