using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Test {
    public int id;
    public string name;

    public Test() {}
    public Test(int id, string name) {
        this.id = id;
        this.name = name;
    }
}

[System.Serializable]
public class TestWarp {
    public List<Test> tests = new List<Test>();

    public TestWarp() {
        tests.Add(new Test(1, "哈哈哈"));
        tests.Add(new Test(2, "呃呃呃"));
        tests.Add(new Test(3, "钱钱钱"));
    }
}

public class JsonTest : MonoBehaviour, IJson
{
    public string Path => Application.streamingAssetsPath + "/TestWarp.Json";

    public object Data { get => testWarp; set => testWarp = (TestWarp)value; }

    TestWarp testWarp;

    void Start()
    {
        testWarp = new TestWarp();
        this.Save();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
