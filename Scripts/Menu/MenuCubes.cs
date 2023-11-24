using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MenuCubes : MonoBehaviour
{
    private float RandomX, RandomY, RandomZ;
    private Vector3 EndPosition;
    private void Awake()
    {
        Destroy(gameObject, 25f);
        EndPosition = transform.position + new Vector3(Random.Range(30, 40), Random.Range(20,30), 0);
        RandomX = Random.Range(-2000, 2000) / 1000;
        RandomY = Random.Range(-2000, 2000) / 1000;
        RandomZ = Random.Range(-2000, 2000) / 1000;
    }
    void Update()
    {
        transform.Rotate(RandomX, RandomY, RandomZ);
        transform.position = Vector3.MoveTowards(transform.position, EndPosition, Time.deltaTime*Random.Range(2,4));
    }
}
