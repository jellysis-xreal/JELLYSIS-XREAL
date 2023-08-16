using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class BrushRaycast : MonoBehaviour
{
    public TexturePaintSetting texturePaintSetting;
    public XRBaseController leftController, rightController;

    public Transform brush_ray;

    public int resolution = 1024;

    
    public InputActionProperty right_Trigger_Action, left_Trigger_Action;
    public InputActionProperty right_Grip_Action, left_Grip_Action;

    public bool right_grab = false, left_grab = false;
    public bool past_right_grab = false, past_left_grab = false;
    public Animator animator;

    public Transform ears, head, body, body1, tail;
    public SkinnedTexturePaint ears_texturePaint, head_texturePaint, body_texturePaint, body1_texturePaint, tail_texturePaint;

    public AllBearTextureCode allBearTextureCode;


    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            animator.Play("pressAni");
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            animator.Play("pullAni");
        }
        if (right_Grip_Action.action.ReadValue<float>() > 0)
        {
            Debug.Log("grip pressed");
            past_right_grab = true;
        }
        else
        {
            past_right_grab = false;
        }

        if (left_Grip_Action.action.ReadValue<float>() > 0)
        {
            Debug.Log("grip pressed");
            past_left_grab = true;
        }
        else
        {
            past_left_grab = false;
        }


        //Debug.DrawRay(brush_ray.position, brush_ray.forward);
        if (texturePaintSetting.on_lock == true)
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
            if (right_grab)
            {
                rightController.SendHapticImpulse(0.2f, 0.1f);
            }
            if (left_grab)
            {
                leftController.SendHapticImpulse(0.2f, 0.1f);
            }

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



/*            for(int i=0 ; i<4; i++)
            {
                if (col.transform == allBearTextureCode.ears_group[i])
                {
                    allBearTextureCode.ears_texturePaint_group[i].DrawTexture(hit);
                    //ears_texturePaint.DrawTexture(hit);
                    Debug.Log("ears hit");
                }
                if (col.transform == allBearTextureCode.head_group[i])
                {
                    allBearTextureCode.head_texturePaint_group[i].DrawTexture(hit);
                    //head_texturePaint.DrawTexture(hit);
                    Debug.Log("head hit");
                }
                if (col.transform == allBearTextureCode.body_group[i])
                {
                    allBearTextureCode.body_texturePaint_group[i].DrawTexture(hit);
                    //body_texturePaint.DrawTexture(hit);
                    Debug.Log("body hit");
                }
                if (col.transform == allBearTextureCode.body1_group[i])
                {
                    allBearTextureCode.body1_texturePaint_group[i].DrawTexture(hit);
                    //body1_texturePaint.DrawTexture(hit);
                    Debug.Log("body 1 hit");
                }
                if (col.transform == allBearTextureCode.tail_group[i])
                {
                    allBearTextureCode.tail_texturePaint_group[i].DrawTexture(hit);
                    //tail_texturePaint.DrawTexture(hit);
                    Debug.Log("tail hit");
                }
            }*/

        }
    }

    public void grab_right()
    {
        if (past_right_grab != true && right_Grip_Action.action.ReadValue<float>() > 0)
        {
            Debug.Log("grip pressed");
            right_grab = true;
        }
    }

    public void release_right()
    {
        if (past_right_grab == true && right_Grip_Action.action.ReadValue<float>() == 0)
        {
            Debug.Log("grip pressed");
            right_grab = false;
        }
    }

    public void grab_left()
    {
        if (past_left_grab != true && left_Grip_Action.action.ReadValue<float>() > 0)
        {
            Debug.Log("grip pressed");
            left_grab = true;
        }
    }

    public void release_left()
    {
        if (past_left_grab == true && left_Grip_Action.action.ReadValue<float>() == 0)
        {
            Debug.Log("grip pressed");
            left_grab = false;
        }
    }

    public void press_on()
    {
        animator.Play("pressAni");
    }

    public void press_off()
    {
        animator.Play("pullAni");
    }
}
