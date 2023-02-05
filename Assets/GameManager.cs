using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public Objective objective;
    public Transform player;
    public int health;
    public int startingHealth = 100;

    public GameObject gameOverScreen;
    public TextMeshProUGUI timerText;

    private float startTime;
    private bool gameOver;



    public bool takeDamagetest = false;
    public int takeDamagetestAmount = 10;

    public List<Transform> GetObjectiveTransforms()
    {
        List<Transform> list = new List<Transform>();

        list.Add(objective.transform);

        return list;
    }


    void Start() {
        objective.ObjectiveTakeDamage.AddListener(OnObjectiveDamage);
        health = startingHealth;
        gameOverScreen.SetActive(false);
        startTime = Time.time;
        gameOver = false;
    }

    void OnObjectiveDamage(int damage) {
        Debug.Log("Objective took damage " + damage);
        TakeDamage(damage);
    }

    void TakeDamage(int damage) {
        health -= damage;

        if(health <= 0) {
            Time.timeScale = 0;
            gameOverScreen.SetActive(true);
            gameOver = true;
        }
    }

    void Update() {
        if(takeDamagetest) { // FOR TEST PURPOSES ONLY
            takeDamagetest = false;
            TakeDamage(takeDamagetestAmount);
        }

        timerText.text = GetTimerText();



        if(gameOver && Keyboard.current.gKey.wasPressedThisFrame) {
            LoadFirstScene();
        }
    }

    string GetTimerText() {
        float currentTime = Time.time-startTime;

        return (currentTime/60).ToString("F0") + "m" + (currentTime%60).ToString("F2");
    }

    public void LoadFirstScene() {
        SceneManager.LoadScene(0);
    }


}
