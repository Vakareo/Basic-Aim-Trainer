
[System.Serializable]
public struct CrosshairData
{
    public CrosshairColor color;
    public CrosshairColor outlineColor;
    public float innerRadius;
    public float outerRadius;
    public float verticalThickness;
    public float horizontalThickness;
    public bool centerDot;
    public float dotSize;
    public bool outline;
    public float outlineThickness;
}

[System.Serializable]
public struct CrosshairColor
{
    public byte red;
    public byte green;
    public byte blue;
    public byte alpha;
}

