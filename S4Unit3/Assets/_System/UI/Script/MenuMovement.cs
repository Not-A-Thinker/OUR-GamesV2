using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuMovement : MonoBehaviour
{
    public GameObject optionsMenu;

    public GameObject optionsFirstButton, optionsCloseButton;

    CanvasGroup canvasGroup;

    private void Start()
    {
        if(GetComponent<CanvasGroup>()!=null)
            canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OpenOptions()
    {
        optionsMenu.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);

        EventSystem.current.SetSelectedGameObject(optionsFirstButton);

        canvasGroup.alpha = 0;
    }

    public void CloseOptions()
    {
        optionsMenu.SetActive(false);

        EventSystem.current.SetSelectedGameObject(null);

        EventSystem.current.SetSelectedGameObject(optionsCloseButton);

        canvasGroup.alpha = 1;
    }
}
