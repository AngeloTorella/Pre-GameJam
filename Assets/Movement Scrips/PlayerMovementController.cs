using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] private float playerMoveSpeed;
    [SerializeField] private float playerJumpForce;
    [SerializeField] private float playerWallJumpForce;

    [SerializeField] private LayerMask grounLayer;
    [SerializeField] private Transform grounSensorT;
    

    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private Transform wallSensorT;
    

    [SerializeField] private float largo = 1.0f;
    [SerializeField] private float ancho = 1.0f;

    private Rigidbody2D     rb2D;
    private Animator        animator;
    private SpriteRenderer  spriteRenderer;

    private int         direccion;
    private float       inputX;

    private bool        isWalled;
    private bool        isGrounded;
    private bool        isWallJumping;


    void Start()
    {
        this.animator = GetComponent<Animator>();
        this.rb2D = GetComponent<Rigidbody2D>();
        this.spriteRenderer = GetComponent<SpriteRenderer>();

        this.isGrounded = true;
        this.isWalled = true;
        this.isWallJumping = false;

        this.direccion = 1;

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
        if (isWallJumping)
            return;

        inputX = Input.GetAxis("Horizontal");

        //actualizar la direcion
        if (inputX > 0)
        {
            direccion = 1;
        }else if (inputX < 0)
        {
            direccion = -1;
        }

        //actualizar direccion sprite
        if (direccion == 1)
        {
            spriteRenderer.flipX = true;
        }
        else if (direccion == -1)
        {
            spriteRenderer.flipX = false;
        }

        Debug.Log(direccion);

        //Movimiento horizontal
        rb2D.velocity = new Vector2(inputX * playerMoveSpeed, rb2D.velocity.y);

        // Salto
        if (Input.GetButtonDown("Jump") )
        {
            if (isGrounded)
            {
                rb2D.AddForce(new Vector2(0.0f , playerJumpForce), ForceMode2D.Impulse);
            }
            else if (isWalled && !isGrounded)
            {
                isWallJumping = true;
                StartCoroutine(WallJump());
            }
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(grounSensorT.position, new Vector2(largo, ancho));
        Gizmos.DrawWireSphere(wallSensorT.position, 0.1f);
    }

    IEnumerator WallJump()
    {
        inputX *= -1;
        rb2D.velocity = new Vector2(playerWallJumpForce * direccion * -1, playerJumpForce);
        yield return new WaitForSeconds(0.5f);
        isWallJumping = false;
    }
}
