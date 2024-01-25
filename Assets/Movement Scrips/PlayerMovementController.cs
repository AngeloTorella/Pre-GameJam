using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] private float playerMoveSpeed;
    [SerializeField] private float playerJumpForce;

    [SerializeField] private LayerMask grounLayer;

    [SerializeField] private Transform grounSensorT;

    [SerializeField] private float largo = 1.0f;
    [SerializeField] private float ancho = 1.0f;

    private Rigidbody2D rb2D;
    private Animator    animator;

    private float       inputX;

    private bool        isGrounded;


    void Start()
    {
        this.animator = GetComponent<Animator>();
        this.rb2D = GetComponent<Rigidbody2D>();

        this.isGrounded = true;

    }

    void FixedUpdate()
    {
        //Actualiza el estado del booleano isGrouded cada frame
        isGrounded = Physics2D.OverlapBox(grounSensorT.position, new Vector3(largo, ancho, 0), 0f, grounLayer);
        Debug.Log(isGrounded.ToString());
    }

    void Update()
    {
        inputX = Input.GetAxis("Horizontal");

        //Movimiento horizontal
        rb2D.velocity = new Vector2(inputX * playerMoveSpeed, rb2D.velocity.y);

        // Salto
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb2D.AddForce(new Vector2(0f, playerJumpForce), ForceMode2D.Impulse);
        }


    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(grounSensorT.position, new Vector2(largo, ancho));
    }
}
