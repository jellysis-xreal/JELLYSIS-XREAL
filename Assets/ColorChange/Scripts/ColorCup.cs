using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorCup : MonoBehaviour
{
    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Cup")) // ���� ���� ������Ʈ�� �±װ� "Cup"�̶��
        {
            Debug.Log("��"); // "��"�� ����׷� ���
        }
    }
}
