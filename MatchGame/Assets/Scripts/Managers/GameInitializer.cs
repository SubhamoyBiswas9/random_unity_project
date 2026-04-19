using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    [SerializeField] GridSpawner gridSpawner;
    [SerializeField] MatchHandler matchHandler;
    [SerializeField] LevelManager levelManager;
    [SerializeField] ScoreSystem scoreSystem;
    [SerializeField] GameplayScreen gameplayScreen;

    void Start()
    {
        gridSpawner.Spawn(matchHandler);
        matchHandler.Initialize(gridSpawner.cardPool);

        var gridConfig = gridSpawner.config;
        int totalCards = gridConfig.rows * gridConfig.cols;
        levelManager.Initialize(matchHandler, totalCards);

        scoreSystem.Init(matchHandler);
        gameplayScreen.Init(scoreSystem, levelManager);
    }
}