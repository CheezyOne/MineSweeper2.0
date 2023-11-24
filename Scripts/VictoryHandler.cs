using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryHandler : MonoBehaviour
{
    public static int EmptyCellsCount;
    private int ClickedCellsCount = 0;
    private void Awake()
    {
        Cell.onEmptyCellClicked += NewCellClicked;
    }
    private void NewCellClicked()
    {
        ClickedCellsCount++;
       //Debug.Log(ClickedCellsCount);
        if (ClickedCellsCount == EmptyCellsCount)
            Victory();
    }    
    private void Victory()
    {
        ClickRegister.isGameOn = false;
        Debug.Log("You won");
    }
}
