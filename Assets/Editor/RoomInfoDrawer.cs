using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomPropertyDrawer(typeof(RoomInfo))]
public class RoomInfoDrawer : PropertyDrawer
{
    // 计算绘制所需的总高度：4行（roomName, position, rotation, 按钮）
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float lineHeight = EditorGUIUtility.singleLineHeight;
        float spacing = EditorGUIUtility.standardVerticalSpacing;
        return (lineHeight + spacing) * 4;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // 获取子属性
        SerializedProperty roomNameProp = property.FindPropertyRelative("roomName");
        SerializedProperty positionProp = property.FindPropertyRelative("position");
        SerializedProperty rotationProp = property.FindPropertyRelative("rotation");

        // 计算各行的 Rect
        float lineHeight = EditorGUIUtility.singleLineHeight;
        float spacing = EditorGUIUtility.standardVerticalSpacing;
        Rect rect = new Rect(position.x, position.y, position.width, lineHeight);

        // 绘制 roomName 字段
        EditorGUI.PropertyField(rect, roomNameProp);
        rect.y += lineHeight + spacing;

        // 绘制 position 字段
        EditorGUI.PropertyField(rect, positionProp);
        rect.y += lineHeight + spacing;

        // 绘制 rotation 字段
        EditorGUI.PropertyField(rect, rotationProp);
        rect.y += lineHeight + spacing;

        // 绘制按钮
        Rect buttonRect = new Rect(rect.x, rect.y, rect.width, lineHeight);
        if (GUI.Button(buttonRect, "从当前选中的物体获取位置/旋转"))
        {
            // 获取当前选中的游戏对象
            if (Selection.activeGameObject != null)
            {
                Transform selectedTransform = Selection.activeGameObject.transform;
                if (selectedTransform != null)
                {
                    // 记录撤销操作
                    Undo.RecordObject(property.serializedObject.targetObject, "Update RoomInfo Transform");

                    // 写入位置和旋转信息
                    positionProp.vector3Value = selectedTransform.position;
                    rotationProp.quaternionValue = selectedTransform.rotation;

                    // 应用修改并标记为已修改
                    property.serializedObject.ApplyModifiedProperties();
                    EditorUtility.SetDirty(property.serializedObject.targetObject);

                    Debug.Log($"已将从物体 '{selectedTransform.name}' 获取的位置/旋转写入房间 '{roomNameProp.stringValue}'");
                }
                else
                {
                    Debug.LogWarning("选中的物体没有 Transform 组件");
                }
            }
            else
            {
                Debug.LogWarning("请先在场景中选中一个物体，然后点击此按钮");
                EditorUtility.DisplayDialog("提示", "请先在场景中选中一个物体，然后点击此按钮。", "确定");
            }
        }
    }
}