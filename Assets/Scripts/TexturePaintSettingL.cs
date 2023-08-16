using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TexturePaintSettingL : MonoBehaviour
{
    public SkinnedTexturePaint skinnedTexturePaint_head_ears, skinnedTexturePaint_head, skinnedTexturePaint_body, skinnedTexturePaint_body1, skinnedTexturePaint_tail;
    [Range(0.01f, 1f)]
    public float brush_size = 0.08f;
    public Texture2D brush_texture_red, brush_texture_blue, brush_texture_green, brush_texture_yellow, brush_texture_white, brush_texture_brown;
    public bool on_lock = false;
    private Collider current_paint;

    public AllBearTextureCode allBearTextureCode;


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name + "sssss");

        if (on_lock == false)
        {
            if (other.name.Contains("Red"))
            {
                current_paint = other;
                Debug.Log("red");
                brush_change(brush_texture_red);
                on_lock = true;
            }
            if (other.name.Contains("Blue"))
            {
                current_paint = other;
                Debug.Log("blue");
                brush_change(brush_texture_blue);
                on_lock = true;
            }
            if (other.name.Contains("Green"))
            {
                current_paint = other;
                Debug.Log("green");
                brush_change(brush_texture_green);
                on_lock = true;
            }
            if (other.name.Contains("Yellow"))
            {
                current_paint = other;
                Debug.Log("yellow");
                brush_change(brush_texture_yellow);
                on_lock = true;
            }
            if (other.name.Contains("White"))
            {
                current_paint = other;
                Debug.Log("white");
                brush_change(brush_texture_white);
                on_lock = true;
            }
            if (other.name.Contains("Brown"))
            {
                current_paint = other;
                Debug.Log("brown");
                brush_change(brush_texture_brown);
                on_lock = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (on_lock == true && current_paint == other)
        {
            if (other.name.Contains("Red"))
            {
                Debug.Log("red");
                on_lock = false;
            }
            if (other.name.Contains("Blue"))
            {
                Debug.Log("blue");
                on_lock = false;
            }
            if (other.name.Contains("Green"))
            {
                Debug.Log("green");
                on_lock = false;
            }
            if (other.name.Contains("Yellow"))
            {
                Debug.Log("yellow");
                on_lock = false;
            }
            if (other.name.Contains("White"))
            {
                Debug.Log("white");
                on_lock = false;
            }
            if (other.name.Contains("Brown"))
            {
                Debug.Log("Brown");
                on_lock = false;
            }
        }
    }

    // Update is called once per frame
/*    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("브러시 체인지 1");
            brush_change(brush_texture_red);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log("브러시 체인지 2");
            brush_change(brush_texture_blue);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Debug.Log("브러시 체인지 3");
            brush_change(brush_texture_green);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Debug.Log("브러시 체인지 4");
            brush_change(brush_texture_yellow);
        }
    }*/

    public void brush_change(Texture2D brush_texture)
    {
        allBearTextureCode.all_brush_change_L(brush_texture, brush_size);

/*        skinnedTexturePaint_head_ears.brushSize_L = brush_size * 3.0f;
        skinnedTexturePaint_head_ears.brushTexture_L = brush_texture;
        skinnedTexturePaint_head.brushSize_L = brush_size * 0.8f;
        skinnedTexturePaint_head.brushTexture_L = brush_texture;
        skinnedTexturePaint_body.brushSize_L = brush_size * 0.6f;
        skinnedTexturePaint_body.brushTexture_L = brush_texture;
        skinnedTexturePaint_body1.brushSize_L = brush_size * 3.0f;
        skinnedTexturePaint_body1.brushTexture_L = brush_texture;
        skinnedTexturePaint_tail.brushSize_L = brush_size * 2.0f;
        skinnedTexturePaint_tail.brushTexture_L = brush_texture;*/
    }
}
