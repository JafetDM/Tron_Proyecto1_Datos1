using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using UnityEngine;

public class itemSpawnerScript : MonoBehaviour
{
    public GameObject item;
    private Vector2 itemPosition;

    // Start is called before the first frame update
    void Start()
    {
        for (int i =0; i <6; i++)
        {
            Instantiate(item);
            Vector2 itemPosition = new Vector2(Random.Range(0,48), Random.Range(0,48));
            item.transform.position = itemPosition;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
