using System;
using System.Collections;
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
public class EnemyScript : MonoBehaviour
{

    //atributos

    private Vector2 gridposition;
    private Vector2 targetPosition; //vector unitario para mantener el target de donde deberia posicionarse
    
    private float Input =0; //input del movimiento

    // booleanos para manejar la rotacion del jugador
    private Boolean isFacingUp = true;
    private Boolean isFacingDown = false;
    private Boolean isFacingLeft = false;
    private Boolean isFacingRight = false;

    [SerializeField] private float speed = 10f; //maneja la rapidez del jugador

    private Rigidbody2D enemyRB; //RigidBody del player

    private Vector2 moveInput; //recibe los inputs para mover
    private Boolean isMoving = true;

    //atributos para la lista enlazada "estelaLuz"
    private LinkedList<Vector2> estelaLuz = new LinkedList<Vector2>();
    private LinkedList<GameObject> spriteEstela = new LinkedList<GameObject>();
    private int estelaSize =30;

    System.Random random = new System.Random();

    private float combustible = 1000f;

    // Start is called before the first frame update
    void Start()
    {
        enemyRB = GetComponent<Rigidbody2D>(); //asigna el RigidBody
        moveInput = Vector2.up; //inicia con una direcion predeterminada
        StartCoroutine(GenerarInputsCadaDosSegundos());
    }

    // Update is called once per frame
    void Update()
    {

        Direccionar();
        
        combustible -= 0.2f;

        if (combustible <=0)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator GenerarInputsCadaDosSegundos()
    {
        while (true) // Loop infinito para que se ejecute continuamente
        {
            // Llama a la función que genera los inputs aleatorios
            Input = random.Next(0,4); //obtiene inputs para el movimiento en x (recibe 1)

            // Espera 2 segundos
            yield return new WaitForSeconds(0.5f); //0 arriba, 1 abajo, 2 izquierda, 3 derecha
        }
    }

    void Direccionar() //da la direccion a la que se dirige
    {
        

        if (isMoving == true)

            {

                if (Input != 0f ) //si la entrada x || y es distinta a 0 
                {
                    CalcularTargetPosition(); //calcula a donde se quiere mover
                }

                else
                {
                    targetPosition = moveInput;
                }

                //ahora se cambia la direccion dependiendo del input
                //los !isFacing... son para verificar que el personaje no se "devuelva"
                if (Input ==0  && !isFacingDown) 
                {
                    moveInput = Vector2.up; //lo gira arriba y cambia su direccion
                    flip();
                }

                else if (Input ==1 &&! isFacingUp )
                {
                    moveInput = Vector2.down; //lo mismo pero abajo
                    flip();
                }

                else if (Input ==2 &&! isFacingRight)
                {
                    moveInput = Vector2.left; //lo mismo pero izquierda
                    flip();
                }

                else if (Input==3 &&! isFacingLeft)
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
        enemyRB.MovePosition(enemyRB.position + normalizedMoveInput * speed * Time.fixedDeltaTime); //obtiene la posicion, le suma el vector multiplicado por la velocidad
        
    }

    void Limites()
    {

        if (enemyRB.position.x < 1)
        {
            isMoving = false;
        }

        if (enemyRB.position.y < 1)
        {
            isMoving = false;
        }

        if (enemyRB.position.x > 48)
        {
            isMoving = false;
        }

        if (enemyRB.position.y >48 )
        {
            isMoving = false;
        }
    }

    void GenerarEstela()
    {
        if (isMoving == true)
        { 
            // se agrega la posicion del jugador a la lista de la estela
            gridposition = enemyRB.position;
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
                    BoxCollider2D boxCollider = cube.GetComponent<BoxCollider2D>();
                    if (boxCollider == null)
                    {
                        boxCollider = cube.AddComponent<BoxCollider2D>();
                    }
                    //cambiar el color de la estela
                    Renderer cubeRenderer = cube.GetComponent<Renderer>();
                    cubeRenderer.material.color = Color.yellow;
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
                    BoxCollider2D boxCollider = cube.GetComponent<BoxCollider2D>();
                    if (boxCollider == null)
                    {
                        boxCollider = cube.AddComponent<BoxCollider2D>();
                    }
                    //cambiar el color de la estela
                    Renderer cubeRenderer = cube.GetComponent<Renderer>();
                    cubeRenderer.material.color = Color.yellow;
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
                    BoxCollider2D boxCollider = cube.GetComponent<BoxCollider2D>();
                    if (boxCollider == null)
                    {
                        boxCollider = cube.AddComponent<BoxCollider2D>();
                    }

                    //cambiar el color de la estela
                    Renderer cubeRenderer = cube.GetComponent<Renderer>();
                    cubeRenderer.material.color = Color.yellow;
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
                    BoxCollider2D boxCollider = cube.GetComponent<BoxCollider2D>();
                    if (boxCollider == null)
                    {
                        boxCollider = cube.AddComponent<BoxCollider2D>();
                    }
                    //cambiar el color de la estela
                    Renderer cubeRenderer = cube.GetComponent<Renderer>();
                    cubeRenderer.material.color = Color.yellow;
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
        if(Input == 3)
        {
            targetPosition = (Vector2)transform.position + Vector2.right;
        }

        else if (Input ==2)
        {
            targetPosition = (Vector2)transform.position + Vector2.left;
        }

        else if (Input ==0)
        {
            targetPosition = (Vector2)transform.position + Vector2.up;
        }

        else if (Input == 1)
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
        if (Input == 1 )
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 90f); //lo gira abajo
            isFacingUp = false;
            isFacingDown = true;
            isFacingLeft = false;
            isFacingRight = false;
            
            
        }
        if (Input == 0 )
        {
            transform.rotation = Quaternion.Euler(0f, 0f, -90f); //lo gira arriba
            isFacingUp = true;
            isFacingDown = false;
            isFacingLeft =false;
            isFacingRight = false;
            
        }

        if (Input == 3 )
        {
            transform.rotation = Quaternion.Euler(0f, 0f, -180f); //lo gira derecha
            isFacingRight = false;
            isFacingLeft = true;
            isFacingUp = false;
            isFacingDown = false;
            
            
        }


        if (Input == 2 )
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
            // Destruye la moto del jugador
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
