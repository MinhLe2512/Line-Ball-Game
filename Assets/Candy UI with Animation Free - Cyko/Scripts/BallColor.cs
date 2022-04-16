using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallColor : MonoBehaviour
{
    public enum ColorType
    {
        RED,
        PURPLE,
        BLUE,
        ORANGE,
        GHOST_O,
        GHOST_R,
        GHOST_P,
        GHOST_B,
        COUNT
    }

    [System.Serializable]
    public struct ColorSprite
    {
        public ColorType color;
        public Sprite sprite;
    }

    private Dictionary<ColorType, Sprite> colorSpriteDict;
    [SerializeField] private ColorSprite[] colorSprites;
    private ColorType color;

    public ColorType Color
    {
        get { return color; }
        set
        {
            SetColor(value);
        }
    }

    private SpriteRenderer render;

    public int NumColors
    {
        get { return colorSprites.Length; }
    }
    public void SetColor(ColorType newColor)
    {
        color = newColor;
        
        if (colorSpriteDict.ContainsKey(newColor))
        {
            render.sprite = colorSpriteDict[newColor];
        }
    }
    void Awake()
    {
        render = GetComponent<SpriteRenderer>();

        colorSpriteDict = new Dictionary<ColorType, Sprite>();
        
        for (int i = 0; i < colorSprites.Length; i++)
        {
            if (!colorSpriteDict.ContainsKey(colorSprites[i].color))
                colorSpriteDict.Add(colorSprites[i].color, colorSprites[i].sprite);
        }
    }

}
