using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [System.Serializable]
    class Round
    {
        public int rows, cols;
        public int numberOfActivePlatforms;
        public bool canPlatformBeUsedManyTimes;
    }

    [Header("Game Variables")]
    [SerializeField] PlayerController player;
    [SerializeField] PlatformSpawner spawner;
    [SerializeField] UIManager UI;
    int hp;
    int roundNumber;
    int currentPlatformNumber;
    bool isGameOn;
    bool canRestart;
    [SerializeField] List<Round> rounds = new();
    [SerializeField] List<Turret> turrets = new();

    

    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("GameManager");
                go.AddComponent<GameManager>();
            }

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
        canRestart = false;
    }
    void Start()
    {
        RestartGame();
    }

    void Update()
    {
        if (!isGameOn && Input.GetKeyDown(KeyCode.S)) StartGame();
        if (canRestart && Input.GetKeyDown(KeyCode.R)) RestartGame();
    }

    public void RestartGame()
    {
        canRestart = false;
        hp = 10;
        roundNumber = 0;
        isGameOn = false;
        player.canMove = false;
        player.canRotate = false;
        UI.PanelVisibility(true);
        UI.UpdateInfo(UI.infoStartText);
        foreach(Turret t in turrets)
        {
            t.isActive = false;
        }
    }

    void StartGame()
    {
        isGameOn = true;
        UI.PanelVisibility(false);
        UI.UpdateHP(hp.ToString());
        
        player.canRotate = true;
        spawner.GenerateMap(rounds[roundNumber].rows, rounds[roundNumber].cols, rounds[roundNumber].numberOfActivePlatforms, rounds[roundNumber].canPlatformBeUsedManyTimes);
        StartCoroutine(StartRound());
    }


    IEnumerator StartRound()
    {
        Debug.Log(roundNumber);
        UI.UpdateRounds((roundNumber + 1).ToString());
        player.canMove = false;
        currentPlatformNumber = 0;
        yield return new WaitForSeconds(1f);
        List<Platform> platforms = spawner.activePlatforms;
        foreach (Turret t in turrets)
        {
            t.isActive = false;
        }
        CheckTurrets();
        for(int i = 0; i < platforms.Count; i++)
        {
            platforms[i].ChangePlatformColorTemporarily(Platform.Color.Blue, 0.75f);
            yield return new WaitForSeconds(0.75f);
        }
        
        foreach(Platform p in platforms)
        {
            p.canCheck = true;
        }
        foreach(Platform p in spawner.platforms)
        {
            p.canCheck = true;
        }
        platforms[currentPlatformNumber].isNext = true;
        player.canMove = true;
        player.canRotate = true;
    }

    public void PlatformChecked(bool correct)
    {
        if (correct)
        {
            List<Platform> platforms = spawner.activePlatforms;
            currentPlatformNumber++;
            if (currentPlatformNumber == rounds[roundNumber].numberOfActivePlatforms)
            {
                if (roundNumber >= rounds.Count-1)
                {
                    StartCoroutine(RoundWon(platforms, true));
                }
                else
                {
                    StartCoroutine(RoundWon(platforms, false)); 
                }
            }
            else
            {
                platforms[currentPlatformNumber].isNext = true;
            }
        }
        else
        {
            StartCoroutine(RoundLost());
        }
    }
    IEnumerator RoundLost()
    {
        
        List<Platform> platforms = new();
        platforms.AddRange(spawner.platforms);
        platforms.AddRange(spawner.activePlatforms);
        foreach (Platform platform in platforms)
        {
            platform.ChangePlatformColor(Platform.Color.Red);
            platform.canCheck = false;
        }
        yield return new WaitForSeconds(0.3f);
        player.StopPlayer();
        player.gameObject.transform.position = GameObject.FindGameObjectWithTag("PSpawner").GetComponent<Transform>().position;
        player.canMove = false;
        
        hp--;
        UI.UpdateHP(hp.ToString());
        if (hp == 0)
        {
            yield return new WaitForSeconds(2f);
            UI.UpdateInfo("You lost! \n Press 'R' to restart the game.");
            UI.PanelVisibility(true);
            canRestart = true;
        }
        else
        {
            yield return new WaitForSeconds(1f);
            foreach (Platform platform in platforms)
            {
                platform.ChangePlatformColor(Platform.Color.Yellow);
            }
            StartCoroutine(StartRound());
        }
    }
    IEnumerator RoundWon(List<Platform> platforms, bool gameOver)
    {
        yield return new WaitForSeconds(0.3f);
        player.StopPlayer();
        
        foreach(Platform platform in platforms)
        {
            platform.ChangePlatformColor(Platform.Color.Yellow);
        }
        player.gameObject.transform.position = GameObject.FindGameObjectWithTag("PSpawner").GetComponent<Transform>().position;
        player.canMove = false;
        
        yield return new WaitForSeconds(0.5f);
        for (int i=0;i<platforms.Count;i++)
        {
            platforms[i].ChangePlatformColor(Platform.Color.Green);
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(0.5f);
        if (gameOver)
        {
            platforms.AddRange(spawner.platforms);
            foreach (Platform platform in platforms)
            {
                platform.ChangePlatformColor(Platform.Color.Green);
            }
            yield return new WaitForSeconds(2f);
            UI.UpdateInfo("You won! \n Press 'R' to restart the game.");
            UI.PanelVisibility(true);
            canRestart = true;
        }
        else
        {
            roundNumber++;
            spawner.GenerateMap(rounds[roundNumber].rows, rounds[roundNumber].cols, rounds[roundNumber].numberOfActivePlatforms, rounds[roundNumber].canPlatformBeUsedManyTimes);
            StartCoroutine(StartRound());
        }
    }

    void CheckTurrets()
    {
        switch(roundNumber)
        {
            case 0:
            case 1:
            case 2:
            case 3:
                break;
            case 4:
                for (int i = 0; i < 2; i++)
                { turrets[i].SetTurret(true, 3, 3.5f); }
                break;
            case 5:
                for(int i=0;i<2;i++)
                { turrets[i].SetTurret(true, 3, 3); }
                break;
            case 6:
                for (int i = 0; i < 4; i++)
                { turrets[i].SetTurret(true, 4, 3); }
                break;
            case 7:
                for (int i = 0; i < 6; i++)
                { turrets[i].SetTurret(true, 5, 3); }
                break;
            case 8:
                for (int i = 0; i < 8; i++)
                { turrets[i].SetTurret(true, 5, 3); }
                break;
            case 9:
                for (int i = 0; i < 8; i++)
                { turrets[i].SetTurret(true, 5, 2); }
                break;
        }
    }
}
