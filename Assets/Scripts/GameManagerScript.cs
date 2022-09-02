using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    public static GameManagerScript instance; 
    public GameObject LoadingScreen;
    private int scene;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        SceneManager.LoadSceneAsync((int)SceneList.Menu, LoadSceneMode.Additive);
    }

    public void LoadGame(SceneList Scene)
    {
        List<AsyncOperation> LoadingScenes = new List<AsyncOperation>();
        LoadingScreen.SetActive(true);

        SceneManager.UnloadSceneAsync((int)SceneList.Menu);
        LoadingScenes.Add(SceneManager.LoadSceneAsync((int)Scene, LoadSceneMode.Additive));
        scene = (int)Scene;
        StartCoroutine(GetSceneLoadProgress(LoadingScenes));
    }

    public IEnumerator GetSceneLoadProgress(List<AsyncOperation> LoadingScenes)
    {
        List<GameObject> rootGameObjects = new List<GameObject>();
        foreach (var Scene in LoadingScenes)
        {
            while(!Scene.isDone)
            {
                Debug.Log("Not Loaded");
                yield return null;
            }
        }
        LoadingScreen.SetActive(false);
        Scene activeScene = SceneManager.GetActiveScene();
        activeScene.GetRootGameObjects(rootGameObjects);
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(scene));
        foreach (GameObject item in rootGameObjects)
        {
            SceneManager.MoveGameObjectToScene(item, SceneManager.GetActiveScene());
        }
    }

}
