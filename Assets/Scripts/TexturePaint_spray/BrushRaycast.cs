using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class BrushRaycast : MonoBehaviour
{
    public TexturePaintSetting texturePaintSetting;

    public Transform brush_ray;
    public Transform ears, head, body, body1, tail;
    public int resolution = 1024;

    public SkinnedTexturePaint ears_texturePaint, head_texturePaint, body_texturePaint, body1_texturePaint, tail_texturePaint;
    public InputActionProperty right_Trigger_Action, left_Trigger_Action;
    public InputActionProperty right_Grip_Action, left_Grip_Action;

    public bool right_grab = false, left_grab = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(brush_ray.position, brush_ray.forward);
        if(texturePaintSetting.on_lock == true)
        {
            if (right_grab)
            {
                if (right_Trigger_Action.action.ReadValue<float>() > 0)
                {
                    Debug.Log("버튼 입력");
                    paint_texture();
                }
            }

            if (left_grab)
            {
                if (left_Trigger_Action.action.ReadValue<float>() > 0)
                {
                    Debug.Log("버튼 입력");
                    paint_texture();
                }
            }
        }


        if (Input.GetKeyDown(KeyCode.Space))
        {
            paint_texture();
        }
    }

    public void paint_texture()
    {
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Ray ray = new Ray(brush_ray.position, brush_ray.forward);

        bool raycast = Physics.Raycast(ray, out var hit, 10);
        Collider col = hit.collider;

        if (raycast && col)
        {
            Debug.Log(col.name);
            if (col.transform == ears)
            {
                ears_texturePaint.DrawTexture(hit);
                Debug.Log("ears hit");
            }
            if (col.transform == head)
            {
                head_texturePaint.DrawTexture(hit);
                Debug.Log("head hit");
            }
            if (col.transform == body)
            {
                body_texturePaint.DrawTexture(hit);
                Debug.Log("body hit");
            }
            if (col.transform == body1)
            {
                body1_texturePaint.DrawTexture(hit);
                Debug.Log("body 1 hit");
            }
            if (col.transform == tail)
            {
                tail_texturePaint.DrawTexture(hit);
                Debug.Log("tail hit");
            }
        }
    }

    public void grab_right()
    {
        if (right_Grip_Action.action.ReadValue<float>() > 0)
        {
            Debug.Log("grip pressed");
            right_grab = true;
        }
    }

    public void release_right()
    {
        if (right_Grip_Action.action.ReadValue<float>() == 0)
        {
            Debug.Log("grip pressed");
            right_grab = false;
        }
    }

    public void grab_left()
    {
        if (left_Grip_Action.action.ReadValue<float>() > 0)
        {
            Debug.Log("grip pressed");
            left_grab = true;
        }
    }

    public void release_left()
    {
        if (left_Grip_Action.action.ReadValue<float>() == 0)
        {
            Debug.Log("grip pressed");
            left_grab = false;
        }
    }
}
