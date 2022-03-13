using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause_Control : MonoBehaviour
{
    SceneControl sceneControl;

    // Start is called before the first frame update
    void Start()
    {
        sceneControl = new SceneControl();
    }

    // Update is called once per frame
    void Update()
    {
        if(this.gameObject.activeInHierarchy)
        {
            if(Input.GetButtonDown("Submit"))
            {
                sceneControl.ToStarScence();
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                sceneControl.ToGameScene();
                Time.timeScale = 1;
            }
        }
    }
}
