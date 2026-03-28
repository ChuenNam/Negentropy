#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ShowIfAttribute))]
public class ShowIfPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var showIfAttribute = (ShowIfAttribute)attribute;
        // 构造条件字段的绝对路径
        string propertyPath = property.propertyPath;
        int lastDot = propertyPath.LastIndexOf('.');
        string conditionPath = lastDot == -1 
            ? showIfAttribute.PropertyName 
            : propertyPath.Substring(0, lastDot + 1) + showIfAttribute.PropertyName;
        var propertyToCheck = property.serializedObject.FindProperty(conditionPath);
        
        if (propertyToCheck != null)
        {
            bool show = false;
            // 标志枚举序列化为整数
            if (propertyToCheck.propertyType == SerializedPropertyType.Integer ||
                propertyToCheck.propertyType == SerializedPropertyType.Enum)
            {
                int enumValue = propertyToCheck.intValue;
                int compareValue = (int)showIfAttribute.CompareValue;
                show = (enumValue & compareValue) == compareValue;
            }
            
            if (show)
            {
                EditorGUI.PropertyField(position, property, label, true);
                if (GUILayout.Button("写入信息"))
                {
                    // 获取当前选中的物体
                    var selectedObject = Selection.activeGameObject;
                    if (selectedObject != null)
                    {
                        Transform transform = selectedObject.transform;
                        switch (property.displayName)
                        {
                            case "Position":
                                property.vector3Value = transform.localPosition;
                                break;
                            case "Rotation":
                                property.quaternionValue = transform.localRotation;
                                break;
                            case "Scale":
                                property.vector3Value = transform.localScale;
                                break;
                        }
                        // 应用修改
                        property.serializedObject.ApplyModifiedProperties();
                    }
                    else
                    {
                        Debug.LogWarning("请先选中一个物体");
                    }
                }
            }
            // 如果不显示，什么都不绘制，但注意 GetPropertyHeight 会返回 0，保证布局正确
        }
        else
        {
            // 找不到条件字段时，依然绘制属性（避免丢失）
            EditorGUI.PropertyField(position, property, label, true);
            Debug.LogWarning($"ShowIf: 找不到属性 '{showIfAttribute.PropertyName}'，请检查字段名是否正确。");
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var showIfAttribute = (ShowIfAttribute)attribute;
        // 构造条件字段的绝对路径
        string propertyPath = property.propertyPath;
        int lastDot = propertyPath.LastIndexOf('.');
        string conditionPath = lastDot == -1 
            ? showIfAttribute.PropertyName 
            : propertyPath.Substring(0, lastDot + 1) + showIfAttribute.PropertyName;
        var propertyToCheck = property.serializedObject.FindProperty(conditionPath);
        
        if (propertyToCheck != null)
        {
            bool show = false;
            if (propertyToCheck.propertyType == SerializedPropertyType.Integer ||
                propertyToCheck.propertyType == SerializedPropertyType.Enum)
            {
                int enumValue = propertyToCheck.intValue;
                int compareValue = (int)showIfAttribute.CompareValue;
                show = (enumValue & compareValue) == compareValue;
            }
            
            if (show)
            {
                return EditorGUI.GetPropertyHeight(property, label, true);
            }
            return 0;
        }
        return EditorGUI.GetPropertyHeight(property, label, true);
    }
}
#endif