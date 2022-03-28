using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopupUtility : MonoBehaviour
{
    public static PopupUtility instance;

    [SerializeField]
    private GameObject popupWindow;
    [SerializeField]
    private TextMeshProUGUI popupDisplay;
    [SerializeField]
    private GameObject dismissButton;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        } else
        {
            Destroy(this);
        }
    }

    /// <summary>
    /// Display a popup message with the given text. Must be dismissed by button press. 
    /// </summary>
    /// <param name="text"></param>
    public void DisplayPopup(string text)
    {
        popupDisplay.text = text;
        popupWindow.SetActive(true);
        dismissButton.SetActive(true);
    }

    /// <summary>
    /// Display a popup message with the given text. Will dismiss automatically after the given time.
    /// </summary>
    /// <param name="text"></param>
    /// <param name="timeToDisplay"></param>
    public void DisplayPopup(string text, float timeToDisplay)
    {
        popupDisplay.text = text;
        dismissButton.SetActive(false);
        popupWindow.SetActive(true);
        StartCoroutine(ClosePopupAfterTime(timeToDisplay));
    }

    /// <summary>
    /// Dismiss the popup message
    /// </summary>
    public void ClosePopup()
    {
        popupDisplay.text = "";
        popupWindow.SetActive(false);
    }

    private IEnumerator ClosePopupAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        ClosePopup();
    }
}
