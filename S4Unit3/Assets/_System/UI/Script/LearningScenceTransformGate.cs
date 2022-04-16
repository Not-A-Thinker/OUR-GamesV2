using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LearningScenceTransformGate : MonoBehaviour
{
    [SerializeField] SceneControl scene;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
            scene.ToGameScene();
    }
}
