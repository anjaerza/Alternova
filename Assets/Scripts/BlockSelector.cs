using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSelector : MonoBehaviour
{
    public Camera mainCamera; // Assign the main camera in the Inspector

    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Detect left mouse button click
        {
            SelectBlock();
        }
    }

    void SelectBlock()
    {
        Debug.Log("SELECT BLOCK");
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            GameObject selectedBlock = hit.collider.gameObject;
            if (selectedBlock.CompareTag("Block")) // Assuming all your cubes are tagged with "Cube"
            {
                Debug.Log("Selected Block: " + selectedBlock.name);
                // Return or use the selected cube as needed
                selectedBlock.GetComponent<BlockController>().BlockSelected();
            }

            else
            {
                Debug.Log("fail");
            }
        }
    }
}