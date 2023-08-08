using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearColorPainter : MonoBehaviour
{
    [SerializeField] private int minimumDipPaintCount;
    [SerializeField] private int dipPaintCount;
    [SerializeField] private GameObject preGameObjectPaint;
    private MeshRenderer brushMeshRenderer;
    uint paintId;

    public AudioSource put_paint, use_paint;
    

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.tag == "Paint")
        {
            //put_paint.Play();
            // ����, 111, ��ũ ,112, ��Ʈ, 113, ��Ȳ 114
            //paintId = other.GetComponent<GlobalObjects>().GUID;
            if (preGameObjectPaint == null || preGameObjectPaint != other.gameObject)
            {
                // 처음 페인트와 트리거, 이전 트리거된 페인트와 다르면 count 초기화
                dipPaintCount = minimumDipPaintCount;
            }
            preGameObjectPaint = other.gameObject;
            
            dipPaintCount -= 1;
            Debug.Log(preGameObjectPaint.name +" :" + dipPaintCount);
            if (dipPaintCount == 0)
            {
                brushMeshRenderer = GetComponent<MeshRenderer>();
                brushMeshRenderer.material = other.gameObject.GetComponent<MeshRenderer>().materials[1];                
            }
        }

        if (other.gameObject.layer == 11) // 11 : Basic Bear
        {
            //use_paint.Play();
            switch (paintId)
            {

                /*case 111:
                    other.transform.root.GetComponent<GuestBear>().colorType = GameManager.BearColorType.Choco;
                    break;
                case 112:
                    other.transform.root.GetComponent<GuestBear>().colorType = GameManager.BearColorType.Strawberry;
                    break;
                case 113:
                    other.transform.root.GetComponent<GuestBear>().colorType = GameManager.BearColorType.Mint;
                    break;
                case 114:
                    other.transform.root.GetComponent<GuestBear>().colorType = GameManager.BearColorType.Orange;
                    break;
                default:
                    break;*/
            }

            //Debug.Log(other.transform.root.name);
            SkinnedMeshRenderer bearMeshRenderer =
                other.transform.root.GetComponentInChildren<SkinnedMeshRenderer>();
            //Debug.Log("bearMeshRenderer is "+bearMeshRenderer);
            bearMeshRenderer.material = brushMeshRenderer.material;



        }
    }
}
