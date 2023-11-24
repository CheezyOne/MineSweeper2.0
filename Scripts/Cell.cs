using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public static Action onEmptyCellClicked, onGameLose;
    public bool HasBlueBomb = false, HasRedBomb = false, HasBeenTouched=false;
    private bool HasBarierUp = false, HasBarierDown = false, HasBarierRight = false, HasBarierLeft = false;
    [SerializeField] private GameObject NumberAboutBombs;
    public int NumberOfRedBombsAround = 0, NumberOfBlueBombsAround=0;
    private Collider[] hitColliders;
    [SerializeField] private Material[] AllMaterials;
    [SerializeField] private GameObject ExplosionEffect;
    private bool ShouldChangeBombNumbers = false, IsShowingRedBombs=true;
    private float TimerToChangeBombNumbers = 1f;
    private void Awake()
    {
        BombsInGameChanger.onBombsMove += GetSurroundingCubesInfo;
        ApplyMines.onAllBombsApply += GetSurroundingCubesInfo;
        onGameLose += ShowIfTheBombTextureIsRight;
        onGameLose += SpawnExplosion;
    }
    private void SpawnExplosion()
    {
        if (HasRedBomb||HasBlueBomb)
        {
            if (GetComponent<MeshRenderer>().material.name != "Bomb (Instance)")
            {
                GetComponent<MeshRenderer>().material = AllMaterials[2];
            }
            Instantiate(ExplosionEffect, new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), Quaternion.identity);
        }
    }
    private void ShowIfTheBombTextureIsRight()
    {
        if(GetComponent<MeshRenderer>().material.name == "Bomb (Instance)")
        {
            if (!HasRedBomb&&!HasBlueBomb)
                GetComponent<MeshRenderer>().material.color = Color.red;
            else
                GetComponent<MeshRenderer>().material.color = Color.green;
        }
    }
    public List<GameObject> GetAllNeighbours()
    {
        List<GameObject> Neighbours = new List<GameObject>();
        hitColliders = Physics.OverlapSphere(transform.position, 1f);
        for (int i = 0;i<hitColliders.Length;i++)
        {
            Neighbours.Add(hitColliders[i].gameObject);
        }
        return Neighbours;
    }
    public GameObject GetRandomNeighbour()
    {
        hitColliders = Physics.OverlapSphere(transform.position, 1f);
        int RN = UnityEngine.Random.Range(0, hitColliders.Length);
        GameObject RandomNeighbour = hitColliders[RN].gameObject;
        if (RandomNeighbour.transform.name != "Cube(Clone)")
            return RandomNeighbour;
        return null;
    }
    private void DecideBarierPosition(Collider Barier)
    {
        float BarierX = Barier.transform.position.x, BarierZ= Barier.transform.position.z;
        float TransformX = transform.position.x, TransformZ = transform.position.z;
        if (BarierX > TransformX && Math.Abs(BarierZ - TransformZ) < 0.1f)
            HasBarierRight = true;
        else if (BarierX < TransformX && Math.Abs(BarierZ - TransformZ) < 0.1f)
            HasBarierLeft = true;
        else if (BarierZ > TransformZ && Math.Abs(BarierX - TransformX)<0.1f)
            HasBarierUp = true;
        else if (BarierZ < TransformZ && Math.Abs(BarierX - TransformX) < 0.1f)
            HasBarierDown = true;
    }
    private bool IsBehindABarier(Collider Object)
    {
        float ObjectX = Object.transform.position.x, ObjectZ = Object.transform.position.z;
        float TransformX = transform.position.x, TransformZ = transform.position.z;
        if (ObjectX - TransformX > 0.1f && HasBarierRight)
        {
            return true;
        }
        if (TransformX - ObjectX > 0.1f && HasBarierLeft)
        {
            return true;
        }
        if (ObjectZ - TransformZ >0.1f && HasBarierUp)
        {
            return true;
        }
        if (TransformZ - ObjectZ >0.1f && HasBarierDown)
        {
            return true;
        }
        return false;
    }
    public void GetSurroundingCubesInfo()
    {
        // Get all colliders within a certain radius of the hit point
        hitColliders = Physics.OverlapSphere(transform.position, 1f);
        NumberOfRedBombsAround = 0;
        NumberOfBlueBombsAround = 0;
        // Iterate over the colliders
        foreach (Collider collider in hitColliders)
        {
            if (collider.gameObject == gameObject)
                continue;
            if(collider.transform.name=="Barier(Clone)")
            {
                DecideBarierPosition(collider);
            }
        }
        foreach (Collider collider in hitColliders)
        {
            if (collider.gameObject == gameObject)
                continue;
            Cell CellComponent = collider.GetComponent<Cell>();
            if (CellComponent != null)
            {
                if (CellComponent.HasRedBomb)
                {
                    if (IsBehindABarier(collider))
                        continue;
                    NumberOfRedBombsAround++;
                }
                if(CellComponent.HasBlueBomb)
                {
                    if (IsBehindABarier(collider))
                        continue;
                    NumberOfBlueBombsAround++;
                }
            }
        }
        /*
        if (UnityEngine.Random.Range(70, 100) > 50)//Liyng blocks
        {
            if (UnityEngine.Random.Range(0, 100) > 50)
                NumberAboutRedBombs.GetComponent<TextMeshPro>().text = Convert.ToString(NumberOfRedBombsAround + 1);
            else
                NumberAboutRedBombs.GetComponent<TextMeshPro>().text = Convert.ToString(NumberOfRedBombsAround - 1);
        }
        else
        */
        NumberAboutBombs.GetComponent<TextMeshPro>().text = Convert.ToString(NumberOfRedBombsAround);
    }
    public void WasClicked()
    {
        HasBeenTouched = true;
        if (HasRedBomb||HasBlueBomb)
        {
            onGameLose?.Invoke();
            //ClickRegister.isGameOn = false;
            return;
        }
        else if (NumberOfRedBombsAround > 0|| NumberOfBlueBombsAround>0)
        {
            NumberAboutBombs.SetActive(true);
            if (NumberOfRedBombsAround > 0 && NumberOfBlueBombsAround > 0)
            {
                NumberAboutBombs.GetComponent<TextMeshPro>().color = Color.red;
                ShouldChangeBombNumbers = true;
            }
            else if (NumberOfRedBombsAround > 0)
                NumberAboutBombs.GetComponent<TextMeshPro>().text = Convert.ToString(NumberOfRedBombsAround);
            else if (NumberOfBlueBombsAround > 0)
            {
                NumberAboutBombs.GetComponent<TextMeshPro>().color = Color.red;
                NumberAboutBombs.GetComponent<TextMeshPro>().text = Convert.ToString(NumberOfBlueBombsAround);
            }
        }
        else
        {
            foreach (Collider collider in hitColliders)
            {
                if (collider.gameObject == gameObject)
                    continue;
                Cell CellComponent = collider.GetComponent<Cell>();
                if (CellComponent != null)
                {
                    if (CellComponent.HasBeenTouched)
                        continue;
                    if (IsBehindABarier(collider))
                        continue;
                    CellComponent.WasClicked();
                }
            }
        }
        if(GetComponent<MeshRenderer>().material.name == "Normal cube (Instance)" || GetComponent<MeshRenderer>().material.name == "Pointed cube (Instance)")
        {
            onEmptyCellClicked?.Invoke();
        }
            GetComponent<MeshRenderer>().material = AllMaterials[1];
    }
    public void WasRightClicked()
    {
        string CurrentMaterial = GetComponent<MeshRenderer>().material.name ;
        switch (CurrentMaterial)
        {
            case "Normal cube (Instance)":
                {
                    GetComponent<MeshRenderer>().material = AllMaterials[2];
                    break;
                }
            case "Pointed cube (Instance)":
                {
                    GetComponent<MeshRenderer>().material = AllMaterials[2];
                    break;
                }
            case "Bomb (Instance)":
                {
                    GetComponent<MeshRenderer>().material = AllMaterials[0];
                    break; 
                }
            default:
                {
                    break;
                }
        }
    }
    private void ChangeBombNumbers()
    {
        if (IsShowingRedBombs)
        {
            NumberAboutBombs.GetComponent<TextMeshPro>().text = Convert.ToString(NumberOfBlueBombsAround);
            NumberAboutBombs.GetComponent<TextMeshPro>().color = Color.red;
            IsShowingRedBombs = false;
        }
        else
        {
            IsShowingRedBombs = true;
            NumberAboutBombs.GetComponent<TextMeshPro>().text = Convert.ToString(NumberOfRedBombsAround);
            NumberAboutBombs.GetComponent<TextMeshPro>().color = Color.blue;
        }

    }
    private void Update()
    {
        if (ShouldChangeBombNumbers)
        {
            if (TimerToChangeBombNumbers <= 0)
            {
                ChangeBombNumbers();
                TimerToChangeBombNumbers = 2.5f;
            }
            TimerToChangeBombNumbers -= Time.deltaTime;
        }
    }
}
