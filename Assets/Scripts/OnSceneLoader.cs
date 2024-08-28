using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnSceneLoader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GameOn());
    }

    IEnumerator GameOn()
    {
        yield return new WaitForSeconds(0.25f);
        GameManager.GetInstance().StartGame();
    }
}
