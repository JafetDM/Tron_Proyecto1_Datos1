using System;

using Tron;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{

    //atributos

    private Vector2 gridposition;
    private Vector2 targetPosition; //vector unitario para mantener el target de donde deberia posicionarse
    private float xInput, yInput; //input del movimiento

    // booleanos para manejar la rotacion del jugador
    private Boolean isFacingUp = true;
    private Boolean isFacingDown = false;
    private Boolean isFacingLeft = false;
    private Boolean isFacingRight = false;

    [SerializeField] private float speed = 20f; //maneja la rapidez del jugador

    private Rigidbody2D playerRB; //RigidBody del player
    private Vector2 moveInput; //recibe los inputs para mover

    //atributos para la lista enlazada "estelaLuz"
    private LinkedList estelaLuz;
    private int estelaSize;







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
            CalcularTargetPosition(); //calcula a donde se quiere mover
        }

        else
        {
            targetPosition = moveInput;
        }

        //ahora se cambia la direccion dependiendo del input
        //los !isFacing... son para verificar que el personaje no se "devuelva"
        if (Input.GetKeyDown(KeyCode.UpArrow) && !isFacingDown) 
        {
            moveInput = Vector2.up; //lo gira arriba y cambia su direccion
            flip();
        }

        else if (Input.GetKeyDown(KeyCode.DownArrow) &&! isFacingUp )
        {
            moveInput = Vector2.down; //lo mismo pero abajo
            flip();
        }

        else if (Input.GetKeyDown(KeyCode.LeftArrow) &&! isFacingRight)
        {
            moveInput = Vector2.left; //lo mismo pero izquierda
            flip();
        }

        else if (Input.GetKeyDown(KeyCode.RightArrow) &&! isFacingLeft)
        {
            moveInput = Vector2.right; //lo mismo pero derecha
            flip();
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            speed = 40f;
            Console.WriteLine("cambie");
        }
    
        
        
    }

    private void FixedUpdate() //intervalos de actualizacion fijas
    {

        // se agrega la posicion del jugador a la lista de la estela
        gridposition = playerRB.position;
        estelaLuz.InsertarI(gridposition);

        if (estelaLuz.size >= estelaSize +1) //si la estela se hace mas grande de lo que deberia
        {
            estelaLuz.EliminarF(); //se elimina la ultima posicion de estela
        }

        for (int i =0; i<estelaLuz.size; i++)
        {
            
        }

        Vector2 normalizedMoveInput = moveInput.normalized; //vector normalizado para evitar errores

        //fisicas van aqui para evitar que el cambio del framerate afecte

        //mover al personaje
        playerRB.MovePosition(playerRB.position + normalizedMoveInput * speed * Time.fixedDeltaTime); //obtiene la posicion, le suma el vector multiplicado por la velocidad
        

    }

    private void CalcularTargetPosition() //metodo para calcular la posicion a la que quiere moverse
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

    private void OnDrawGizmos() //metodo para revisar los inputs del teclado
    {
        Gizmos.DrawWireSphere(targetPosition, 0.15f);
    }

    private void flip() //metodo para girar al personaje
    {
        if (yInput == -1f )
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 90f); //lo gira abajo
            isFacingUp = false;
            isFacingDown = true;
            isFacingLeft = false;
            isFacingRight = false;
            
            
        }
        if (yInput == 1f )
        {
            transform.rotation = Quaternion.Euler(0f, 0f, -90f); //lo gira arriba
            isFacingUp = true;
            isFacingDown = false;
            isFacingLeft =false;
            isFacingRight = false;
            
        }

        if (xInput == -1f )
        {
            transform.rotation = Quaternion.Euler(0f, 0f, -180f); //lo gira derecha
            isFacingRight = false;
            isFacingLeft = true;
            isFacingUp = false;
            isFacingDown = false;
            
            
        }


        if (xInput == 1f )
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f); //lo gira izquierda
            isFacingRight = true;
            isFacingLeft = false;
            isFacingUp = false;
            isFacingDown = false;
            
            
    }
}
}
