using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.SceneManagement; 

/// <summary>
/// <para>检测场景缺少脚本 Missing Scripts</para>
/// <para>来源：https://blog.csdn.net/piai9568/article/details/99660290</para>
/// </summary>
public class SelectGameObjectsWithMissingScripts : Editor
{
    [MenuItem("Tools/Framework/Select GameObjects With Missing Scripts")]
    static void SelectGameObjects()
    {
        //Get the current scene and all top-level GameObjects in the scene hierarchy
        Scene currentScene = SceneManager.GetActiveScene();
        GameObject[] rootObjects = currentScene.GetRootGameObjects();
 
        List<Object> objectsWithDeadLinks = new List<Object>();
        foreach (GameObject g in rootObjects)
        {
			var trans = g.GetComponentsInChildren<Transform>();
			foreach (Transform tran in trans)
			{
				Component[] components = tran.GetComponents<Component>();
				for (int i = 0; i < components.Length; i++)
				{
					Component currentComponent = components[i];
	
					//If the component is null, that means it's a missing script!
					if (currentComponent == null)
					{
						//Add the sinner to our naughty-list
						objectsWithDeadLinks.Add(tran.gameObject);
						Selection.activeGameObject = tran.gameObject;
						Debug.Log(tran.gameObject + " has a missing script!"); //Console中输出
						break;
					}
				}

			}
            //Get all components on the GameObject, then loop through them 

        }
        if (objectsWithDeadLinks.Count > 0)
        {
            //Set the selection in the editor
            Selection.objects = objectsWithDeadLinks.ToArray();
        }
        else
        {
            Debug.Log("No GameObjects in '" + currentScene.name + "' have missing scripts!");
        }
    }
}
