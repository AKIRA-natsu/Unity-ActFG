using System;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 节点
/// </summary>
public class Node {
    public Rect rect;
    public string title;
    public bool isDragged;
    public bool isSelected;

    public ConnectionPoint inPoint;
    public ConnectionPoint outPoint;

    public GUIStyle style;
    public GUIStyle defaultNodeStyle;
    public GUIStyle selectedNodeStyle;

    public Action<Node> OnRemoveNode;

    public Node(Vector2 position, float width, float height, GUIStyle nodeStyle, GUIStyle selectedStyle,
                GUIStyle inPointStype, GUIStyle outPointStype, Action<ConnectionPoint> OnClickInPoint, Action<ConnectionPoint> OnClickOutPoint,
                Action<Node> OnClickRemoveNode) {
        rect = new Rect(position.x, position.y, width, height);
        this.style = nodeStyle;
        inPoint  = new ConnectionPoint(this, ConnectionPointType.In, inPointStype, OnClickInPoint);
        outPoint  = new ConnectionPoint(this, ConnectionPointType.Out, outPointStype, OnClickOutPoint);

        this.defaultNodeStyle = nodeStyle;
        this.selectedNodeStyle = selectedStyle;
        OnRemoveNode = OnClickRemoveNode;
    }

    /// <summary>
    /// 拖拽
    /// </summary>
    /// <param name="delta"></param>
    public void Drag(Vector2 delta) {
        rect.position += delta;
    }

    /// <summary>
    /// 绘制
    /// </summary>
    public void Draw() {
        inPoint.Draw();
        outPoint.Draw();
        GUI.Box(rect, title, style);
    }

    /// <summary>
    /// 事件处理
    /// </summary>
    /// <param name="e"></param>
    /// <returns></returns>
    public bool ProcessEvents(Event e) {
        switch (e.type) {
            case EventType.MouseDown:
                if (e.button == 0)  {
                    if (rect.Contains(e.mousePosition)) {
                        isDragged = true;
                        GUI.changed = true;
                        isSelected = true;
                        style = selectedNodeStyle;
                    } else {
                        GUI.changed = true;
                        isSelected = false;
                        style = defaultNodeStyle;
                    }
                }

                if (e.button== 1 && isSelected && rect.Contains(e.mousePosition)) {
                    ProcessContextMenu();
                    e.Use();
                }
            break;
            case EventType.MouseUp:
                isDragged = false;
            break;
            case EventType.MouseDrag:
                if (e.button == 0 && isDragged) {
                    Drag(e.delta);
                    e.Use();
                    return true;
                }
            break;
        }

        return false;
    }

    /// <summary>
    /// 右键菜单
    /// </summary>
    private void ProcessContextMenu() {
        GenericMenu genericMenu = new GenericMenu();
        genericMenu.AddItem(new GUIContent("Remove Node"), false, OnClickRemoveNode);
        genericMenu.ShowAsContext();
    }

    /// <summary>
    /// 删除节点
    /// </summary>
    private void OnClickRemoveNode() {
        if (OnRemoveNode != null) {
            OnRemoveNode(this);
        }
    }
}