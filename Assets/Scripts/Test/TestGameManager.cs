using UnityEngine;
using AKIRA.Manager;

public class TestGameManager : MonoSingleton<TestGameManager> {
    private void Start() {
        ResourceCollection.Instance.Load();
    }
}