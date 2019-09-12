﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//【◉ᴥ◉】
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    int levelIndex;

   [Header("Datos que se mantienen")]
    public int maxEnergy = 6;
    public int maxLives = 3;
    public int zoneProgress;
    public int paidDeudas = 0;
    public int ahorros;
    public bool sound;

    [Header("Datos del nivel")]
    public int resetCount = 0;
    public bool victory = false;
    public int energy = 10;
    public int lives = 3;
    public Vector2 lastCheckpos;
    public int money;

    private void Awake()
    {
        //No tocar, permite mantener al gamemanager en todas las escenas y poder probar todos los niveles sin tener que empezar desde el menu
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;

            //gameObject.SendMessage("OnLevelWasLoaded", Application.loadedLevel);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    //private void OnLevelWasLoaded(int level)
    //{
        
    //    levelIndex = SceneManager.GetActiveScene().buildIndex;

    //    if (resetCount == 0)
    //    {
    //        Debug.Log("Level loaded for the first time");
    //        lives = GameManager.instance.maxLives;
    //        energy = maxEnergy;
    //    }
        
    //    //Si hay una, Buscar condicion de victoria. Cuando gano o salgo del nivel --> resetCount = 0;

    //}

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        levelIndex = SceneManager.GetActiveScene().buildIndex;

        if (resetCount == 0)
        {
            Debug.Log("Level loaded for the first time");

            victory = false;
            lives = GameManager.instance.maxLives;
            energy = maxEnergy;
        }
        else
        {
            Debug.Log("Level loaded");
        }
    }

    void Start()
    {
        //levelIndex = SceneManager.GetActiveScene().buildIndex;
    }

    void Update()
    {
        if (energy <= 0)
        {
            lives -= 1;
            Reset();
        }        
    }    

    public void Reset()
    {
        if (lives <= 0)
        {
            resetCount = 0;
            //Game over

        }
        else
        {
            resetCount += 1;
            //SceneManager.LoadScene(levelIndex);
            Player player = GameObject.FindObjectOfType<Player>();
            player.transform.position = lastCheckpos;
        }
        energy = maxEnergy;
    }
    
    public void GainMoney(int gain)
    {
        money += gain;
    }

    public void SaveData()
    {
        PlayerPrefs.SetInt("CurrentZone", zoneProgress);
        PlayerPrefs.SetInt("Deudas Pagadas", paidDeudas);
        PlayerPrefs.SetInt("Ahorros", ahorros);
        PlayerPrefs.SetInt("MaxLives", maxLives);
        PlayerPrefs.SetInt("MaxEnergy", maxEnergy);
    }

    public void BackToMap()
    {
        energy = maxEnergy;
        lives = maxLives;
        SaveData();
        SceneManager.LoadScene("MapaZonas");        
    }

    public void VictoryCondition()
    {
        Debug.Log("Llego a la meta");
        victory = true;
        resetCount = 0;
    }
}
