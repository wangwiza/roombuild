using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuLogic : MonoBehaviour
{
    public GameObject menuScreen;
    private bool isOpen = false;
    public TMP_Text text;

    public void OpenMenu()
    {
        isOpen = !isOpen;
        menuScreen.SetActive(isOpen);
        if (isOpen) {
            text.text = "Close";
        } else {
            text.text = "Open";
        }
    }
}
