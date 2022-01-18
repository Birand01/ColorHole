using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleMovement : MonoBehaviour
{
    [Header("Hole Mesh")]
    [SerializeField] MeshFilter meshFilter;
    [SerializeField] MeshCollider meshCollider;
    [Space]
    [Header("Hole Vertices radius")]
    [SerializeField] float radius;
    [SerializeField] Transform holeCenter;
    [SerializeField] float _lerpSpeed;
    


    Mesh mesh;
    List<int> holeVertices;
    List<Vector3> offsets;
    int holeVerticesCount;

    float _x, _z;
    Vector3 touch, targetPos;
    
    void Start()
    {
        GameSettings.isMoving = false;
        GameSettings.isGameOver = false;
        holeVertices = new List<int>();
        offsets = new List<Vector3>();
        mesh = meshFilter.mesh;
        //Find the hole vertices on the ground(mesh)
        FindHoleVertices();
        
    }
    void Update()
    {
        GameSettings.isMoving = Input.GetMouseButton(0);
        if(!GameSettings.isGameOver && GameSettings.isMoving)
        {
            // move hole center
            MoveHole();
            //Update hole vertices
            UpdateHoleVerticesPosition();
        }

    }

    private void UpdateHoleVerticesPosition()
    {
        Vector3[] vertices = mesh.vertices;
        for (int i = 0; i < holeVerticesCount; i++)
        {
            vertices[holeVertices[i]] = holeCenter.position + offsets[i];
        }
        //update mesh  
        mesh.vertices = vertices;
        meshFilter.mesh = mesh;
        meshCollider.sharedMesh = mesh;
    }

    private void MoveHole()
    {
        _x = Input.GetAxis("Mouse X");
        _z = Input.GetAxis("Mouse Y");
        touch = Vector3.Lerp(holeCenter.position, holeCenter.position + new Vector3(_x, -0.01726f, _z), _lerpSpeed * Time.deltaTime);

        holeCenter.position = touch;    
    }

    private void FindHoleVertices()
    {
        for (int i = 0; i < mesh.vertices.Length; i++)
        {
            float distance = Vector3.Distance(holeCenter.position , mesh.vertices[i]);
            if(distance<radius)
            {
                holeVertices.Add(i);
                offsets.Add(mesh.vertices[i] - holeCenter.position);
            }

        }
        holeVerticesCount = holeVertices.Count;
    }
    private void OnDrawGizmos()
    {   
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(holeCenter.position, radius);
    }
}
