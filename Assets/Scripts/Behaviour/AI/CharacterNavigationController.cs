using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AKIRA.AI {
    public class CharacterNavigationController : AIBase
    {
        public bool reachDestination;

        public override void GameUpdate() {
            // FIXME: 测试视野
            if (this.TryGetComponent<FieldView>(out FieldView view)) {
                if (view.ViewRayHit(out RaycastHit hit)) {
                    hit.transform.Log();
                }
            }
        }

        public override void Recycle() { }

        public override void Wake() { }

        public void SetDestination(Vector3 vector3) { }
    }
}
