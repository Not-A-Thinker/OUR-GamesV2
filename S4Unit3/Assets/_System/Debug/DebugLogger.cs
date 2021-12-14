using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugLogger : MonoBehaviour
{
    public GameObject debugPanel,playerControlPanel;
    public Text logText,controlText;

    public bool isOpen = false;

    void Start()
    {
        ShowMsgOnDebugPanel("Debug Mode", "On"); //Case Use
        ShowMsgOnDebugPanel("WindBlade", "Press 1");
        ShowMsgOnDebugPanel("VacuumPressure", "Press 2");
        ShowMsgOnDebugPanel("BubbleAttack", "Press 3");
        ShowMsgOnDebugPanel("TornadoAttack", "Press 4");
        ShowMsgOnDebugPanel("TornadoGattai", "Press 5");
        ShowMsgOnDebugPanel("TornadoFour", "Press 6");
        ShowMsgOnDebugPanel("'AI'Enable", "Press KeyPad0");

        ShowMsgOnControlPanel("²¾°Ê", "ASDW(P1),UPDownLR(P2)");
        ShowMsgOnControlPanel("§l©M¥¸", "left mouse(P1),(numKey 1(P2)");
        ShowMsgOnControlPanel("´_¬¡", "F(P1),(numKey 3(P2)");
        ShowMsgOnControlPanel("°jÁ×", "Space(P1),RightShift(P2)");

        if (!isOpen)
        {
            debugPanel.SetActive(false);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote) && !isOpen)
        {
            debugPanel.SetActive(true);
            isOpen = true;
        }
        else if (Input.GetKeyDown(KeyCode.BackQuote) && isOpen)
        {
            debugPanel.SetActive(false);
            isOpen = false;
        }

    }

    public void ShowMsgOnDebugPanel(string header, string msg)
    {
        logText.text += header + ": " + msg + "\n";
    }

    public void ShowMsgOnControlPanel(string header, string msg)
    {
        controlText.text += header + ": " + msg + "\n";
    }
}
