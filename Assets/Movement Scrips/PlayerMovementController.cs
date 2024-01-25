using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] private float playerMoveSpeed;
    [SerializeField] private float playerJumpForce;

    [SerializeField] private LayerMask grounLayer;
    [SerializeField] private Transform grounSensorT;
    private bool        isGrounded;

    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private Transform wallSensorT;
    private bool        isWalled;

    [SerializeField] private float largo = 1.0f;
    [SerializeField] private float ancho = 1.0f;

    private Rigidbody2D rb2D;
    private Animator    animator;

    private float       inputX;


    void Start()
    {
        this.animator = GetComponent<Animator>();
        this.rb2D = GetComponent<Rigidbody2D>();

        this.isGrounded = true;
        this.isWalled = true;

    }

    void FixedUpdate()
    {
        //Actualiza el estado del booleano isGrouded cada frame
        isGrounded = Physics2D.OverlapBox(grounSensorT.position, new Vector3(largo, ancho, 0), 0f, grounLayer);
        Debug.Log("ground: "+isGrounded.ToString());
        //actualizar la posicion del sendor en base al input x cada frame
        wallSensorT.position = new Vector2(transform.position.x + inputX, transform.position.y);
        //actualiza el estado del booleano isWalled cada frame
        isWalled = Physics2D.OverlapCircle(wallSensorT.position, 0.1f, wallLayer);
        Debug.Log("wall: "+isWalled.ToString());
            
    }

    void Update()
    {
        inputX = Input.GetAxis("Horizontal");

        //Movimiento horizontal
        rb2D.velocity = new Vector2(inputX * playerMoveSpeed, rb2D.velocity.y);

        // Salto
        if (Input.GetButtonDown("Jump") )
        {
            if (isGrounded)
            {
                rb2D.AddForce(new Vector2(0f, playerJumpForce), ForceMode2D.Impulse);
            }
            else if (isWalled)
            {
                //cambiar posicion del personaje
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                //agrear una velocidad en direccion del personaje hacia arriba con su respectiva fuerza
                rb2D.velocity = new Vector2(-playerMoveSpeed*2, playerJumpForce);
            }
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(grounSensorT.position, new Vector2(largo, ancho));
    }
}
