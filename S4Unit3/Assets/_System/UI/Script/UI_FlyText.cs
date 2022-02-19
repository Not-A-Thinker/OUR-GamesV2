using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_FlyText : MonoBehaviour
{
    public Color color;//��r���C��
    public string content;//�奻���e
    TextMeshProUGUI t;//Text�ե�

    void Start()
    {
        t = transform.GetComponent<TextMeshProUGUI>();
        t.text = content;//�]�m�ե�
        t.color = color;
        t.rectTransform.anchoredPosition = new Vector2(t.rectTransform.anchoredPosition.x, t.rectTransform.anchoredPosition.y + 30);
    }

    public float fadeTimer = 1;
    void Update()
    {
        fadeTimer -= Time.deltaTime;
        t.color = new Color(color.r, color.g, color.b, fadeTimer);
        t.rectTransform.anchoredPosition = new Vector2(t.rectTransform.anchoredPosition.x, t.rectTransform.anchoredPosition.y + (2 - fadeTimer));
        if (fadeTimer < 0)
        {
            GameObject.Destroy(this.gameObject);
        }
    }
}
