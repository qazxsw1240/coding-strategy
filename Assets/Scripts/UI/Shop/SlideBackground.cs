using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlideBackground : MonoBehaviour
{
    [SerializeField] ScrollRect scrollRect;
    private Image image;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        Color color = image.color;
        color.a = 0.7f * scrollRect.horizontalNormalizedPosition;
        image.color = color;
    }
}
