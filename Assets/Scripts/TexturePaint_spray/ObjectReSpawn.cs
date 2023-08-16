using System.Collections;
using UnityEngine;

public class ObjectReSpawn : MonoBehaviour
{
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    [SerializeField]
    private bool isFalling = false;
    private Rigidbody rb;
    private void Start()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (isFalling)
        {
            StartCoroutine(ReturnToOriginalPosition());
            isFalling = false;
        }
    }

    private IEnumerator ReturnToOriginalPosition()
    {
        rb.velocity = Vector3.zero; // �ӷ� �ʱ�ȭ
        rb.angularVelocity = Vector3.zero; // ���ӵ� �ʱ�ȭ

        yield return new WaitForSeconds(2f); // 2�� ���� ���

        //float elapsedTime = 0f;
        //float moveDuration = 1f; // �̵��� �ɸ� �ð�
        Vector3 startPos = transform.position;

/*        while (elapsedTime < moveDuration)
        {
            transform.position = Vector3.Lerp(startPos, originalPosition, elapsedTime / moveDuration);
            transform.rotation = Quaternion.Lerp(originalRotation, originalRotation, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }*/

        transform.position = originalPosition; // ��Ȯ�� ��ġ�� ����
        rb.velocity = Vector3.zero; // �ӷ� �ʱ�ȭ
        rb.angularVelocity = Vector3.zero; // ���ӵ� �ʱ�ȭ
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Ground"))
        {
            isFalling = true;
        }
    }
}
