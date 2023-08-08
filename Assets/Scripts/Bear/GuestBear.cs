using System;
using System.Collections;
using System.Collections.Generic;
using EnumTypes;
using UnityEngine;

    public class GuestBear : GlobalBears
    {
        public GameObject answerCard;

        private GameObject AnswerBear;
        private void Awake()
        {
            BearType = BearType.GuestBear;
        }
        
        private void Start()
        {
            
        }

        /// <summary>
        /// 젤리곰의 Base Color를 변경합니다
        /// 이때 이미 지정한 BaseMaterials의 clone을 만들고, Texture Array만 변경하여 해당 색으로 적용합니다
        /// </summary>
        public void ChangeBaseColor(Texture2DArray texture2DArray)
        {
            transform.GetChild(1).GetComponent<Renderer>().material = GetNewMaterial(0, texture2DArray);    // body_1
            transform.GetChild(2).GetComponent<Renderer>().material = GetNewMaterial(1, texture2DArray);    // body
            transform.GetChild(3).GetComponent<Renderer>().material = GetNewMaterial(2, texture2DArray);    // bear_ears_1
            transform.GetChild(4).GetComponent<Renderer>().material = GetNewMaterial(3, texture2DArray);    // bear_ears
            Material[] mat = transform.GetChild(5).GetComponent<Renderer>().materials;
            mat[0] = GetNewMaterial(4, texture2DArray);                                                     // bear_eyes_1
            mat[1] = GetNewMaterial(5, texture2DArray);
            transform.GetChild(5).GetComponent<Renderer>().materials = mat;
            transform.GetChild(6).GetComponent<Renderer>().material = GetNewMaterial(6, texture2DArray);    // bear_eyes_top
            transform.GetChild(7).GetComponent<Renderer>().material = GetNewMaterial(7, texture2DArray);    // bear_head
            transform.GetChild(8).GetComponent<Renderer>().material = GetNewMaterial(8, texture2DArray);    // bear_mouth_low
            transform.GetChild(9).GetComponent<Renderer>().material = GetNewMaterial(9, texture2DArray);    // nose
            transform.GetChild(10).GetComponent<Renderer>().material = GetNewMaterial(10, texture2DArray);  // tail
        }

        private Material GetNewMaterial(int IndexMaterial, Texture2DArray texture2DArray)
        {
            Material newMaterial = new Material(GameManager.Bear.BaseMaterials[IndexMaterial]);
            newMaterial.SetTexture("_Texture2D_Array", texture2DArray);

            return newMaterial;
        }
    }
