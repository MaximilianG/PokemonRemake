using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor.Sprites;
using UnityEngine;


[RequireComponent(typeof(SpriteRenderer))]
public class ColorRandomizer : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    public void ChangeRandomColor()
    {
        if (TryGetComponent<SpriteRenderer>(out spriteRenderer))
        {
            spriteRenderer.color = new UnityEngine.Color(Random.Range(0F, 1F), Random.Range(0, 1F), Random.Range(0, 1F));
        }
    }
}
