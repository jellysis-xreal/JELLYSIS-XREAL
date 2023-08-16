using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorCup : MonoBehaviour
{
    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Cup")) // 만약 닿은 오브젝트의 태그가 "Cup"이라면
        {
            Debug.Log("컵"); // "컵"을 디버그로 출력
        }
    }
}
