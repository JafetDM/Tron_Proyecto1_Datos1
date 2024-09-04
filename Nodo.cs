using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Schema;
using JetBrains.Annotations;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace Tron
{
public class Node 
{
    //atributos
     //informacion del nodo
     public int X;
     public int Y;


    //punteros o referencia del nodo siguiente
    public Node Up; 
    public Node Down;
    public Node Left;
    public Node Right;

// metodos

    public Node (int x, int y) //constructor
    {
        X = x;
        Y = y;
        Up = null;
        Down = null;
        Left = null;
        Right = null;
        //punteros inician en tierra

    }
    
}
