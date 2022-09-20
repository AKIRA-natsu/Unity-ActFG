using UnityEngine;

/// <summary>
/// 要挂在Animator同一物体下
/// </summary>
public class LimbsIKOffset : MonoBehaviour {
    // 控制器
    private Animator animator;

    // 
    public HumanBodyBones bone;
    // 
    public AvatarIKGoal goal;

    // 位移偏移量
    public Vector3 bonePositionOffset;
    // 旋转偏移量
    public Vector3 boneRotationOffset;

    private void Awake() {
        animator = this.GetComponent<Animator>();
    }

    private void OnAnimatorIK(int layerIndex) {
        var b = animator.GetBoneTransform(bone);
        animator.SetIKPositionWeight(goal, 1);

        b.localPosition += bonePositionOffset;
        animator.SetIKPosition(goal, b.position);

        b.localEulerAngles += boneRotationOffset;
        animator.SetBoneLocalRotation(bone, b.localRotation);
    }
}