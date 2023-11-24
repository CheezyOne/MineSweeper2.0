using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyMines : MonoBehaviour
{
    [SerializeField] private GameObject MineField;
    public static GameObject CantBeBomb;
    public int FieldSize = 0;
    private int RedBombCount, BlueBombCount, CantBeBombNumber;
    private List<int> RedBombCells = new List<int>(), BlueBombCells = new List<int>();
    public static Action onAllBombsApply;
    public static bool ApplyBlueBombs=false;
    private void Awake()
    {
        ClickRegister.onFirstCubeTouch += ApplyBombCells;
    }
    private void DecideBombCount()
    {

        RedBombCount = FieldSize / 10 + FieldSize / 20;
        if(ApplyBlueBombs)
        {
            RedBombCount /= 2;
            BlueBombCount = RedBombCount;
        }

        //RedBombCount = FieldSize/2;
    }
    private int GetRandomRedNumber()
    {
        int RandomNumber = UnityEngine.Random.Range(0, FieldSize);
        foreach(int Number in RedBombCells)
        {
            if(RandomNumber==Number || CantBeBombNumber == RandomNumber)
            {
                return GetRandomRedNumber();
            }
        }
        return RandomNumber;
    }
    private int GetRandomBlueNumber()
    {
        int RandomNumber = UnityEngine.Random.Range(0, FieldSize);
        foreach (int Number in BlueBombCells)
        {
            if (RandomNumber == Number || CantBeBombNumber == RandomNumber)
            {
                return GetRandomBlueNumber();
            }
        }
        return RandomNumber;
    }
    private int GetNoBombNumber() 
    {
        for (int i=0;i< MineField.transform.childCount;i++)
        {
            if (CantBeBomb == MineField.transform.GetChild(i).gameObject)
            {
                return i;
            }
        }
        return -1;
    }
    private int GetCellNumber(GameObject Cell)
    {
        for (int i = 0; i < MineField.transform.childCount; i++)
        {
            if (Cell == MineField.transform.GetChild(i).gameObject)
            {
                return i;
            }
        }
        return -1;
    }
    private GameObject FindASpotForANewConnectedRedBomb(int BombNumberToApply)
    {
        GameObject NeighbourBomb = MineField.transform.GetChild(RedBombCells[BombNumberToApply]).GetComponent<Cell>().GetRandomNeighbour();
        for (int i = 0; i < RedBombCells.Count; i++)
        {
            if (GetCellNumber(NeighbourBomb) == RedBombCells[i] || CantBeBomb == NeighbourBomb)
            {
                return null;
            }
        }
        return NeighbourBomb;
    }
    private GameObject FindASpotForANewConnectedBlueBomb(int BombNumberToApply)
    {
        GameObject NeighbourBomb = MineField.transform.GetChild(BlueBombCells[BombNumberToApply]).GetComponent<Cell>().GetRandomNeighbour();
        for (int i = 0; i < BlueBombCells.Count; i++)
        {
            if (GetCellNumber(NeighbourBomb) == BlueBombCells[i] || CantBeBomb == NeighbourBomb)
            {
                return null;
            }
        }
        return NeighbourBomb;
    }
    private void ApplyConnectedBombs()
    {
        int BombNumberToApply = 0, CounterForTries = 0;
        for (int j = 0; j < RedBombCount; j++)
        {
            GameObject NeighbourBomb = null;
            CounterForTries = 0;
            while (NeighbourBomb==null)
            {
                NeighbourBomb = FindASpotForANewConnectedRedBomb(BombNumberToApply);
                CounterForTries++;
                if (CounterForTries > 3)
                {
                    CounterForTries = 0;
                    if (BombNumberToApply > 0)
                        BombNumberToApply--;
                    else
                        BombNumberToApply = RedBombCells.Count;
                }
            }
            BombNumberToApply = RedBombCells.Count;
            RedBombCells.Add(GetCellNumber(NeighbourBomb));
        }
        for (int j = 0; j < BlueBombCount; j++)
        {
            GameObject NeighbourBomb = null;
            CounterForTries = 0;
            while (NeighbourBomb == null)
            {
                NeighbourBomb = FindASpotForANewConnectedBlueBomb(BombNumberToApply);
                CounterForTries++;
                if (CounterForTries > 3)
                {
                    CounterForTries = 0;
                    if (BombNumberToApply > 0)
                        BombNumberToApply--;
                    else
                        BombNumberToApply = BlueBombCells.Count;
                }
            }
            BombNumberToApply = BlueBombCells.Count;
            BlueBombCells.Add(GetCellNumber(NeighbourBomb));
        }
    }    
    private void ApplyNormalBombs()
    {
        for (int i = 0; i < RedBombCount; i++)
        {
            RedBombCells.Add(GetRandomRedNumber());
        }
        for (int i = 0; i < BlueBombCount; i++)
        {
            BlueBombCells.Add(GetRandomRedNumber());
        }
    }
    private int OnlyBlueBombsNumber()
    {
        int OnlyBlueBombCounter = 0;
        bool NotOnlyBlue = false;
        foreach(int BlueBomb in BlueBombCells)
        {
            foreach(int RedBomb in RedBombCells)
            {
                if (RedBomb == BlueBomb)
                {
                    NotOnlyBlue = true;
                    break;
                }
            }
            if (!NotOnlyBlue)
                OnlyBlueBombCounter++;
        }
        return OnlyBlueBombCounter;
    }
    public void ApplyBombCells()
    {
        CantBeBombNumber = GetNoBombNumber();
        DecideBombCount();




        RedBombCells.Add(GetRandomRedNumber());
        if (ApplyBlueBombs)
            BlueBombCells.Add(GetRandomBlueNumber());
        //ApplyConnectedBombs();
        ApplyNormalBombs();

        VictoryHandler.EmptyCellsCount = FieldSize - RedBombCount - OnlyBlueBombsNumber();

        for (int i=0;i< RedBombCount; i++)
        {
            MineField.transform.GetChild(RedBombCells[i]).GetComponent<Cell>().HasRedBomb = true;
        }
        for (int i = 0; i < BlueBombCount; i++)
        {
            MineField.transform.GetChild(BlueBombCells[i]).GetComponent<Cell>().HasBlueBomb = true;
        }
        onAllBombsApply?.Invoke();
    }
}
