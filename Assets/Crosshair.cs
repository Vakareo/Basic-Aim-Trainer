using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    [SerializeField] CrosshairData crosshair;
    [SerializeField] GameObject imagePrefab;
    private RectTransform[] images = new RectTransform[10];
    private Vector3[] directions = new Vector3[]{
        Vector3.up,

        Vector3.left,
        Vector3.down,
        Vector3.right,
        Vector3.zero
    };

    private void Awake()
    {
        InstantiateCrosshair();
        SetInitialPositions();
        UpdateCrosshair();
    }
    private void InstantiateCrosshair()
    {
        var rects = new List<RectTransform>();
        for (int i = 0; i < images.Length; i++)
        {
            var image = Instantiate(imagePrefab, transform);
            rects.Add(image.GetComponent<RectTransform>());
        }
        images = rects.ToArray();
    }
    private void UpdateCrosshair()
    {
        UpdateRadius();
        UpdateColor();
        UpdateVisibility();
    }

    private void UpdateColor()
    {
        for (int i = 0; i < images.Length; i++)
        {
            if (i >= images.Length / 2)
            {
                images[i].GetComponent<RawImage>().color = GetColor();
            }
            else
            {
                images[i].GetComponent<RawImage>().color = GetOutlineColor();
            }
        }
    }

    private Color GetColor()
    {
        return new Color32(crosshair.color.red, crosshair.color.green, crosshair.color.blue, crosshair.color.alpha);
    }

    private Color GetOutlineColor()
    {
        return new Color32(crosshair.outlineColor.red, crosshair.outlineColor.green, crosshair.outlineColor.blue, crosshair.outlineColor.alpha);
    }

    private void SetInitialPositions()
    {
        for (int i = 0; i < images.Length; i++)
        {
            var dirIndex = i % (images.Length / 2);
            images[i].pivot = new Vector2(0.5f, 0);
            images[i].localPosition = Vector3.zero;
            images[i].localRotation = Quaternion.identity;
            images[i].Rotate(new Vector3(0, 0, dirIndex * 90));
            if (dirIndex == images.Length / 2 - 1)
                images[i].pivot = new Vector2(0.5f, 0.5f);
        }
    }

    private void UpdateVisibility()
    {
        for (int i = 0; i < images.Length; i++)
        {
            images[i].gameObject.SetActive(true);
        }
        var dotIndex = images.Length / 2 - 1;
        if (!crosshair.centerDot)
        {
            images[dotIndex].gameObject.SetActive(false);
            images[dotIndex + dotIndex + 1].gameObject.SetActive(false);
        }
        if (!crosshair.outline)
        {
            for (int i = 0; i < images.Length / 2; i++)
            {
                images[i].gameObject.SetActive(false);
            }
        }
    }

    private void UpdateRadius()
    {
        for (int i = 0; i < images.Length; i++)
        {
            var dirIndex = i % (images.Length / 2);
            var odd = dirIndex & 1;
            var thickness = (crosshair.horizontalThickness * odd) + (crosshair.verticalThickness * (odd - 1) * -1);
            var length = crosshair.outerRadius * 2;
            var dotSize = crosshair.dotSize;
            images[i].localPosition = directions[dirIndex] * crosshair.innerRadius;
            if (i < images.Length / 2)
            {
                images[i].localPosition = directions[dirIndex] * (crosshair.innerRadius - crosshair.outlineThickness * 0.5f);
                thickness += crosshair.outlineThickness;
                length += crosshair.outlineThickness;
                dotSize += crosshair.outlineThickness;
            }

            images[i].sizeDelta = new Vector2(thickness, length);

            if (dirIndex == (images.Length / 2 - 1))
            {
                images[i].sizeDelta = new Vector2(dotSize, dotSize);
            }

        }
    }

}
