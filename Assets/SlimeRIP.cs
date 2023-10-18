using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SlimeRIP : MonoBehaviour
{
    [SerializeField] public Vector3 location;
    [SerializeField] private Vector3 closestPoint;
    [SerializeField] private Collider triggerCollider;
    [SerializeField] private Transform triggerTransform;
    [SerializeField] private MeshFilter triggerMeshFilter;
    
    [Space]
    [SerializeField] private Vector3[] vertexTransforms; // 움직일 버텍스 위치 조작
    [SerializeField] private List<Vector3> grabbedVertexTransforms; // 잡은 메시 Vector3
    [SerializeField] private List<int> grabbedVertexIndexes; // 감지된 버텍스의 메시 내 인덱스
    [SerializeField] private float grabbedDistance;
    [SerializeField] private bool isGrabbing = false;
    [SerializeField] private float[] lengthModifier;

    [Space]
    [SerializeField] private Dictionary<Vector3, float> vertexDistance; // <vertex Transform, distance between vertex and grabbed Object> 

    private void Start()
    {
        vertexDistance = new Dictionary<Vector3, float>();
    }
    private void Update()
    {
        if (Vector3.Distance(location, closestPoint) >= 0)
        {
            //Debug.Log( "Distance : "+ Vector3.Distance(location, closestPoint));    
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            FindClosestVertexes();
            CalculateVertex();
            // ChasePlayerHand();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            grabbedVertexTransforms.Clear();
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            isGrabbing = !isGrabbing;
        }

        if (isGrabbing)
        {
            ChasePlayerHand();
        }
    }
    // 가장 가까운 'vertex 하나'를 찾는 함수 (Trigger에서 얻은 mesh에 접근해서 가져오는 방식, 최초 충돌 위치, 가장 가까운 위치 하나 만을 가져옴.
    private Vector3 FindClosestOneVertex()
    {
        Vector3 closestVertex = Vector3.zero;
        float closestDistance = Mathf.Infinity;

        if (triggerMeshFilter != null)
        {
            Mesh mesh = triggerMeshFilter.sharedMesh;
            
            // 모든 vertex 반복
            foreach (Vector3 vertex in mesh.vertices)
            {
                // 실제 월드 좌표로 변환
                Vector3 worldVertex = triggerTransform.TransformPoint(vertex);
                
                // 충돌 지점과의 거리 계산
                float distance = Vector3.Distance(closestPoint, worldVertex);
                
                // 현재까지 찾은 가장 가까운 버텍스보다 더 가까우면 업데이트
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestVertex = worldVertex;
                }
                
                // 일정 거리 내에 존재하는 vertex 찾으면 for문 종료시키기
            }
        }
        return closestVertex;
    }

    // 가장 가까운 'vertex 여러 개'를 찾는 함수
    private void FindClosestVertexes()
    {
        Vector3 closestVertex = Vector3.zero;
        float closestDistance = Mathf.Infinity;

        if (triggerMeshFilter != null)
        {
            // 모든 vertex 반복
            for (int i = 0; i < vertexTransforms.Length - 1; i++)
            {
                // 실제 월드 좌표로 변환
                Vector3 worldVertex = triggerTransform.TransformPoint(vertexTransforms[i]);
                
                // 충돌 지점과의 거리 계산
                float distance = Vector3.Distance(closestPoint, worldVertex);
                
                // 현재까지 찾은 가장 가까운 버텍스보다 더 가까우면 업데이트
                if (distance < grabbedDistance)
                {
                    grabbedVertexTransforms.Add(worldVertex);
                    vertexDistance[worldVertex] = 0;
                    
                    // 해당 버텍스의 인덱스 얻기
                    int vertexIndex =
                        System.Array.IndexOf(vertexTransforms,
                            vertexTransforms[i]);
                    grabbedVertexIndexes.Add(vertexIndex);
                    Debug.Log("vertexIndex : "+vertexIndex);
                }
                // 일정 거리 내에 존재하는 vertex 찾으면 for문 종료시키기
            }
        }
    }

    private void CalculateVertex()
    {
        // 잡은 오브젝트와 grabbedVertexTransforms[i] 중 처음 트리거 된 포인트와 가까운 버텍스를 가중치 최대인 1, 멀리 있는 걸 0으로(이건 테스트해보고 값 조절)

        // Dictionary<Vector3, float> => Object의 중심으로 부터 각 vertex간 길이에 따라 정렬
        SortDistanceOrder();

        // 가장 큰 Distancee를 1 가장 짧은 Distance를 0으로 두어 가중치 계산해서 곱해줌.
        lengthModifier = new float[vertexDistance.Count];
        lengthModifier = CalculateLengthModifier();  //  => lengthModifier이 대부분 50에 수렴함. 계산 실수인가..?
    }

    private void SortDistanceOrder()
    {
        Debug.Log("Sort Distance Order");
        // 잡은 오브젝트의 원점 위치
        Vector3 grabbedObjectTransform = triggerCollider.transform.position;
        
        // Dictionary의 Key 컬렉션 가져오기
        ICollection<Vector3> keys = vertexDistance.Keys;

        Debug.Log("1 : vertexDistance Num "+keys.Count);
        // 컬렉션(예: 리스트, 사전, 배열 등)을 반복하는 동안 컬렉션의 내용을 수정하면 에러 => 임시 컬렉션 사용
        var copyKeys = keys.ToList(); 
       
        foreach (Vector3 key in copyKeys)
        {
            // 여기서 vertexDistance에 key따른 Value 할당, 추가로 인덱스도 할당 
            vertexDistance[key] = Vector3.Distance(grabbedObjectTransform, key);
        }

        // 인덱스를 부여하고 내림차순으로 정렬
        var indexedAndSortedDictionary = vertexDistance.Select((pair, index) 
                => new { Key = pair.Key, Value = pair.Value, Index = grabbedVertexIndexes[index] })
            .OrderByDescending(item => item.Value)
            .ToList();
        
        vertexDistance = vertexDistance.OrderByDescending(pair => pair.Value).
            ToDictionary(x => x.Key, x => x.Value);
        
        grabbedVertexIndexes.Clear();
        // 결과 출력
        Debug.Log("정렬된 Dictionary:");
        foreach (var item in indexedAndSortedDictionary)
        {
            Debug.Log($"Key: {item.Key}, Value: {item.Value}, Original Index: {item.Index}");
            grabbedVertexIndexes.Add(item.Index);
        }
    }
    private float[] CalculateLengthModifier()
    {
        // vertexDistance<Key, Value> 중 Value 가장 큰 값을 가중치 1, 가장 작은 값을 가중치 0으로 둠.
        float maxDistance = vertexDistance.First().Value;
        float minDistance = vertexDistance.Last().Value;

        float num = 1f / (maxDistance + minDistance);
        
        float[] modifier = new float[vertexDistance.Count]; 
        
        // Dictionary의 Key 컬렉션 가져오기
        ICollection<float> values = vertexDistance.Values;

        int i = 0;
        foreach (var value in values)
        {
            modifier[i] = num * value;
            //Debug.Log("modifier : " + modifier[i]);
            i += 1;
        }
            
        return modifier;
    }

    private void ChasePlayerHand()
    {
        // CalculateLengthModifier()에서 구한 가중치를 적용해 실제로 Player의 위치를 추적한다.
        // 변경된 지점의 mesh의 vertex에 접근해서 움직인다.
        
        for (int i = 0; i < vertexTransforms.Length-1; i++)
        {
            Vector3 direction = gameObject.transform.position - triggerTransform.TransformPoint(vertexTransforms[grabbedVertexIndexes[i]]);
            //Vector3 direction = gameObject.transform.position - vertexTransforms[grabbedVertexIndexes[i]];
                
            vertexTransforms[grabbedVertexIndexes[i]] += direction * lengthModifier[i] * Time.deltaTime;
            triggerMeshFilter.mesh.vertices = vertexTransforms;
            triggerMeshFilter.mesh.RecalculateBounds();
        }
        // Debug.Log("Chase Player Hand, vertexTransforms.Length :" + vertexTransforms.Length);
    }
    private void OnTriggerEnter(Collider other)
    {
        triggerCollider = other.GetComponent<Collider>();
        triggerMeshFilter = other.GetComponent<MeshFilter>();
        triggerTransform = other.GetComponent<Transform>();
        vertexTransforms = triggerMeshFilter.mesh.vertices;
        
        Debug.Log(vertexTransforms.Length);
    }
    private void OnTriggerStay(Collider other)
    {
        location = other.transform.position;
        closestPoint = other.ClosestPoint(transform.position);
        
        //Debug.Log( "충돌 지점과 가장 가까운 지점의 좌표 : "+FindClosestVertex());
    }
    private void OnTriggerExit(Collider other)
    {
        triggerCollider = null;
        //triggerMeshFilter = null;
        //triggerTransform = null;
        //vertexTransforms = null;
    }
    public void OnDrawGizmos()
    {

        if (!triggerCollider)
        {
            return; // nothing to do without a collider
        }
        
        Gizmos.DrawSphere(location, 0.1f);
        Gizmos.DrawWireSphere(closestPoint, 0.2f);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(closestPoint, 0.05f);
        foreach (Vector3 vertexTransform in vertexTransforms)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(vertexTransform, 0.01f);
        }
        foreach (Vector3 grabbedVertexTransform in grabbedVertexTransforms)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(grabbedVertexTransform, 0.01f);
        }
        foreach (Vector3 grabbedVertexTransform in grabbedVertexTransforms)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawLine(grabbedVertexTransform, transform.position);
        }

        for (int i = 0; i < vertexTransforms.Length - 1; i++)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(gameObject.transform.position, triggerTransform.TransformPoint(vertexTransforms[grabbedVertexIndexes[i]]));
        }
    }


}
/*
 *1. 오브젝트의 메시에서 충돌 지점과 가장 가까운 버텍스를 찾기 (손 근처의 구형 콜라이더와 충돌한 지점 collision.contacts[index].point와 가장 가까운 버텍스 찾기)

버텍스 찾는 과정에서 알고리즘 고려
실린더 메시에서 충돌 지점과 가장 가까운 버텍스 찾기

1. 충돌 지점을 얻는다.
2. 실린더의 모든 버텍스를 반복하여 가장 가까운 버텍스를 찾는다.


 *2. 찾은 버텍스를 직접 건드려 조정함
 
1. Grip 버튼을 누르기 전 구와 가장 먼저 충돌한 지점과 가까운 버텍스 하나 찾기
2. Grip 버튼을 누르는 순간 처음 충돌한 지점을 제외한 나머지 충돌 지점들과 그 지점과 가까운 버텍스 찾기
    1. 잘렸을 때 메시를 처리하기 위해 1번과 2번에 해당하는 버텍스 값을 기억함.
3. Grip 버튼을 누르는 동안 손의 위치를 추적해 해당하는 버텍스들의 위치를 Hand를 추적하도록 함.
    1. 처음 Grip한 순간 우선 손에서 가까운 순서를 만듬.
    2. 가까운 버텍스의 경우 더 많은 가중치를 두어 Hand를 추적
    3. 먼 버텍스의 경우 더 적은 가중치를 두어 Hand를 추적
    
    
 *3. 오브젝트와 버텍스의 거리가 일정 값 이상 올라가면 끊기

1. 2번에서 기억한 버텍스 값을 넣어 기존 오브젝트의 메시를 변형
2. 잡아 뜯은 손에도 2번에서 기억한 값을 이용해 새로운 오브젝트 생성
 */
