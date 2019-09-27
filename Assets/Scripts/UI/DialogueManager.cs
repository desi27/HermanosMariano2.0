﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    GameManager gm;
    UserInterface ui;

    [Header("Start")]
    public Sprite charAstart;
    public Sprite charBstart;

    public DialogueClass[] Dialogues;

    [Header("End")]
    public Sprite charAstart2;
    public Sprite charBstart2;

    public DialogueClass[] DialoguesEnd;

    [Header("Referencias")]
    public Text Speaker;
    public Text dialogueText;
    public Image charA;
    public Image charB;
    Image background;
    public Text SpaceoZ;
    public Text ESC;

    int index = 0;

    [HideInInspector]
    public bool end = false;


    private void Awake()
    {
        if (Dialogues.Length == 0)
        {
            gameObject.SetActive(false);
        }
        else
        {
            Time.timeScale = 0;

            background = GetComponent<Image>();

            charA.sprite = charAstart;
            charB.sprite = charBstart;
        }

        
    }
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        ui = FindObjectOfType<UserInterface>();
        if(Dialogues.Length > 0)
        {
            ui.menuPausa.enabled = false;
        }        
    }
    
    void Update()
    {
        if(Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.Space))
        {
            index += 1;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 1;
            ui.menuPausa.enabled = true;
            gameObject.SetActive(false);
        }

        if (!gm.victory)
        {
            if (index < Dialogues.Length)
            {
                if(Dialogues[index].background != null)
                {
                    background.sprite = Dialogues[index].background;
                }                
                
                if(Dialogues[index].Speaker != null)
                {
                    Speaker.text = Dialogues[index].Speaker;
                }
                else
                {
                    Speaker.text = Dialogues[index - 2].Speaker;
                }

                dialogueText.text = Dialogues[index].Dialogue;
                
                if (Dialogues[index].character == DialogueClass.PosibleCharacters.CharA)
                {
                    charA.sprite = Dialogues[index].CharArt;
                    charA.color = new Color(1, 1, 1, 1f);

                    charB.color = new Color(1, 1, 1, 0.5f);
                }
                else
                {
                    charB.sprite = Dialogues[index].CharArt;
                    charB.color = new Color(1, 1, 1, 1f);

                    charA.color = new Color(1, 1, 1, 0.5f);
                }
            }
            else
            {
                Time.timeScale = 1;
                index = 0;
                ui.menuPausa.enabled = true;

                gameObject.SetActive(false);
            }
        }
        else if (gm.victory)
        {
            if (index < DialoguesEnd.Length && DialoguesEnd.Length > 0)
            {
                if (DialoguesEnd[index].background != null)
                {
                    background.sprite = DialoguesEnd[index].background;
                }

                if (DialoguesEnd[index].Speaker != "")
                {
                    Speaker.text = DialoguesEnd[index].Speaker;
                }
                else
                {
                    Speaker.text = DialoguesEnd[index - 2].Speaker;
                }

                dialogueText.text = DialoguesEnd[index].Dialogue;

                if (DialoguesEnd[index].character == DialogueClass.PosibleCharacters.CharA)
                {
                    charA.sprite = DialoguesEnd[index].CharArt;
                    charA.color = new Color(1, 1, 1, 1f);

                    charB.color = new Color(1, 1, 1, 0.5f);
                }
                else
                {
                    charB.sprite = DialoguesEnd[index].CharArt;
                    charB.color = new Color(1, 1, 1, 1f);

                    charA.color = new Color(1, 1, 1, 0.5f);
                }
            }
            else
            {
                end = true;
                Debug.Log("Ended end dialogue");
                Time.timeScale = 1;
                ui.Victory(ui.gananciaMaxBK);
                gameObject.SetActive(false);
            }
        }
    }
}
