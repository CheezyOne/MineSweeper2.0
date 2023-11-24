using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickRegister : MonoBehaviour
{
    public static bool isGameOn = false; // This is the public bool variable
    private bool theFirstCube = true;
    public static Action onFirstCubeTouch;
    private GameObject touchedCube;
    void Update()
    {
        if (isGameOn)
        {
            if(Input.GetMouseButtonUp(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.TryGetComponent<Cell>(out Cell CellComponent))
                    {
                        if (hit.transform.gameObject == touchedCube)
                        {
                            if (theFirstCube)
                            {
                                theFirstCube = false;
                                ApplyMines.CantBeBomb = hit.transform.gameObject;
                                onFirstCubeTouch?.Invoke();
                            }
                            hit.transform.GetComponent<Cell>().WasClicked();
                        }
                    }
                }
            }
            if (Input.GetMouseButtonDown(0)) // 0 is the left mouse button
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if(hit.transform.TryGetComponent<Cell>(out Cell CellComponent))
                    {
                        touchedCube = hit.transform.gameObject;
                    }
                }
            }
            else if (Input.GetMouseButtonDown(1)) // 1 is the right mouse button
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.TryGetComponent<Cell>(out Cell CellComponent))
                    {
                        CellComponent.WasRightClicked();
                    }
                }
            }
        }
    }
}
