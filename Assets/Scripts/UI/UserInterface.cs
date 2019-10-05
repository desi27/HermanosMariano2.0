﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour
{
    GameManager gm;
    Player player;

    CountDown countDown;
    Survive survive;

    public Text LivesText;
    public GameObject EnergyBorder;
    public Image[] EnergyBars;
    public Text MoneyText;
    public Text TimerText;
    //public Sprite EnergyComplete;
    //public Sprite EnergyEmpty;

    public GameObject GameOverScreen;
    public GameObject VictoryScreen;
    public Text ganancias;
    public Text impuestos;
    public Text total;
    public Text bullets;
    public MenuPausa menuPausa;

    public DialogueManager dialogueManager;
    public NewDialogueManager newDialogueManager;

    [HideInInspector]
    public int gananciaMaxBK;

    private void Awake()
    {
        menuPausa = GetComponent<MenuPausa>();

    }

    void Start()
    {
        gm = GameManager.instance;

        player = FindObjectOfType<Player>();
        GameOverScreen.SetActive(false);

        if (FindObjectOfType<Survive>())
        {
            TimerText.gameObject.SetActive(true);
            survive = FindObjectOfType<Survive>();
        }
        else if (FindObjectOfType<CountDown>().enabled)
        {
            TimerText.gameObject.SetActive(true);
            countDown = FindObjectOfType<CountDown>();
        }
        else
        {
            TimerText.gameObject.SetActive(false);
        }

        for (int i = 0; i < EnergyBars.Length; i++)
        {
            if (i < GameManager.maxEnergy)
            {
                EnergyBars[i].gameObject.SetActive(true);
            }
            else
            {
                EnergyBars[i].gameObject.SetActive(false);
            }
        }

        RectTransform rt = EnergyBorder.GetComponent<RectTransform>();

        //rt.sizeDelta = new Vector2(rt.sizeDelta.x / 2, rt.sizeDelta.y );
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (rt.rect.size.x / 10f) * GameManager.maxEnergy);

    }

    // Update is called once per frame
    void Update()
    {        
        LivesText.text = "" + GameManager.instance.lives;
        MoneyText.text = "" + GameManager.instance.money;
        bullets.text = "" + player.bullets;

        if (survive != null)
        {
            TimerText.text = (survive.countDown / 60 - 1).ToString("00") + ":" + (survive.countDown % 60).ToString("00");
        }
        else if (countDown != null)
        {
            TimerText.text = "" + (countDown.currentTime / 60 - 1).ToString("00") + ":" + (countDown.currentTime % 60).ToString("00"); ;
        }            

        if (gm.lives == 0)
        {
            Time.timeScale = 0f;
            GameOverScreen.SetActive(true);
        }

        for (int i = 0; i < EnergyBars.Length; i++)
        {
            if (i < GameManager.instance.energy)
            {
                EnergyBars[i].enabled = true;

                if (GameManager.instance.energy <= GameManager.maxEnergy / 4)
                {
                    EnergyBars[i].color = Color.red;
                }
                else if (GameManager.instance.energy <= GameManager.maxEnergy / 2)
                {
                    EnergyBars[i].color = Color.yellow;
                }
                else
                {
                    EnergyBars[i].color = Color.green;
                }
            }
            else
            {
                EnergyBars[i].enabled = false;
            }            
        }
    }

    public void BackToMap()
    {        
        GameManager.instance.BackToMap();
    }

    public void Victory(int gananciaMaxima)
    {
        menuPausa.enabled = false;
        int impuesto;

        //bool canVictory = false;

        Time.timeScale = 0f;
        GameManager.instance.VictoryCondition();

        //if(dialogueManager != null)
        //{
        //    if (dialogueManager.DialoguesEnd.Length > 0 && !dialogueManager.end)
        //    {
        //        gananciaMaxBK = gananciaMaxima;

        //        dialogueManager.gameObject.SetActive(true);

        //        return;
        //    }
        //    else
        //    {
        //        canVictory = true;                
        //    }
        //}

        //if (newDialogueManager != null)
        //{
        //    if (newDialogueManager.EndCutscene.Dialogues.Length > 0 && !newDialogueManager.end)
        //    {
        //        gananciaMaxBK = gananciaMaxima;

        //        newDialogueManager.gameObject.SetActive(true);

        //        return;
        //    }
        //    else
        //    {
        //        canVictory = true;
        //    }
        //}        

        //if (canVictory)
        {
            VictoryScreen.SetActive(true);
            ganancias.text = "" + GameManager.instance.money;

            Debug.Log(GameManager.instance.money);


            if (GameManager.instance.money > gananciaMaxima)
            {
                impuesto = GameManager.instance.money - gananciaMaxima;
            }
            else if (GameManager.instance.money == gananciaMaxima)
            {
                impuesto = ((gananciaMaxima * 2) / 100);
            }
            else
            {
                impuesto = ((gananciaMaxima * 10) / 100);
            }

            impuestos.text = "" + impuesto;
            Debug.Log(impuesto);
            total.text = "" + (GameManager.instance.money - impuesto);
            GameManager.ahorros += (GameManager.instance.money - impuesto);
        }        
    }
    
}
