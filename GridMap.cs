using System.Collections;
using System.Collections.Generic;
using Tron;
using UnityEngine;

public class GridMap : MonoBehaviour
{

    public int gridSize = 50;
    private GridLinkedList grid;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("grid manager inicio");
        grid = new GridLinkedList(gridSize);
        DibujarGrid();
    
    }

    private void DibujarGrid()
    {
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                Node node = grid.GetNode(x, y);
                Vector3 position = new Vector3(x, y, -5);
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.position = position;
                cube.name = $"Node ({x},{y})";

                Renderer cubeRenderer = cube.GetComponent<Renderer>();
                cubeRenderer.material.color = Color.cyan;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
