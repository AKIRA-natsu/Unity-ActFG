using UnityEngine;

public static class VecntorExtend {
    /// <summary>
    /// 夹角
    /// 来源：https://blog.csdn.net/sumkee911/article/details/53730446?spm=1001.2101.3001.6650.3&utm_medium=distribute.pc_relevant.none-task-blog-2%7Edefault%7ECTRLIST%7Edefault-3.pc_relevant_default&depth_1-utm_source=distribute.pc_relevant.none-task-blog-2%7Edefault%7ECTRLIST%7Edefault-3.pc_relevant_default&utm_relevant_index=6
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static float GetAngle(Vector3 a, Vector3 b) {
		b.x -= a.x;
		b.z -= a.z;
 
		float deltaAngle = 0;
		if (b.x == 0 && b.z == 0) {
			return 0;
		} else if (b.x > 0 && b.z > 0) {
			deltaAngle = 0;
		} else if (b.x > 0 && b.z == 0) {
			return 90;
		} else if (b.x > 0 && b.z < 0) {
			deltaAngle = 180;
		} else if (b.x == 0 && b.z < 0) {
			return 180;
		} else if (b.x < 0 && b.z < 0) {
			deltaAngle = -180;
		} else if (b.x < 0 && b.z == 0) {
			return -90;
		} else if (b.x < 0 && b.z > 0) {
			deltaAngle = 0;
		}
 
		float angle = Mathf.Atan(b.x / b.z) * Mathf.Rad2Deg + deltaAngle;
		return angle;
	}
}