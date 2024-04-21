using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionScreen : MonoBehaviour
{
    public GameObject instructionScreen;

    void Start()
    {
        instructionScreen.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleInstructionScreen();
        }
    }

    void ToggleInstructionScreen()
    {
        instructionScreen.SetActive(!instructionScreen.activeSelf);
    }
}

