using System;
using UnityEngine;
using UnityEngine.UI;

public class ImageAlphaThreshold : MonoBehaviour
{
    private const float AlphaThreshold = 1f;
    
    [SerializeField] private Image _image;

    private void Awake()
    {
        if (!_image)
        {
            _image = GetComponent<Image>();

            if (!_image)
            {
                throw new Exception("Cannot find Image component");
            }
        }

        _image.alphaHitTestMinimumThreshold = AlphaThreshold;
    }
}