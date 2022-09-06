using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAI2 : AgentLocomotion
{
    public override void GameUpdate()
    {
        throw new System.NotImplementedException();
    }

    private void OnEnable() {
        Wake();
    }

    private void OnDisable() {
        Recycle();
    }

    private void Update() {
        if (path != null)
            path.SetPath(agent.path.corners, 1f);
    }
}
