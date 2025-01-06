using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public static LevelManager main;

    [Header("Refrences")]
    [SerializeField] private TextMeshProUGUI strokeUI;
    [Space(10)]
    [SerializeField] private GameObject levelCompletedUI;
    [SerializeField] private TextMeshProUGUI levelComplatedStrokeUI;
    [SerializeField] private GameObject gameOverUI;

    [Header("Attributes")]
    [SerializeField] private int maxStrokes;

    private int strokes;
    [HideInInspector] public bool outOfStrokes;
    [HideInInspector] public bool levelCompleted;

    private void Awake() {
        main = this;
    }

    private void Start() {
        UpdateStrokeUI();
    }

    public void IncreaseStroke(){
        strokes++;
        UpdateStrokeUI();

        if(strokes >= maxStrokes){
            outOfStrokes = true;
        }
    }

    public void LevelComplete(){
        levelCompleted = true;
        levelComplatedStrokeUI.text = strokes > 1 ? "You putted in " + strokes + " strokes" : "You got a hole in one!";

        levelCompletedUI.SetActive(true);
    }

    public void gameOver(){
        gameOverUI.SetActive(true);
    }

    private void UpdateStrokeUI(){
        strokeUI.text = strokes + "/" + maxStrokes;
    }

    

}