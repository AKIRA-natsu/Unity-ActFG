using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour, IPool
{
    public void Recycle()
    {
        $"Cube被回收".Log();
        this.Delay(() => {
            $"回收后延迟触发的东西".Log();
            this.Repeat((i) => {
                $"这是循环执行第{i}次".Log();
            }, 3).End(() => {
                $"这是循环结束事件".Log();
            });
        }, 3f).End(() => {
            $"这是延迟结束事件".Log();
        });
    }

    public void Wake()
    {
        $"Cube被创建".Log();
    }
}
