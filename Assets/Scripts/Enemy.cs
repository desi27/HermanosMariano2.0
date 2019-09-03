﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Rigidbody2D rb;
    Animator anim;
    Player player;

    [Header("Debug, no tocar")]
    public Vector2 playerDistance;
    public Vector2 playerDistanceAbs;
    public Vector2 playerDirection;
    public bool onGround;
    //public bool onWall;
    public bool fRight = true;    

    //Con esto sabe si se va a caer si continua caminando
    public bool onRightFloor;
    public bool onLeftFloor;

    [Header("Detector de colisiones")]
    public LayerMask groundLayer;
    public LayerMask playerLayer;
    public float collisionRadius = 0.25f;
    public Vector2 bottomOffset, rightOffset, leftOffset;
    public Vector2 bottomSize;
    private Color debugCollisionColor = Color.red;

    [Space]
    public Vector2 triggerOffset, triggerSize;
    public bool ShowRange;
    public bool trigger = false;

    [Header("Vida")]
    public int lives;
    public float hitStunTimer = 1f;

    [Header("Perseguir")]
    public bool chase = true;
    public bool flee = true;
    public float speed = 10f;
    bool canChase = true;

    [Header("Ataque")]
    public int meleeDamage = 2;
    public float meleeDelay = 1;
    public int shootDamage = 1;
    public Vector2 meleeHitBoxSize;
    public Vector2 meleeHitBoxOffset;
    public bool hit = false;
    bool hited = false;
    bool canAttack = true;
    
    public bool frameDamage = false;

    IEnumerator generalActionsDelay;
    

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        var positions = new Vector2[] { bottomOffset, rightOffset, leftOffset, triggerOffset, meleeHitBoxOffset };
        var sizes = new Vector2[] { bottomSize, meleeHitBoxSize, triggerSize };

        //Colisiones
        Gizmos.DrawWireCube((Vector2)transform.position + bottomOffset, (Vector2)bottomSize);
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, collisionRadius);

        //Trigger size
        if (ShowRange)
        {
            Gizmos.DrawWireCube((Vector2)transform.position + triggerOffset, (Vector2) triggerSize);
        }

        //Melee
        Gizmos.DrawWireCube((Vector2)transform.position + meleeHitBoxOffset, (Vector2) meleeHitBoxSize);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        player = GameObject.FindObjectOfType<Player>();

        if (transform.rotation.y == 180)
        {
            fRight = false;
        }

        //que comience volteado
        if (fRight == false)
        {
            transform.Rotate(0f, 180f, 0f);

            //volteo del hitbox y trigger
            meleeHitBoxOffset.x *= -1;
            triggerOffset.x *= -1;
        }
    }

    private void FixedUpdate()
    {
        DetectCollisions();
    }

    void Update()
    {
        getPlayerDir();
        
        if (lives <= 0)
        {
            anim.SetBool("Death", true);

            StartCoroutine(DeathDelay(1f));
        }

        //si el jugador me salta por arriba, freno
        if (playerDirection.y == 1 && playerDirection.x == 0)
        {
            generalActionsDelay = ActionsDelay(0.6f);

            StopCoroutine(generalActionsDelay);
            StartCoroutine(generalActionsDelay);
        }


        //Comportamientos
        if (trigger && lives > 0)
        {
            //saltar (y frenar cuando se encuentra un precipicio) NO CAMBIAR DE LUGAR
            if(onGround && (!onLeftFloor || !onRightFloor))
            {
                generalActionsDelay = ActionsDelay(0.6f);

                if (chase)
                {
                    if ((playerDirection.x == 1 && !onRightFloor) || (playerDirection.x == -1 && !onLeftFloor))
                    {
                        //saltar

                        playerDirection.x = 0;
                    }                   
                }

                if (meleeDamage > 0)
                {
                    canAttack = false;
                }
            }

            //perseguir (el onGround puede joder el salto ¿sacarlo podria arreglarlo?)
            if (chase && canChase && onGround && playerDirection.x != 0)
            {
                rb.velocity = new Vector2(playerDirection.x * speed, rb.velocity.y);

                anim.SetBool("Run", true);
            }
            else
            {
                anim.SetBool("Run", false);
            }

            //evitar o FLEE
            

        }

        SetAnim();
    }
    
    //Sistema de deteccion de jugador
    void getPlayerDir()
    {
        float xDis = player.transform.position.x - transform.position.x;
        float yDis = player.transform.position.y - transform.position.y;

        int xDir = Mathf.FloorToInt(Mathf.Clamp(xDis, -1, 1));
        int yDir = Mathf.FloorToInt(Mathf.Clamp(yDis, -1, 1));

        playerDistance = new Vector2 (xDis, yDis);
        playerDistanceAbs = new Vector2(Mathf.Abs(xDis), Mathf.Abs(yDis));
        playerDirection = new Vector2(xDir, yDir);
    }

    void DetectCollisions()
    {
        //Colisiones con el piso:
        onGround = Physics2D.OverlapBox((Vector2)transform.position + bottomOffset, (Vector2)bottomSize, 0f, groundLayer);
        
        onRightFloor = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, collisionRadius, groundLayer);
        onLeftFloor = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, collisionRadius, groundLayer);

        //trigger
        if (!trigger)
        {
            trigger = Physics2D.OverlapBox((Vector2)transform.position + triggerOffset, (Vector2)triggerSize, 0f, playerLayer);
        }        

        //Atacks        
        Collider2D[] hitBox = Physics2D.OverlapBoxAll((Vector2)transform.position + meleeHitBoxOffset, (Vector2)meleeHitBoxSize, 0f, playerLayer);

        
        //evento de colision con el jugador

        
        if (hitBox.Length > 0 && meleeDamage >0)
        {
            //Debug.Log("Player Hit");
            hit = true;
            if (canAttack)
            {
                generalActionsDelay = ActionsDelay(meleeDelay);

                StopCoroutine(generalActionsDelay);
                StartCoroutine(generalActionsDelay);
                anim.SetTrigger("MeleeAttack");
                
                //hacer daño
                //if (frameDamage)
                //{
                //    hitBox[0].GetComponent<Player>().TakeDamage(meleeDamage, playerDirection.x);
                //}
                canAttack = false;
            }               
                               
        }
        else if (hitBox.Length == 0 && meleeDamage > 0)
        {
                hit = false;
        }

        if (hit && frameDamage && !hited && meleeDamage > 0)
        {
            player.TakeDamage(meleeDamage, playerDirection.x);
            hited = true;
        }
                      
    }

    void SetAnim()
    {
        IEnumerator Flip = DelayFlip();

        anim.SetBool("OnGround", onGround);

        if (trigger)
        {
            //voltear segun donde este el jugador
            if (playerDirection.x > 0 && !fRight)
            {
                fRight = true;

                StartCoroutine(Flip);

            }
            else if (playerDirection.x < 0 && fRight)
            {
                fRight = false;

                StartCoroutine(Flip);
            }
        }

        IEnumerator DelayFlip()
        {
            yield return new WaitForSeconds(0.5f);
            transform.Rotate(0f, 180f, 0f);

            //volteo del hitbox y trigger
            meleeHitBoxOffset.x *= -1;
            triggerOffset.x *= -1;
        }
    }

    //con que me muero
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Fall")
        {
            Destroy(gameObject);
        }
    }

    IEnumerator ActionsDelay(float delay)
    {
        bool chse = false;
        bool atk = false;

        if (canChase)
        {
            canChase = false;
            chse = true;
        }

        if (meleeDamage > 0)
        {
            canAttack = false;
            atk = true;
        }            

        yield return new WaitForSeconds(delay);

        if (chse)
        {
            canChase = true;
        }
        
        if (atk)
        {
            canAttack = true;
            hited = false;
        }
        
        //hit = false;
    }

    IEnumerator DeathDelay (float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    public void TakeDamage(int damage, float dir)
    {
        lives -= damage;
        anim.SetTrigger("GetHit");

        rb.AddForce(new Vector2(dir * 150, 200));

        generalActionsDelay = ActionsDelay(hitStunTimer);

        StopCoroutine(generalActionsDelay);
        StartCoroutine(generalActionsDelay);
    }
}
