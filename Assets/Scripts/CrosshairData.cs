
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

    public CrosshairData(CrosshairColor color, CrosshairColor outlineColor, float innerRadius, float outerRadius, float verticalThickness, float horizontalThickness, bool centerDot, float dotSize, bool outline, float outlineThickness)
    {
        this.color = color;
        this.outlineColor = outlineColor;
        this.innerRadius = innerRadius;
        this.outerRadius = outerRadius;
        this.verticalThickness = verticalThickness;
        this.horizontalThickness = horizontalThickness;
        this.centerDot = centerDot;
        this.dotSize = dotSize;
        this.outline = outline;
        this.outlineThickness = outlineThickness;
    }
}

[System.Serializable]
public struct CrosshairColor
{
    public byte red;
    public byte green;
    public byte blue;
    public byte alpha;

    public CrosshairColor(byte red, byte green, byte blue, byte alpha)
    {
        this.red = red;
        this.green = green;
        this.blue = blue;
        this.alpha = alpha;
    }
}

