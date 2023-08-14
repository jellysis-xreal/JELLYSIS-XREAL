using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class BrushRaycast_L : MonoBehaviour
{
    public TexturePaintSettingL texturePaintSetting;
    public XRBaseController leftController, rightController;

    public Transform brush_ray;
    public Transform ears, head, body, body1, tail;
    public int resolution = 1024;

    public SkinnedTexturePaint ears_texturePaint, head_texturePaint, body_texturePaint, body1_texturePaint, tail_texturePaint;

    public InputActionProperty right_Trigger_Action, left_Trigger_Action;
    public InputActionProperty right_Grip_Action, left_Grip_Action;

    public bool right_grab_L = false, left_grab_L = false;
    public bool past_right_grab_L = false, past_left_grab_L = false;


    [SerializeField] private Transform tableTransform;

    public Animator animator;


    public SkinnedTexturePaint new_body_texturePaint;
    public Transform new_body;

    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            //_tableEventManager.RaiseEvent();
            //tableTransform.DORotate(tableTransform.rotation.eulerAngles + Quaternion.AngleAxis(15f, Vector3.forward).eulerAngles,
            //2f, RotateMode.LocalAxisAdd);
            animator.Play("pressAni");
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            //_tableEventManager.RaiseEvent();
            //tableTransform.DORotate(tableTransform.rotation.eulerAngles + Quaternion.AngleAxis(15f, Vector3.back).eulerAngles,
            //2f, RotateMode.LocalAxisAdd);
            animator.Play("pullAni");
        }

        if (right_Grip_Action.action.ReadValue<float>() > 0)
        {
            Debug.Log("grip pressed");
            past_right_grab_L = true;
        } else
        {
            past_right_grab_L = false;
        }

        if (left_Grip_Action.action.ReadValue<float>() > 0)
        {
            Debug.Log("grip pressed");
            past_left_grab_L = true;
        }
        else
        {
            past_left_grab_L = false;
        }



        //Debug.DrawRay(brush_ray.position, brush_ray.forward);
        if (texturePaintSetting.on_lock == true)
        {
            if (right_grab_L)
            {
                if (right_Trigger_Action.action.ReadValue<float>() > 0)
                {
                    Debug.Log("버튼 입력");
                    paint_texture_L();
                }
            }

            if (left_grab_L)
            {
                if (left_Trigger_Action.action.ReadValue<float>() > 0)
                {
                    Debug.Log("버튼 입력");
                    paint_texture_L();
                }
            }
        }
    }


    public void paint_texture_L()
    {
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Ray ray = new Ray(brush_ray.position, brush_ray.forward);

        bool raycast = Physics.Raycast(ray, out var hit, 10);
        Collider col = hit.collider;

        if (raycast && col)
        {
            if (right_grab_L)
            {
                rightController.SendHapticImpulse(0.2f, 0.1f);
            }
            if (left_grab_L)
            {
                leftController.SendHapticImpulse(0.2f, 0.1f);
            }

            Debug.Log(col.name);
            if (col.transform == ears)
            {
                ears_texturePaint.DrawTexture_L(hit);
                Debug.Log("ears hit");
            }
            if (col.transform == head)
            {
                head_texturePaint.DrawTexture_L(hit);
                Debug.Log("head hit");
            }
            if (col.transform == body)
            {
                body_texturePaint.DrawTexture_L(hit);
                Debug.Log("body hit");
            }
            if (col.transform == body1)
            {
                body1_texturePaint.DrawTexture_L(hit);
                Debug.Log("body 1 hit");
            }
            if (col.transform == tail)
            {
                tail_texturePaint.DrawTexture_L(hit);
                Debug.Log("tail hit");
            }
            if (col.transform == new_body)
            {
                new_body_texturePaint.DrawTexture_L(hit);
                Debug.Log("tail hit");
            }
        }
    }


    public void grab_right()
    {
        if (past_right_grab_L != true && right_Grip_Action.action.ReadValue<float>() > 0)
        {
            Debug.Log("grip pressed");
            right_grab_L = true;
        }
    }

    public void release_right()
    {
        if (past_right_grab_L == true && right_Grip_Action.action.ReadValue<float>() == 0)
        {
            Debug.Log("grip pressed");
            right_grab_L = false;
        }
    }

    public void grab_left()
    {
        if (past_left_grab_L != true && left_Grip_Action.action.ReadValue<float>() > 0)
        {
            Debug.Log("grip pressed");
            left_grab_L = true;
            tableTransform.DORotate(tableTransform.rotation.eulerAngles + Quaternion.AngleAxis(15f, Vector3.back).eulerAngles,
1f, RotateMode.Fast);
        }
    }

    public void release_left()
    {
        if (past_left_grab_L == true && left_Grip_Action.action.ReadValue<float>() == 0)
        {
            Debug.Log("grip pressed");
            left_grab_L = false;
            tableTransform.DORotate(tableTransform.rotation.eulerAngles + Quaternion.AngleAxis(-15f, Vector3.back).eulerAngles,
1f, RotateMode.Fast);
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
