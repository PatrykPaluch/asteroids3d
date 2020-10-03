using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Generator : MonoBehaviour {

    [SerializeField]
    private int poolSize = 1000;
    
    public Vector3 center;
    public Vector3 startSize;

    public Vector3 safeArea;

    public GameObject asteroidPrefab;

    private GameObject[] asteroidPool; 
    
    void Start() {
        asteroidPool = new GameObject[poolSize];
        Vector3 min = center - startSize / 2;
        Vector3 max = center + startSize / 2;
        for (int i = 0; i < poolSize; i++) {
            Vector3 randomPosition = center;
            while (IsInSafeArea(randomPosition)) //do poprawy
                randomPosition = RandomVector(min, max);
            
            asteroidPool[i] = Instantiate(asteroidPrefab, 
                    randomPosition, 
                    Quaternion.LookRotation(RandomVector(-1, 1)), 
                    transform);
        }

    }

    /*
     
     +----------+ safeArea
     |        / |
     |      /   |
     |    *     |  center
     |  /       |
     |/         |
     +----------+
     
     */
    private bool IsInSafeArea(Vector3 pos) {
        return pos.x < center.x + safeArea.x / 2 && pos.x > center.x - safeArea.x / 2 &&
               pos.y < center.y + safeArea.y / 2 && pos.y > center.y - safeArea.y / 2 &&
               pos.z < center.z + safeArea.z / 2 && pos.z > center.z - safeArea.z / 2;
    }

    private Vector3 RandomVector(float min, float max) {
        return RandomVector(Vector3.one * min, Vector3.one * max);
    }

    private Vector3 RandomVector(Vector3 min, Vector3 max) {
        return new Vector3(
            Random.Range(min.x, max.x),
            Random.Range(min.y, max.y),
            Random.Range(min.z, max.z)
        );
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(center, 0.25f);
        Gizmos.DrawWireCube(center, startSize);
        Gizmos.DrawWireCube(center, safeArea);
    }
}

