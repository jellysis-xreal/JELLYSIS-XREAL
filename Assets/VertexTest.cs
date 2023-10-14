using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class VertexTest : MonoBehaviour
{
    Mesh mesh;
    Vector3[] vertices;
    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        vertices = mesh.vertices;
        
        Dict();
    }

    void Dict()
    {
        // Dictionary 초기화
        Dictionary<Vector3, float> vectorDictionary = new Dictionary<Vector3, float>();
        vectorDictionary[new Vector3(1, 2, 3)] = 10.5f;
        vectorDictionary[new Vector3(4, 5, 6)] = 20.3f;
        vectorDictionary[new Vector3(7, 8, 9)] = 30.2f;
        vectorDictionary[new Vector3(10, 11, 12)] = 25.2f;
        vectorDictionary[new Vector3(13, 14, 15)] = 5.2f;

        // 인덱스를 부여하고 내림차순으로 정렬
        var indexedAndSortedDictionary = vectorDictionary.Select((pair, index) => new { Key = pair.Key, Value = pair.Value, Index = index })
            .OrderByDescending(item => item.Value)
            .ToList();

        // 결과 출력
        Debug.Log("정렬된 Dictionary:");
        foreach (var item in indexedAndSortedDictionary)
        {
            Debug.Log($"Key: {item.Key}, Value: {item.Value}, Original Index: {item.Index}");
        }
    }
    void Update()
    {
        for (var i = 0; i < vertices.Length-2; i++)
        {
            vertices[i] += Vector3.up * Time.deltaTime;
        }

        // assign the local vertices array into the vertices array of the Mesh.
        mesh.vertices = vertices;
        mesh.RecalculateBounds();
    }
}
