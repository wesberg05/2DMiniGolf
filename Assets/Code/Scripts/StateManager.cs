using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StateManager : MonoBehaviour
{
  public void LoadLevel(string levelName){
    SceneManager.LoadScene(levelName, LoadSceneMode.Single);
  }

  public void ReloadLevel(){
    SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
  }



}
