using UnityEditor;
using UnityEngine;

namespace SimpleLibrary
{
    public class SimpleEditor
    {

        public static float EditorLineHeight
        {
            get
            {
                return EditorGUIUtility.singleLineHeight;
            }
        }

    }
}