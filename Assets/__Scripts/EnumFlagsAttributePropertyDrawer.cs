using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR 
using UnityEditor;
#endif
using System;

// Gracias al usuario de Unity Aqibsadiq por este código:
// https://forum.unity.com/threads/multiple-enum-select-from-inspector.184729/

/// <summary>
/// Este atributo personalizado permite editar enums en el Inspector de Unity
/// utilizando una máscara de bits (MaskField), similar a cómo se editan las capas de física.
/// </summary>
public class EnumFlagsAttribute : PropertyAttribute
{
    public EnumFlagsAttribute() { }
}

#if UNITY_EDITOR 
/// <summary>
/// Clase personalizada para manejar la visualización del atributo [EnumFlags] en el Inspector de Unity.
/// </summary>
[CustomPropertyDrawer(typeof(EnumFlagsAttribute))]
public class EnumFlagsAttributePropertyDrawer : PropertyDrawer 
{
    public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label)
    {
        // Se eliminan "none" y "all" de la lista porque ya existen las opciones "Nothing" y "Everything".
        List<string> propsToShow = new List<string>(_property.enumNames);
        propsToShow.Remove("none"); // Si "none" está en la lista, se elimina.
        propsToShow.Remove("all");

        // Se muestra la MaskField en el Inspector para permitir selección múltiple de enums.
        _property.intValue = EditorGUI.MaskField(_position, _label, _property.intValue, propsToShow.ToArray());
    }
}
#endif