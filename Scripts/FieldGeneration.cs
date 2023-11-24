using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class FieldGeneration : MonoBehaviour
{
    public static string FieldType = "Square";
    [SerializeField] private GameObject Cell, Field, Barier;
    private Vector3 PositionFromField = Vector3.zero;
    private float ColomnsDivison = 1.1f * 10, StringsDivision = 1.1f * 10;
    private int Counter = 0, SpecialFormsCounter=0;//SpecialFormsCounter Надо менять, если будем менять масшаб
    private ApplyMines ApplyComponent;
    public static List <GameObject> AllCubes = new List<GameObject>();
    public static Action onFieldGenerated;
    public static bool ApplyBariers = false;
    public static int BariersCount=0;

    private void GenerateAField()
    {
        switch (FieldType)
        {
            case "Square":
                {
                    ApplyComponent.FieldSize = 100;
                    for (float i=0;i< ColomnsDivison;i+=1.1f)
                    {
                        for(float j=0;j< StringsDivision;j+=1.1f)
                        {
                            GameObject NewCell = Instantiate(Cell, Field.transform.position + PositionFromField, Quaternion.identity);
                            NewCell.transform.SetParent(Field.transform);
                            AllCubes.Add(NewCell);
                            PositionFromField += new Vector3(1.1f, 0, 0);
                            Counter++;
                        }
                        PositionFromField -= new Vector3(1.1f* Counter, 0, 0);
                        Counter = 0;
                        PositionFromField += new Vector3(0, 0, 1.1f);
                    }

                    break;
                }
            case "Cross":
                {
                    ApplyComponent.FieldSize = 84;
                    SpecialFormsCounter = 0;
                    for (float i = 0; i < ColomnsDivison; i += 1.1f)
                    {
                        for (float j = 0; j < StringsDivision; j += 1.1f)
                        {
                            if (Counter > 1 && Counter < 8 || (SpecialFormsCounter>1&& SpecialFormsCounter<8))
                            {
                                GameObject NewCell = Instantiate(Cell, Field.transform.position + PositionFromField, Quaternion.identity);
                                NewCell.transform.SetParent(Field.transform);
                                AllCubes.Add(NewCell);
                            }
                            PositionFromField += new Vector3(1.1f, 0, 0);
                            Counter++;
                        }
                        PositionFromField -= new Vector3(1.1f * Counter, 0, 0);
                        SpecialFormsCounter++;
                        Counter = 0;
                        PositionFromField += new Vector3(0, 0, 1.1f);
                    }
                    break;
                }
            case "Diamond":
                {
                    ApplyComponent.FieldSize = 60;
                    SpecialFormsCounter = 0;
                    for (float i = 0; i < ColomnsDivison/2; i += 1.1f)//Upper part of the diamond
                    {
                        for (float j = 0; j < StringsDivision; j += 1.1f)
                        {
                            if (!(4 - SpecialFormsCounter > Counter || 6 + SpecialFormsCounter <= Counter))
                            {
                                GameObject NewCell = Instantiate(Cell, Field.transform.position + PositionFromField, Quaternion.identity);
                                NewCell.transform.SetParent(Field.transform);
                                AllCubes.Add(NewCell);
                                Counter++;
                                PositionFromField += new Vector3(1.1f, 0, 0);
                                continue;
                            }
                           
                            PositionFromField += new Vector3(1.1f, 0, 0);
                            Counter++;
                        }
                        PositionFromField -= new Vector3(1.1f * Counter, 0, 0);
                        SpecialFormsCounter++;
                        Counter = 0;
                        PositionFromField += new Vector3(0, 0, 1.1f);
                    }
                    SpecialFormsCounter--;
                    for (float i = ColomnsDivison / 2; i < ColomnsDivison ; i += 1.1f)
                    {
                        for (float j = 0; j < StringsDivision; j += 1.1f)
                        {
                            if (!(4 - SpecialFormsCounter > Counter || 6 + SpecialFormsCounter <= Counter))
                            {
                                GameObject NewCell = Instantiate(Cell, Field.transform.position + PositionFromField, Quaternion.identity);
                                NewCell.transform.SetParent(Field.transform);
                                AllCubes.Add(NewCell);
                                Counter++;
                                PositionFromField += new Vector3(1.1f, 0, 0);
                                continue;
                            }

                            PositionFromField += new Vector3(1.1f, 0, 0);
                            Counter++;
                        }
                        PositionFromField -= new Vector3(1.1f * Counter, 0, 0);
                        SpecialFormsCounter--;
                        Counter = 0;
                        PositionFromField += new Vector3(0, 0, 1.1f);
                    }
                    break;
                }
            case "Hole":
                {
                    ApplyComponent.FieldSize = 84;
                    SpecialFormsCounter = 0;
                    for (float i = 0; i < ColomnsDivison; i += 1.1f)
                    {
                        for (float j = 0; j < StringsDivision; j += 1.1f)
                        {
                            if (SpecialFormsCounter >= 3 && SpecialFormsCounter <= 6 && Counter >= 3 && Counter <= 6)
                            {
                                PositionFromField += new Vector3(1.1f, 0, 0);
                                Counter++;
                                continue;
                            }
                            GameObject NewCell = Instantiate(Cell, Field.transform.position + PositionFromField, Quaternion.identity);
                            NewCell.transform.SetParent(Field.transform);
                            AllCubes.Add(NewCell);
                            PositionFromField += new Vector3(1.1f, 0, 0);
                            Counter++;
                        }
                        SpecialFormsCounter++;
                        PositionFromField -= new Vector3(1.1f * Counter, 0, 0);
                        Counter = 0;
                        PositionFromField += new Vector3(0, 0, 1.1f);
                    }
                    break;
                }
            case "4 Sides":
                {
                    SpecialFormsCounter = 0;
                    ApplyComponent.FieldSize = 80;
                    for (float i = 0; i < ColomnsDivison; i += 1.1f)
                    {
                        for (float j = 0; j < StringsDivision; j += 1.1f)
                        {
                            if (Counter == SpecialFormsCounter || 9 - SpecialFormsCounter==Counter)
                            {
                                PositionFromField += new Vector3(1.1f, 0, 0);
                                Counter++;
                                continue;
                            }
                            GameObject NewCell = Instantiate(Cell, Field.transform.position + PositionFromField, Quaternion.identity);
                            NewCell.transform.SetParent(Field.transform);
                            AllCubes.Add(NewCell);
                            PositionFromField += new Vector3(1.1f, 0, 0);
                            Counter++;
                        }
                        SpecialFormsCounter++;
                        PositionFromField -= new Vector3(1.1f * Counter, 0, 0);
                        Counter = 0;
                        PositionFromField += new Vector3(0, 0, 1.1f);
                    }
                    break;
                }
            default:
                {
                    Debug.Log("Can't identify the field type");
                    break;
                }
        }
        if (ApplyBariers)
            CreateBariers();
        onFieldGenerated?.Invoke();
    }
    private void CreateBariers()
    {
        Vector3 BarierPosition =Vector3.zero;
        List<Vector3> AllBariersPositions = new List<Vector3>(); 
        bool ShouldRotate, ShallNotProceed = false;
        int TriesCounter = 0;
        if (BariersCount == 0)
            BariersCount = ApplyComponent.FieldSize /5;
       // BariersCount = 1;
        List<GameObject> AllCubes = new List<GameObject>();
        for(int i=0;i<FieldGeneration.AllCubes.Count;i++)
        {
            AllCubes.Add(FieldGeneration.AllCubes[i]);
        }
        for(int i=0;i<BariersCount;i++)
        {
            ShouldRotate = false;
            int RandomInt=UnityEngine.Random.Range(0, AllCubes.Count - 1);

            GameObject Neighbour = null;
            while (Neighbour == null)
            {
                Neighbour = AllCubes[RandomInt].GetComponent<Cell>().GetRandomNeighbour();
                TriesCounter++;
                if (TriesCounter > 80)
                    return;
                continue;
            }

            if (Neighbour.transform.position.x > AllCubes[RandomInt].transform.position.x)
            {
                if (Neighbour.transform.position.z > AllCubes[RandomInt].transform.position.x)
                {
                    BarierPosition = new Vector3(AllCubes[RandomInt].transform.position.x, 0, Neighbour.transform.position.z - AllCubes[RandomInt].transform.position.z + 0.55f);
                }
                else
                {
                    ShouldRotate = true;
                    BarierPosition = new Vector3(Neighbour.transform.position.x - AllCubes[RandomInt].transform.position.x + 0.55f, 0, AllCubes[RandomInt].transform.position.z);
                }
            }
            else if (Neighbour.transform.position.x < AllCubes[RandomInt].transform.position.x)
            {
                if (Neighbour.transform.position.z > AllCubes[RandomInt].transform.position.x)
                {
                    BarierPosition = new Vector3(AllCubes[RandomInt].transform.position.x, 0, Neighbour.transform.position.z - AllCubes[RandomInt].transform.position.z + 0.55f);
                }
                {
                    ShouldRotate = true;
                    BarierPosition = new Vector3(AllCubes[RandomInt].transform.position.x - Neighbour.transform.position.x - 0.55f, 0, AllCubes[RandomInt].transform.position.z);
                }
            }
            else if (Neighbour.transform.position.z > AllCubes[RandomInt].transform.position.x)
                BarierPosition = new Vector3(AllCubes[RandomInt].transform.position.x, 0, Neighbour.transform.position.z - AllCubes[RandomInt].transform.position.z + 0.55f);
            else if (Neighbour.transform.position.z > AllCubes[RandomInt].transform.position.x)
                BarierPosition = new Vector3(AllCubes[RandomInt].transform.position.x, 0, AllCubes[RandomInt].transform.position.z - Neighbour.transform.position.z - 0.55f);

            foreach(Vector3 BP in AllBariersPositions)
            {
                if (BP == BarierPosition)
                {
                    ShallNotProceed = true;
                    break;
                }

            }
            if (ShallNotProceed)
            {
                ShallNotProceed = false;
                continue;
            }
            GameObject NewBarier = Instantiate(Barier, BarierPosition,Quaternion.identity);//Rotate if on the left and right
            AllBariersPositions.Add(BarierPosition);
            if (ShouldRotate)
                NewBarier.transform.Rotate(0, 90f, 0);
            AllCubes.RemoveAt(RandomInt);
        }
    }
    private void Awake()
    {
        ButtonsInMenu.onPlayButtonPress += GenerateAField;
        ApplyComponent = GetComponent<ApplyMines>();
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
            GenerateAField();
        }
    }
}
