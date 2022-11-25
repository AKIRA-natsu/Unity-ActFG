using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flock : AIBase
{

    public float speed = 0.0001f;
    float rotationSpeed = 4.0f;
    public float neighbourDistance = 10.0f;

    bool turning = false;

    // Use this for initialization
    void Start() {
        speed = Random.Range(0.5f, 1);
        this.Regist();
    }

    // Update is called once per frame
    public override void GameUpdate() {
        if (Vector3.Distance(transform.position, Vector3.zero) >= team.count)
            turning = true;
        else
            turning = false;

        if (turning) {
            Vector3 direction = Vector3.zero - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
            speed = Random.Range(0.5f, 1);
        } else {
            if (Random.Range(0, 5) < 1)
                ApplyRules();
        }
        transform.Translate(0, 0, Time.deltaTime * speed);
    }

    void ApplyRules() {
        var gos = AIGroupManager.GetGroup(groupTag).GetTeam(ID);

        Vector3 vcentre = Vector3.zero;
        Vector3 vavoid = Vector3.zero;
        float gSpeed = 0.1f;

        float dist;

        int groupSize = 0;
        foreach (var go in gos)
        {
            if (go != this.gameObject)
            {
                dist = Vector3.Distance(go.transform.localPosition, this.transform.localPosition);
                if (dist <= neighbourDistance)
                {
                    vcentre += go.transform.localPosition;
                    groupSize++;

                    if (dist < 1.0f)
                    {
                        vavoid = vavoid + (this.transform.localPosition - go.transform.localPosition);
                    }

                    flock anothrtFlock = go.GetComponent<flock>();
                    gSpeed = gSpeed + anothrtFlock.speed;
                }
            }
        }

        if (groupSize > 0)
        {
            vcentre = vcentre / groupSize + (team.affectPosition - this.transform.localPosition);
            speed = gSpeed / groupSize;

            Vector3 direction = (vcentre + vavoid) - transform.localPosition;
            if (direction != Vector3.zero)
                transform.rotation = Quaternion.Slerp(transform.rotation,
                                                         Quaternion.LookRotation(direction),
                                                         rotationSpeed * Time.deltaTime);
        }
    }
}