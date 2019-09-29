﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Collectable : MonoBehaviour
{
    Collect collect;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if(GameObject.FindObjectOfType<Collect>() != null)
            {
                collect = FindObjectOfType<Collect>();

                collect.toCollect -= 1;

                Destroy(gameObject);
            }
            else if (GameObject.FindObjectOfType<Transport>() != null)
            {
                Player player = FindObjectOfType<Player>();

                if (!player.transport)
                {
                    player.transport = true;
                    player.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = GetComponent<SpriteRenderer>().sprite;
                    Destroy(gameObject);
                }                
            }            
        }
    }
}
