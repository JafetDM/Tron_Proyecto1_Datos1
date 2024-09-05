using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{

    //atributos

    private Vector2 targetPosition; //vector unitario para mantener el target de donde deberia posicionarse
    private float xInput, yInput; //

    private Boolean isFacingUp = true;
    private Boolean isFacingRight = true;

    [SerializeField] private float speed = 3f; //maneja la rapidez del jugador

    private Rigidbody2D playerRB; //RigidBody del player
    private Vector2 moveInput; //recibe los inputs para mover

    //metodos

    // Start is called before the first frame update
    void Start() //inicia todo
    {
        playerRB = GetComponent<Rigidbody2D>(); //asigna el RigidBody
        moveInput = Vector2.up; //inicia con una direcion predeterminada
    }

    // Update is called once per frame
    void Update() //actualiza lo que ocurre
    {
        xInput = Input.GetAxisRaw("Horizontal"); //obtiene inputs para el movimiento en x (recibe 1)
        yInput = Input.GetAxisRaw("Vertical"); // mismo pero vertical (recibe -1)
        
        if (xInput != 0f || yInput != 0f) //si la entrada x || y es distinta a 0 
        {
            CalcularTargetPosition();
        }

        else
        {
            targetPosition = moveInput;
        }
        
        //ahora se cambia la direccion dependiendo del input
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            moveInput = Vector2.up;
            flip();
        }

        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            moveInput = Vector2.down;
            flip();
        }

        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            moveInput = Vector2.left;
            flip();
        }

        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            moveInput = Vector2.right;
            flip();
        }
    }

    private void FixedUpdate() //intervalos de actualizacion fijas
    {
        //fisicas van aqui para evitar que el cambio del framerate afecte
        playerRB.MovePosition(playerRB.position + moveInput *speed * Time.fixedDeltaTime); //obtiene la posicion, le suma el vector multiplicado por la velocidad

    }

    private void CalcularTargetPosition()
    {
        if(xInput == 1f)
        {
            targetPosition = (Vector2)transform.position + Vector2.right;
        }

        else if (xInput ==-1f)
        {
            targetPosition = (Vector2)transform.position + Vector2.left;
        }

        else if (yInput ==1f)
        {
            targetPosition = (Vector2)transform.position + Vector2.up;
        }

        else if (yInput == -1f)
        {
            targetPosition = (Vector2)transform.position + Vector2.down;
        }


    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(targetPosition, 0.15f);
    }

    private void flip()
    {
        if (yInput == -1f && isFacingUp)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 90f); //lo gira arriba
            isFacingUp = false;
        }
        if (yInput == 1f && !isFacingUp)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, -90f); //lo gira abajo
            isFacingUp = true;
        }

        if (xInput == -1f && isFacingRight)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f); //lo gira derecha
            isFacingRight = false;
        }

        if (xInput == 1f && !isFacingRight)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 180f); //lo gira izquierda
            isFacingRight = true;
    }
}
}
