using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public int maxLevel = 9;
    public int currentLevel = 0;

    public GameObject Battleship;
    public Material[] skyboxes = new Material[10];

    public BattleshipManager[] battleships;
    public int numShipsRemaining;
    public string nextSceneName = "";

    void Start()
    {
        //DontDestroyOnLoad(this.gameObject);
        
        //if(currentLevel > 9)
        //{
        //    nextSceneName = SceneManager.GetSceneByName("WinScreen").name;
        //    AdvanceToNextLevel();
        //}

        //RenderSettings.skybox = skyboxes[currentLevel];

        battleships = new BattleshipManager[currentLevel + 1];

        //battleships = GameObject.FindObjectsOfType<BattleshipManager>();
        SpawnBattleships();

        if(nextSceneName == "")
        {
            print("No next scene set, setting to self");
            nextSceneName = SceneManager.GetActiveScene().name;
        }
    }

    void SpawnBattleships()
    {

        Quaternion spawnRot = Quaternion.Euler(Random.Range(-50, 50), Random.Range(-50, 50), Random.Range(-50, 50));

        for (int i = 0; i < currentLevel + 1; i++)
        {
            bool spawned = false;
            while (!spawned)
            {
                Vector3 randomPos = new Vector3(Random.Range(-1000, 1000), Random.Range(-1000, 1000), Random.Range(-1000, 1000));
                if(!Physics.CheckSphere(randomPos, 100))
                {
                    battleships[i] = Instantiate(Battleship, randomPos, spawnRot).GetComponent<BattleshipManager>();
                    spawned = true;
                }
            }
        }
    }

    void Update()
    {
        numShipsRemaining = GetLivingBattleships();

        if(numShipsRemaining == 0)
        {
            AdvanceToNextLevel();
        }
    }

    int GetLivingBattleships()
    {
        int tally = 0;
        foreach(var ship in battleships)
        {
            if (ship.IsAlive())
            {
                tally++;
            }
        }
        return tally;
    }

    void AdvanceToNextLevel()
    {
        currentLevel++;
        SceneManager.LoadScene(nextSceneName);
    }

}
