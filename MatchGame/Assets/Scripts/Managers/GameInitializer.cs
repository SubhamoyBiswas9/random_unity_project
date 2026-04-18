using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    [SerializeField] private GridSpawner gridSpawner;
    [SerializeField] private MatchHandler matchHandler;
    [SerializeField] private LevelManager levelManager;

    void Start()
    {
        gridSpawner.Spawn(matchHandler);
        matchHandler.Initialize(gridSpawner.cardPool);

        var gridConfig = gridSpawner.config;
        int totalCards = gridConfig.rows * gridConfig.cols;
        levelManager.Initialize(matchHandler, totalCards);
    }
}