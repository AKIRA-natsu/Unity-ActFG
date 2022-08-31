using UnityEngine;

public class TestAI : AgentLocomotion
{
    public override void GameUpdate()
    {
        throw new System.NotImplementedException();
    }

    public override void Recycle()
    {
        base.Recycle();
    }

    public override void Wake()
    {
        base.Wake();
    }

    private void OnEnable() {
        Wake();
    }

    private void OnDisable() {
        Recycle();
    }

    void Update()
    {
        //鼠标左键点击
        if (Input.GetMouseButtonDown(0))
        {
            //摄像机到点击位置的的射线
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                //点击位置坐标
                Vector3 point = hit.point;
                //转向
                transform.LookAt(new Vector3(point.x, transform.position.y, point.z));
                //设置寻路的目标点
                agent.SetDestination(point);
            }
        }

        // SetDestination(GameObject.FindWithTag("Player").transform.position);

        if (path != null)
            path.SetPath(agent.path.corners, 1f);
    }
}