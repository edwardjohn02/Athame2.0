using GameCreator.Runtime.Inventory;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Inventory
{
    [CustomPropertyDrawer(typeof(WealthData))]
    public class WealthDataDrawer : PropertyDrawer
    {
        public const string PROP_CURRENCY = "m_Currency";
        public const string PROP_AMOUNT = "m_Amount";
        
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new VisualElement();

            root.SetEnabled(!EditorApplication.isPlayingOrWillChangePlaymode);
            
            SerializedProperty currency = property.FindPropertyRelative(PROP_CURRENCY);
            SerializedProperty amount = property.FindPropertyRelative(PROP_AMOUNT);

            var fieldCurrency = new GameCreator.Editor.Common.PropertyTool(currency);
            var fieldAmount = new GameCreator.Editor.Common.PropertyTool(amount, " ");
            
            root.Add(fieldCurrency);
            root.Add(fieldAmount);
            
            return root;
        }
    }
}