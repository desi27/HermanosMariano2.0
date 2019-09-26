﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collect : MonoBehaviour
{
    GameManager gm;
    UserInterface ui;

    public int toCollect = 0;

    public int gananciaMaxima = 100;
    public bool Obligatorio = false;

    private void Start()
    {
        gm = GameObject.FindObjectOfType<GameManager>();
        ui = GameObject.FindObjectOfType<UserInterface>();

        toCollect += FindObjectsOfType<Collectable>().Length;

        Enemy[] enemies = FindObjectsOfType<Enemy>();

        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i].dropOnHit != null)
            {
                toCollect += 1;
            }

            if (enemies[i].dropOnDeath != null )
            {
                toCollect += 1;
            }
        }
    }

    void Update()
    {
        if (toCollect <= 0)
        {
            gm.VictoryCondition();
            ui.Victory(gananciaMaxima);

            toCollect = 100;

            if (Obligatorio)
            {
                GameManager.zoneProgress += 1;
                GameManager.maxEnergy += 1;
            }
        }
    }
}
