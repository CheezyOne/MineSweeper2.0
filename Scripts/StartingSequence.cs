using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class StartingSequence : MonoBehaviour
{
    private List<GameObject> AllCubes = new List<GameObject>(), FallingCubes = new List<GameObject>();
    private List<Vector3> AllStartingPositions = new List<Vector3>(), AllEndPositions = new List<Vector3>();
    private bool CubesShouldFall = false;
    private float TimeBeforeNextCubeFalls, FallSpeed=15f;
    private void Awake()
    {
        FieldGeneration.onFieldGenerated += ApplyCubes;
        FieldGeneration.onFieldGenerated += CubesShouldFallChange;
        FieldGeneration.onFieldGenerated += StartFallingCoroutine;
    }
    private void CubesShouldFallChange()
    {
        CubesShouldFall = !CubesShouldFall;
    }
    private void StartFallingCoroutine()
    {
        StartCoroutine(AddNewCubesFalling());
    }
    private void ApplyCubes()
    {
        AllCubes = FieldGeneration.AllCubes;
        TimeBeforeNextCubeFalls = 5 / AllCubes.Count;
    }
    private void DecideStartAndEndPositions(GameObject Cube)
    {
            AllStartingPositions.Add(Cube.transform.position);
            AllEndPositions.Add
                (new Vector3
                (Cube.transform.position.x,
                Cube.transform.position.y-11f,
                Cube.transform.position.z)
                );
    }
    private GameObject PickRandomCube()
    {
        int Randomnumber = Random.Range(0, AllCubes.Count);
        GameObject RandomCube = AllCubes[Randomnumber];
        AllCubes.RemoveAt(Randomnumber);
        return RandomCube;
    }
    private IEnumerator AddNewCubesFalling()
    {

        yield return new WaitForSeconds(TimeBeforeNextCubeFalls);
        GameObject RandomCube = PickRandomCube();
        DecideStartAndEndPositions(RandomCube);
        FallingCubes.Add(RandomCube);
        if (AllCubes.Count == 0)
        {
            StopAllCoroutines();
            yield return null;
        }
        yield return AddNewCubesFalling();
    }
    private void FallCubes()
    {
        for (int i = 0; i < FallingCubes.Count; i++) //change
        {
            FallingCubes[i].transform.position = Vector3.MoveTowards(FallingCubes[i].transform.position, AllEndPositions[i], Time.deltaTime* FallSpeed);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (CubesShouldFall)
        {
            FallCubes();
            if (FallingCubes.Count > 0)
            {
                if (FallingCubes[FallingCubes.Count - 1].transform.position == AllEndPositions[FallingCubes.Count - 1])
                {
                    ClickRegister.isGameOn = true;
                    CubesShouldFall = false;
                }
            } 
        }
    }
}
