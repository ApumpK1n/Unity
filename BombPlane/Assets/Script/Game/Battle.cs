using UnityEngine;
using System.Collections;
using PlayFab.ClientModels;

public class Battle : MonoBehaviour
{

    private EnemyGrid enemyGrid;
    private AllyGrid allyGrid;


    private void Awake()
    {
        enemyGrid = GetComponentInChildren<EnemyGrid>();
        allyGrid = GetComponentInChildren<AllyGrid>();
    }


    private void OnEnable()
    {
        Heartbeat.Instance.OnReceived += OnReceived;
    }

    private void OnDisable()
    {
        Heartbeat.Instance.OnReceived -= OnReceived;
    }

    private void OnReceived(PlaneBattleResponse response)
    {
        //enemyGrid.OnReceiveBattle();
        //allyGrid.OnReceiveBattle();
    }

    private void OnRound()
    {

    }

    private void OnEnd()
    {

    }


}
