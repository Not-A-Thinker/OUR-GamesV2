using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuMovement : MonoBehaviour
{
    public GameObject optionsMenu;

    public GameObject optionsFirstButton, optionsCloseButton;

    public void OpenOptions()
    {
        optionsMenu.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);

        EventSystem.current.SetSelectedGameObject(optionsFirstButton);
    }

    public void CloseOptions()
    {
        optionsMenu.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);

        EventSystem.current.SetSelectedGameObject(optionsCloseButton);
    }
}
