using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml;
using System.Xml.Schema;
using JetBrains.Annotations;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;

namespace Tron
{
public class Node //nodo para el grid
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

public class SimpleNode //nodo generalizado para objetos
{
    // atributos
    public object dato;
    public SimpleNode Next;

    //constructor
    public SimpleNode(object data)
    {
        dato = data;
        Next = null;
    }

}

public class LinkedList //lista generalizada para objetos
{
    //atributos

    private SimpleNode head; //puntero cabeza que servira para recorrer la lista
    public int size;

    //metodos
    public LinkedList()
    {
        head = null; //puntero cabeza inicia en el principio
        size=0;
    }

    public void InsertarI(object data)//metodo que inserta al inicio
    {
        //creamos el nuevo nodo
        SimpleNode newNode = new SimpleNode(data); //crea el nuevo nodo

        if (head == null) 
        {
            head = newNode; //asigna el nodo al principio
        }

        else
        {
            SimpleNode puntero; //creamos nodo para ayudarnos
            puntero = head;
            head = newNode;
            newNode.Next = puntero;
        }

        size +=1;

    }

    public void InsertarF(object data) //metodo que agrega nodos al final
    {
        SimpleNode newNode = new SimpleNode(data); //crea el nuevo nodo

        if (head == null) //si la lista esta vacia
        {
            head = newNode; //asigna el puntero cabeza al nuevo nodo

        }

        else
        {
            SimpleNode current = head; //empieza a recorrer la lista
            while (current.Next != null)
            {
                current = current.Next; //recorre la lista hasta que llegue al final
            }

            current.Next = newNode; //asigna el nuevo nodo al final de la lista

        }

        size +=1;

    }

    public void EliminarF()
    {
        if (head ==null)//si esta vacia
        {
            Console.WriteLine("lista esta vacia");
            return;
        }

        if (head.Next==null) //si solo tiene un elemento
        {
            head = null;
        }

        else
        {
            SimpleNode puntero = head; //puntero para ayudar a recorrer
            while (puntero.Next.Next != null)
            {
                puntero = puntero.Next;
            }
            puntero.Next =null; //despues de recorrer todo, elimina el ultimo
        }

        size -= 1;
    }

}

public class GridLinkedList
{
    //atributos
    private Node head; //nodo de inicio
    private int gridSize; //variable para el tamano de celdas

    //metodos

    //constructor
    public GridLinkedList (int size)
    {
        gridSize = size;
        IniciarGrid();
    }

    //metodo que crea una matriz 

    private void IniciarGrid()
    {
        //definimos los nodos a usar para el movimiento en la lista
        Node previousHead = null;
        Node currentHead = null;
        Node previousNode = null; 

        for (int y = 0; y < gridSize; y++)
        {
            for (int x=0; x<gridSize; x++)
            {
                //creamos el nodo para cada espacio en el grid
                Node newNode = new Node (x,y);

                //si es el primer nodo en una fila
                if (x==0)
                {
                    //si es el primer nodo en esa fila (primero en fila y columna)

                    if (y==0)
                    {
                        head = newNode;
                    }

                    else if (previousHead != null)
                    {
                        previousHead.Down = newNode; //asignamos verticalmente
                        newNode.Up = previousHead;
                    }

                    currentHead = newNode; //asignamos el primero
                }

                else

                {
                    previousNode.Right = newNode; //asignamos horizontalmente
                    newNode.Left = previousNode;

                    if (previousNode.Up != null) //excepcion en esquinas
                    {
                        newNode.Up = previousNode.Up.Right;
                        previousNode.Up.Right.Down = newNode;
                    }
                }

                previousNode = newNode; //nos movemos
            }

            previousHead = currentHead;
            previousNode = null;
        }
    }

//metodo para obtener el nodo y poderlo dibujar
    public Node GetNode(int x, int y)
    {
        Node current = head;

        for (int i =0; i<y; i++)
        {
            current = current.Down; //moverse abajo por filas
        }

        for (int j = 0; j<x; j++)

        {
            current = current.Right; //moverse derecha en las filas
        }

        return current;


    }
}
}
