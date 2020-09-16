using UnityEditor;
using UnityEngine;

namespace aisilol
{
    namespace GUI_
    {
        public class ColorScope : GUI.Scope
        {
            public ColorScope(Color _color)
            {
                mOriginalColor = GUI.color;
                GUI.color = _color;
            }
            protected override void CloseScope() => GUI.color = mOriginalColor;

            private readonly Color mOriginalColor;
        }

        public class IndentScope : GUI.Scope
        {
            public IndentScope() => EditorGUI.indentLevel++;
            protected override void CloseScope() => EditorGUI.indentLevel--;
        }
    }
}