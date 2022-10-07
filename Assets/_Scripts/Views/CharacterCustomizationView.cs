using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Lym
{
    public class CharacterCustomizationView : View
    {

        [SerializeField]
        private GameObject modelsParent;

        [SerializeField]
        private GameObject background;

        [SerializeField]
        private GameObject maleModel;
        [SerializeField]
        private GameObject femaleModel;

        private User user;

        public override void Init()
        {
            base.Init();
            user = FirebaseManager.instance.userData;

            // Make sure the display shows what the user has saved
            if (user.character.gender)
            {
                maleModel.SetActive(true);
                femaleModel.SetActive(false);
            } else
            {
                maleModel.SetActive(false);
                femaleModel.SetActive(true);
            }

            // enable character models
            modelsParent.SetActive(true);
            // disable background
            background.SetActive(false);
        }

        public override void Deinit()
        {
            base.Init();

            // disable character models
            modelsParent.SetActive(false);
            // enable background
            background.SetActive(true);
        }

        public void SwapGender()
        {
            // Update the User's character

            if (maleModel.activeSelf)
            {
                // female
                user.character.gender = false;
                maleModel.SetActive(false);
                femaleModel.SetActive(true);
            } else
            {
                // male
                user.character.gender = true;
                maleModel.SetActive(true);
                femaleModel.SetActive(false);
            }
           
          
        }
    }

}