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

//metodo para dibujar el grid
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

                // Alternar colores para crear un patron alternado
                Color color = (x + y) % 2 == 0 ? Color.black: Color.gray;
                Renderer cubeRenderer = cube.GetComponent<Renderer>();
                cubeRenderer.material.color = color;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
