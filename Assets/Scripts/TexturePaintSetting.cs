using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TexturePaintSetting : MonoBehaviour
{
    public SkinnedTexturePaint skinnedTexturePaint_head_ears,skinnedTexturePaint_head, skinnedTexturePaint_body, skinnedTexturePaint_body1, skinnedTexturePaint_tail;
    
    [Range(0.01f, 1f)]
    public float brush_size = 0.1f;
    public Texture2D brush_texture_red, brush_texture_blue, brush_texture_green, brush_texture_yellow;
    public bool on_lock = false;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name + "sssss");

        if(on_lock == false)
        {
            if (other.name.Contains("Red"))
            {
                Debug.Log("red");
                brush_change(brush_texture_red);
                on_lock = true;
            }
            if (other.name.Contains("Blue"))
            {
                Debug.Log("blue");
                brush_change(brush_texture_blue);
                on_lock = true;
            }
            if (other.name.Contains("Green"))
            {
                Debug.Log("green");
                brush_change(brush_texture_green);
                on_lock = true;
            }
            if (other.name.Contains("Yellow"))
            {
                Debug.Log("yellow");
                brush_change(brush_texture_yellow);
                on_lock = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (on_lock == true)
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
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
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
    }

    public void brush_change(Texture2D brush_texture)
    {
        skinnedTexturePaint_head_ears.brushSize = brush_size;
        skinnedTexturePaint_head_ears.brushTexture = brush_texture;
        skinnedTexturePaint_head.brushSize = brush_size;
        skinnedTexturePaint_head.brushTexture = brush_texture;
        skinnedTexturePaint_body.brushSize = brush_size;
        skinnedTexturePaint_body.brushTexture = brush_texture;
        skinnedTexturePaint_body1.brushSize = brush_size;
        skinnedTexturePaint_body1.brushTexture = brush_texture;
        skinnedTexturePaint_tail.brushSize = brush_size;
        skinnedTexturePaint_tail.brushTexture = brush_texture;
    }
}
