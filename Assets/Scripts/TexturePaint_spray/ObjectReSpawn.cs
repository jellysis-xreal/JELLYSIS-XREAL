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
        rb.velocity = Vector3.zero; // 속력 초기화
        rb.angularVelocity = Vector3.zero; // 각속도 초기화

        yield return new WaitForSeconds(2f); // 2초 동안 대기

        //float elapsedTime = 0f;
        //float moveDuration = 1f; // 이동에 걸릴 시간
        Vector3 startPos = transform.position;

/*        while (elapsedTime < moveDuration)
        {
            transform.position = Vector3.Lerp(startPos, originalPosition, elapsedTime / moveDuration);
            transform.rotation = Quaternion.Lerp(originalRotation, originalRotation, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }*/

        transform.position = originalPosition; // 정확한 위치로 보정
        rb.velocity = Vector3.zero; // 속력 초기화
        rb.angularVelocity = Vector3.zero; // 각속도 초기화
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Ground"))
        {
            isFalling = true;
        }
    }
}
