using UnityEngine;
using System;
using System.Collections;
using PlayFab;
using PlayFab.ClientModels;

public class Heartbeat : SingletonBehaviour<Heartbeat>
{

    public Action<PlaneBattleResponse> OnReceived;

    private float interval = 1f;

    private float timer = 1f;
    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 1f)
        {
            timer = 0;
            //net
            PlaneBattleRequest request = new PlaneBattleRequest();
            PlayFabClientAPI.RequestBattleData(request, OnReceivedBattleData, null);
        }
    }

    private void OnReceivedBattleData(PlaneBattleResponse response)
    {
        if (response == null) return;
        OnReceived.Invoke(response);
    }
}