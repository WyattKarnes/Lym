using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Lym
{
    /// <summary>
    /// Whenever a screen is accessed that has multiple options, this script will generate all possible choices as buttons. 
    /// </summary>
    public class ContentPopulator : MonoBehaviour
    {
        public string[] lines;

        [SerializeField]
        private GameObject choiceButtonPrefab;

        [SerializeField]
        private Transform templateContainer, categoriesContainer, wordsContainer, conjunctionContainer, gestureContainer;


        public void PopulateScrollViews()
        {
            PopulateTemplates();
            PopulateCategories();
            PopulateConjunctions();
            PopulateGestures();
        }

        public void ClearScrollViews()
        {
            ClearTemplates();
            ClearCategories();
            ClearWords();
            ClearConjunctions();
            ClearGestures();
        }



        private void PopulateTemplates()
        {
            TextAsset templates = Resources.Load<TextAsset>("templates");
            lines = templates.text.Split('\n');

            // loop over the list of lines and create a new choice button for each one

            for(int i = 0; i < lines.Length; i++)
            {
                GameObject temp = Instantiate(choiceButtonPrefab, templateContainer);

                // give the button label the text from 'lines'
                TextMeshProUGUI tempText = temp.GetComponentInChildren<TextMeshProUGUI>();

                tempText.text = lines[i].Trim('\r');

                // grab the button component from temp
                Button tempButton = temp.GetComponent<Button>();

                // link up its event to the MessageEditorUIManager
                tempButton.onClick.AddListener(() => MessageEditorView.instance.UpdateLabel(tempText));
            }

        }

        // this method is special because it works on a dropdown and not a list of buttons
        private void PopulateCategories()
        {
            TextAsset templates = Resources.Load<TextAsset>("categories");
            lines = templates.text.Split('\n');

            // loop over the list of categories and add each one to the dropdown
            TMP_Dropdown temp = categoriesContainer.GetComponentInChildren<TMP_Dropdown>();

            List<TMP_Dropdown.OptionData> optionData = new List<TMP_Dropdown.OptionData>();

            foreach (string line in lines)
            {
                optionData.Add(new TMP_Dropdown.OptionData(line));
            }

            temp.AddOptions(optionData);

            PopulateWords(0);
        }

        private void PopulateConjunctions()
        {
            TextAsset templates = Resources.Load<TextAsset>("conjunctions");
            lines = templates.text.Split('\n');

            // loop over the list of lines and create a new choice button for each one

            for(int i = 0; i < lines.Length; i++)
            {
                GameObject temp = Instantiate(choiceButtonPrefab, conjunctionContainer);

                // give the button label the text from 'lines'
                TextMeshProUGUI tempText = temp.GetComponentInChildren<TextMeshProUGUI>();

                tempText.text = lines[i].Trim('\r');

                // grab the button component from temp
                Button tempButton = temp.GetComponent<Button>();            

                // link up its event to the MessageEditorUIManager
                tempButton.onClick.AddListener(() => MessageEditorView.instance.UpdateLabel(tempText));

            }
        }

        // The words are special, because they fall into categories. So the words are populated based on the category that is chosen.
        // As such, this method should only be called when the categories are populated, or when the category is switched in the UI.
        public void PopulateWords(int category)
        {
            // clean out the previous words to make room for the new ones
            ClearWords();

            TextAsset templates = Resources.Load<TextAsset>("words/words " + category);

            lines = templates.text.Split('\n');

            for(int i = 0; i < lines.Length; i++)
            {
                GameObject temp = Instantiate(choiceButtonPrefab, wordsContainer);

                // give the button label the text from 'lines'
                TextMeshProUGUI tempText = temp.GetComponentInChildren<TextMeshProUGUI>();

                tempText.text = lines[i].Trim('\r');

                // grab the button component from temp
                Button tempButton = temp.GetComponent<Button>();

                // link up its event to the MessageEditorUIManager
                tempButton.onClick.AddListener(() => MessageEditorView.instance.UpdateLabel(tempText));
            }
        }

        // Gestures could be special again, because they might not have words, but rather icons. 
        public void PopulateGestures()
        {
            // load a list of possible animations from text file
            TextAsset templates = Resources.Load<TextAsset>("gestures");
            lines = templates.text.Split('\n');

            // loop over list and create button for each one
            for(int i = 0; i < lines.Length; i++)
            {
                GameObject temp = Instantiate(choiceButtonPrefab, gestureContainer);

                // give the button label the text from 'lines'
                TextMeshProUGUI textRef = temp.GetComponentInChildren<TextMeshProUGUI>();

                textRef.text = lines[i].Trim('\r');

                // grab the button component from temp
                Button tempButton = temp.GetComponent<Button>();

                // link up its event to the MessageEditorUIManager
                tempButton.onClick.AddListener(() => MessageEditorView.instance.UpdateGesture(textRef));
            }
        }


        private void ClearTemplates()
        {
            foreach (Transform temp in templateContainer)
            {
                temp.GetComponent<Button>().onClick.RemoveAllListeners();
                Destroy(temp.gameObject);
            }
        }

        private void ClearCategories()
        {
            categoriesContainer.GetComponent<TMP_Dropdown>().ClearOptions();
        }

        private void ClearWords()
        {
            foreach (Transform temp in wordsContainer)
            {
                temp.GetComponent<Button>().onClick.RemoveAllListeners();
                Destroy(temp.gameObject);
            }
        }

        private void ClearConjunctions()
        {
            foreach (Transform temp in conjunctionContainer)
            {
                temp.GetComponent<Button>().onClick.RemoveAllListeners();
                Destroy(temp.gameObject);
            }
        }

        private void ClearGestures()
        {
             foreach(Transform temp in gestureContainer)
            {
                temp.GetComponent<Button>().onClick.RemoveAllListeners();
                Destroy(temp.gameObject);
            }
        }
    }

}
