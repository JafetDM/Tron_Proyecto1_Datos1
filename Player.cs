using System;
using System.Security.Cryptography;
using TMPro;
using Tron;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

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

    private Collider2D playerHitBox;
    private Vector2 moveInput; //recibe los inputs para mover
    private Boolean isMoving = true;

    //atributos para la lista enlazada "estelaLuz"
    private LinkedList<Vector2> estelaLuz = new LinkedList<Vector2>();
    private LinkedList<GameObject> spriteEstela = new LinkedList<GameObject>();
    private int estelaSize =30;

    //atributos para la stack de los items

    

    System.Random random = new System.Random();

    private float combustible = 1000f;




    //Metodos

    // Start is called before the first frame update
    void Start() //inicia todo
    {
        playerRB = GetComponent<Rigidbody2D>(); //asigna el RigidBody
        moveInput = Vector2.up; //inicia con una direcion predeterminada
        // Verifica si el jugador ya tiene un BoxCollider2D
        playerHitBox = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update() //actualiza lo que ocurre
    {
        Direccionar();
        combustible -= 0.2f;

        if (combustible <=0)
        {
            Destroy(gameObject);
        }

        if (isMoving == false)
        {
            Destroy(gameObject);
        }
    }

    void Direccionar() //da la direccion a la que se dirige
    {

        if (isMoving == true)

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

            }
    
    }

    private void FixedUpdate() //intervalos de actualizacion fijas
    {
        Limites();
        GenerarEstela();

        Vector2 normalizedMoveInput = moveInput.normalized; //vector normalizado para evitar errores

        //fisicas van aqui para evitar que el cambio del framerate afecte

        //mover al personaje
        playerRB.MovePosition(playerRB.position + normalizedMoveInput * speed * Time.fixedDeltaTime); //obtiene la posicion, le suma el vector multiplicado por la velocidad
        
    }


    void Limites()
    {

        if (playerRB.position.x < 1)
        {
            isMoving = false;
        }

        if (playerRB.position.y < 1)
        {
            isMoving = false;
        }

        if (playerRB.position.x > 48)
        {
            isMoving = false;
        }

        if (playerRB.position.y >48 )
        {
            isMoving = false;
        }
    }

    void GenerarEstela()
    {
        if (isMoving == true)
        { 
            // se agrega la posicion del jugador a la lista de la estela
            gridposition = playerRB.position;
            estelaLuz.InsertarI(gridposition);

            //crear los cubos (tanto en dibujo como en lista(cubo y direccion))
            for (int i =0; i<estelaLuz.size; i++)
            {
                if (isFacingUp == true)
                {
                    //asignar posicion de la estela
                    SimpleNode<Vector2> node = estelaLuz.Get(i);
                    Vector2 estelaPosition= node.dato;
                    Vector3 position = new Vector3(estelaPosition.x, estelaPosition.y, 3);
                    
                    //crear el cubo
                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube.transform.position = position;
                    cube.tag = "cubo";
                    //cambiar el color de la estela
                    Renderer cubeRenderer = cube.GetComponent<Renderer>();
                    cubeRenderer.material.color = Color.cyan;
                    cube.name = $"EstelaNode ({estelaPosition.x},{estelaPosition.y})";

                    //insertar el cubo en la lista para manejarlo desde ahi
                    spriteEstela.InsertarI(cube);
                }

                else if (isFacingDown)
                {
                    //asignar posicion de la estela
                    SimpleNode<Vector2> node = estelaLuz.Get(i);
                    Vector2 estelaPosition= node.dato;
                    Vector3 position = new Vector3(estelaPosition.x, estelaPosition.y, 3);
                    
                    //crear el cubo
                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube.transform.position = position;
                    cube.tag = "cubo";
                    //cambiar el color de la estela
                    Renderer cubeRenderer = cube.GetComponent<Renderer>();
                    cubeRenderer.material.color = Color.cyan;
                    cube.name = $"EstelaNode ({estelaPosition.x},{estelaPosition.y})";

                    //insertar el cubo en la lista para manejarlo desde ahi
                    spriteEstela.InsertarI(cube);
                }

                else if (isFacingRight)
                {
                    //asignar posicion de la estela
                    SimpleNode<Vector2> node = estelaLuz.Get(i);
                    Vector2 estelaPosition= node.dato;
                    Vector3 position = new Vector3(estelaPosition.x, estelaPosition.y, 3);
                    
                    //crear el cubo
                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube.transform.position = position;
                    cube.tag = "cubo";
                    //cambiar el color de la estela
                    Renderer cubeRenderer = cube.GetComponent<Renderer>();
                    cubeRenderer.material.color = Color.cyan;
                    cube.name = $"EstelaNode ({estelaPosition.x},{estelaPosition.y})";

                    //insertar el cubo en la lista para manejarlo desde ahi
                    spriteEstela.InsertarI(cube);
                }

                else if (isFacingLeft)
                {
                    //asignar posicion de la estela
                    SimpleNode<Vector2> node = estelaLuz.Get(i);
                    Vector2 estelaPosition= node.dato;
                    Vector3 position = new Vector3(estelaPosition.x, estelaPosition.y, 3);
                    
                    //crear el cubo
                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube.transform.position = position;
                    cube.tag = "cubo";
                    //cambiar el color de la estela
                    Renderer cubeRenderer = cube.GetComponent<Renderer>();
                    cubeRenderer.material.color = Color.cyan;
                    cube.name = $"EstelaNode ({estelaPosition.x},{estelaPosition.y})";

                    //insertar el cubo en la lista para manejarlo desde ahi
                    spriteEstela.InsertarI(cube);
                }

                if (spriteEstela.size > estelaSize ) //si la estela se hace mas grande de lo que deberia
                {
                    //se obtiene el nodo con la ultima estela
                    SimpleNode<GameObject> lastEstela = spriteEstela.Get(spriteEstela.size -1);
                    Destroy(lastEstela.dato);
                    estelaLuz.EliminarF(); //se elimina la ultima posicion de estela
                    spriteEstela.EliminarF();
                }
                
                
            }
        }
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

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Verifica si el objeto con el que colisionó tiene el tag "Enemigos"
        if (collision.gameObject.CompareTag("Enemigos"))
        {
            // Destruye la moto del jugador y la estela
            
            Destroy(gameObject);
            Destroy(collision.gameObject); //destruye la otra moto

        }

        if (collision.gameObject.CompareTag("Item"))
        {
            int numAleatorio = random.Next(0,3);
            if (numAleatorio == 0) //el item es de aumento de estela
            {
                estelaSize += 30*random.Next(0,11);
            }

            if (numAleatorio == 1) //el item es de celda de combustible
            {
                combustible =+ random.Next(0,31);
            }

            if (numAleatorio ==2)

            {
                Destroy(gameObject);
            }
            
            Destroy(collision.gameObject); //elimina el item del mapa
        }

        if (collision.gameObject.CompareTag("cubo"))
        {
            Destroy(gameObject);
            Destroy(collision.gameObject);
        }




    }


}
