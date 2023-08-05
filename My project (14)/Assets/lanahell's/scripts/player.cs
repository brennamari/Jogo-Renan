using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    public int health = 3;
    public float speed;
    public float jumpForce;
    
    private bool isJumping;
    private bool doubleJump;

    private Rigidbody2D rig;
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        
        GameController.instance.UpdateLives(health);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
    }

    void Move()
    {
        // se não pressionar nada o valor é 0, se pressionar direita valor máximo 1, se pressionar pra esquerda valor máximo -1
        float movement = Input.GetAxis("Horizontal");
        
        // adiciona velocidade ao corpo do personagem no eixo x e y
        rig.velocity = new Vector2(movement * speed, rig.velocity.y);
       
        //andando para direita
        if (movement > 0)
        {
            if (!isJumping)
            {
                anim.SetInteger("transition", 1);
            }
            
            transform.eulerAngles = new Vector3(0,0,0);
        }
        
        //andando para esquerda
        if (movement < 0)
        {
            if (!isJumping)
            {
                anim.SetInteger("transition", 1);
            }
          
            transform.eulerAngles = new Vector3(0,180,0);
        }

        if (movement == 0 && !isJumping)
        {
            anim.SetInteger("transition", 0);
        }
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (!isJumping)
            {
                anim.SetInteger("transition",2);
                rig.AddForce(new Vector2(0, jumpForce),ForceMode2D.Impulse);
                doubleJump = true;
                isJumping = true;
            }
            else
            {
                if (doubleJump)
                {
                    anim.SetInteger("transition", 2);
                    rig.AddForce(new Vector2(0, jumpForce * 1),ForceMode2D.Impulse);
                    doubleJump = false;
                }
            }

        }
    }

    public void Demage(int dmg)
    {
        health -= dmg;
        GameController.instance.UpdateLives(health);
        
        if(transform.rotation. y == 0)
        {
            transform.position += new Vector3(-0.5f, 0, 0);
        }
        
        if(transform.rotation. y == 180)
        {
            transform.position += new Vector3(0.5f,0, 0);

        }

        if (health <- 0 ) // chama game over
        {
           
        }
    }

    public void IncraseLife(int value)
    {
        health += value;
        GameController.instance.UpdateLives(health);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            isJumping = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Inimigo")
        {
            rig.velocity = Vector2.zero;
            rig.AddForce(Vector2.up * 4, ForceMode2D.Impulse);
            col.gameObject.GetComponent<inimigo>().enabled = false;
            col.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            col.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            col.gameObject.GetComponent<Animator>().SetBool("hit", true );
            col.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            Destroy(col.gameObject, 1f);
            
        }
        
    }

    
}