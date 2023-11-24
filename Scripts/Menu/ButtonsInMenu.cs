using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ButtonsInMenu : MonoBehaviour
{
    public static Action onPlayButtonPress;
    private string[] MapNames = new string[] {"Square","Cross","4 Sides", "Hole", "Diamond" };
    [SerializeField] private GameObject MapNameText, MapNameChangeRight, MapNameChangeLeft;
    private static int MapNameNumber = 0;
    
    public void PlayButton()
    {
        Camera.main.transform.Rotate(90, 0, 0);
        onPlayButtonPress?.Invoke();
        gameObject.SetActive(false);
    }
    public void ChangeMapNameLeft()
    {
        if(MapNameNumber != 0)
        {
            MapNameChangeRight.SetActive(true);
            MapNameNumber--;
            SetMapNameText();
            if (MapNameNumber == 0)
            {
                MapNameChangeLeft.SetActive(false);
            }
        }
    }
    private void SetMapNameText()
    {
        MapNameText.GetComponent<TextMeshProUGUI>().text = MapNames[MapNameNumber];
        FieldGeneration.FieldType = MapNames[MapNameNumber];
    }    
    public void ChangeMapNameRight()
    {
        if (MapNameNumber != MapNames.Length-1)
        {
            MapNameChangeLeft.SetActive(true);
            MapNameNumber++;
            SetMapNameText();
            if (MapNameNumber == MapNames.Length-1)
            {
                MapNameChangeRight.SetActive(false);
            }
        }
    }
}
