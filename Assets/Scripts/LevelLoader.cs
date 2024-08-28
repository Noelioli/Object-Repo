using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    /* Old code rendered useless, I think?
    // Start is called before the first frame update
    void Start()
    {
        //Create player
        Player player = new Player();

        //Create enemies
        //Enemy enemy1 = new Enemy();
        //Enemy enemy2 = new Enemy();

        //Create weapons
        //Weapon gun1 = new Weapon();
        //Weapon gun2 = new Weapon("Assault Rifle", 50f);
       // Weapon machineGun = new Weapon();

        //Give Weapons
        //player.weapon = gun1;
       // enemy1.weapon = machineGun;
        //enemy2.weapon = gun2;
    }*/

    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject displayMenu;

    [SerializeField] private int buildIndex = 1;

    public void ChangeScene()
    {
        SceneManager.LoadScene(buildIndex);
    }

    public void CloseMenu()
    {
        mainMenu.SetActive(false);
        displayMenu.SetActive(true);
    }
}
