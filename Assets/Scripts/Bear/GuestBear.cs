using System;
using System.Collections;
using System.Collections.Generic;
using EnumTypes;
using StructsType;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class GuestBear : GlobalBears
    {
        [Header("Field of view")] 
        public float viewRadius; 
        [Range(0, 360)] public float viewAngle;
        public LayerMask targetMask;
        public bool canSeeTarget;
        
        [Header("Game Set")] 
        public GameObject pairPlayer;
        public GameObject AnswerBear;

        // 해당 Bear에게 꾸며진 Decorate를 확인하고, 정보를 저장해둔다
        public List<DecoItemData> decoItemDataList = new List<DecoItemData>();

        private IEnumerator _FoVRoutine;
        
        private void Awake()
        {
            BearType = BearType.GuestBear;
            originParent = transform.Find("Armature");

        }
        
        private void Start()
        {
            // _FoVRoutine = FieldOfViewRoutine();
            // StartCoroutine(_FoVRoutine);
        }

        private IEnumerator FieldOfViewRoutine()
        {
            WaitForSeconds wait = new WaitForSeconds(0.05f);
            while (true)
            {
                yield return null;
                FieldOfViewCheck();
            }
        }
        
        private void FieldOfViewCheck()
        {
            Collider[] rangeChecks = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

            if (rangeChecks.Length != 0)
            {
                Transform pairTarget = rangeChecks[0].transform;
                Vector3 dirToTarget = (pairTarget.position - transform.position).normalized;

                if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
                {
                    float distanceToTarget = Vector3.Distance(transform.position, pairTarget.position);
                    
                    RaycastHit hit;
                    if (Physics.Raycast(transform.position, dirToTarget, out hit, distanceToTarget))
                    {
                        canSeeTarget = true;
                        pairPlayer = pairTarget.root.gameObject;
                        Debug.Log(this.name + "의 Pair : " + pairPlayer.name);
                        StopCoroutine(_FoVRoutine);
                    }
                }
            }
        }

        /// <summary>
        /// 해당 젤리곰을 꾸미는 Player의 타입을 확인하고 pairPlayer 변수에 저장합니다
        /// 이후 Bear Manager에서 Pair로 지정됩니다
        /// </summary>
        public void CheckPairPlayer()
        {
            canSeeTarget = false;
            pairPlayer = null;

            _FoVRoutine = FieldOfViewRoutine();
            StartCoroutine(_FoVRoutine);
        }

        public GameObject GetPairPlayer()
        {
            if (canSeeTarget)
                return pairPlayer;
            else
            {
                Debug.Log("에러(LogError) : Player를 꾸밀 곰 앞으로 세팅해주세요");
                return null;
            }
        }
            
        /// <summary>
        /// Deco Item의 Dictionary를 생성함
        /// [0814] 해당 함수 GuestBear로 옮겨졌습니다
        /// </summary>
        public void SetDecorationList()
        {
            Transform[] childrens = originParent.GetComponentsInChildren<Transform>();

            foreach (Transform child in childrens)
            {
                if (child.CompareTag("Deco"))
                {
                    // 각 Deco는 Bear의 Armature Hierarchy 하위에 존재
                    //Debug.Log("Find Answer Decoration : "+ child.name);
                
                    // TODO: Deco Item으로부터 Type을 알아야함 따라서 각 프리팹에 타입을 정의해야함 (임시로 해둠)
                    DecoItemData item = new DecoItemData(child.gameObject, DecorateType.CutAndShape);
                
                    decoItemDataList.Add(item);
                    decoItemNum++;
                    
                    child.gameObject.SetActive(false);
                }
            }
        }
        
        // TODO: 기능 1, 2 구현
        
        
        /// <summary>
        /// 기능 3
        /// 정답곰에게 붙어있는 deco item 중, CutAndShape 관련 아이템을 적용합니다
        /// </summary>
        
        /*public void CutAndShape_before()
        {
            foreach (var item in AnswerBear.GetComponent<AnswerBear>().GetDecoItemList())
            {
                if (item.ItemType == DecorateType.CutAndShape)
                {
                    GameObject copyItem = Instantiate(item.ItemObject);
                    
                    Transform[] childrens = originParent.GetComponentsInChildren<Transform>();
                    foreach (Transform child in childrens)
                    {
                        if (child.name == item.Parent.name)
                        {
                            copyItem.transform.SetParent(child);
                            copyItem.transform.localPosition = item.LocalPosition;
                            copyItem.transform.localRotation = item.LocalRotation;
                            copyItem.SetActive(true);
                        }
                    }
                }
            }
        }*/

        public void CutAndShape()
        {
            foreach (var item in decoItemDataList)
            {
                if (item.ItemType == DecorateType.CutAndShape)
                {
                    item.ItemObject.SetActive(true);
                }
            }
        }

        /// <summary>
        /// 기능 4
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
