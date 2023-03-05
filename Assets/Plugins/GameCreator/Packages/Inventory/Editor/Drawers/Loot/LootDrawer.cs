using System;
using GameCreator.Editor.Common;
using GameCreator.Runtime.Inventory;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameCreator.Editor.Inventory
{
    [CustomPropertyDrawer(typeof(Loot))]
    public class LootDrawer : PropertyDrawer
    {
        internal const string PROP_RATE = "m_Rate";
        internal const string PROP_TYPE = "m_LootType";
        internal const string PROP_ITEM = "m_Item";
        internal const string PROP_CURRENCY = "m_Currency";
        internal const string PROP_AMOUNT = "m_Amount";
        internal const string PROP_AMOUNT_CONSTANT = "m_AmountConstant";
        internal const string PROP_AMOUNT_MIN = "m_AmountMinimum";
        internal const string PROP_AMOUNT_MAX = "m_AmountMaximum";
        
        private const string LABEL_LOOT_TYPE = "Loot";
        private const string LABEL_LOOT_EMPTY = " ";

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new VisualElement();
            VisualElement amountContent = new VisualElement();
            
            SerializedProperty rate = property.FindPropertyRelative(PROP_RATE);
            SerializedProperty type = property.FindPropertyRelative(PROP_TYPE);
            SerializedProperty item = property.FindPropertyRelative(PROP_ITEM);
            SerializedProperty currency = property.FindPropertyRelative(PROP_CURRENCY);
            SerializedProperty amount = property.FindPropertyRelative(PROP_AMOUNT);

            var fieldRate = new GameCreator.Editor.Common.PropertyTool(rate);
            var fieldType = new GameCreator.Editor.Common.PropertyTool(type, LABEL_LOOT_TYPE);
            var fieldItem = new GameCreator.Editor.Common.PropertyTool(item, LABEL_LOOT_EMPTY);
            var fieldCurrency = new GameCreator.Editor.Common.PropertyTool(currency, LABEL_LOOT_EMPTY);
            var fieldAmount = new GameCreator.Editor.Common.PropertyTool(amount);

            root.Add(fieldRate);
            root.Add(new SpaceSmall());
            root.Add(fieldType);
            root.Add(fieldItem);
            root.Add(fieldCurrency);
            root.Add(new SpaceSmall());
            root.Add(fieldAmount);
            root.Add(amountContent);
            
            fieldType.EventChange += changeEvent =>
            {
                fieldItem.style.display = changeEvent.changedProperty.enumValueIndex == 0
                    ? DisplayStyle.Flex
                    : DisplayStyle.None;
                
                fieldCurrency.style.display = changeEvent.changedProperty.enumValueIndex == 1
                    ? DisplayStyle.Flex
                    : DisplayStyle.None;
            };
            
            fieldItem.style.display = type.enumValueIndex == 0
                ? DisplayStyle.Flex
                : DisplayStyle.None;
                
            fieldCurrency.style.display = type.enumValueIndex == 1
                ? DisplayStyle.Flex
                : DisplayStyle.None;

            fieldRate.EventChange += changeEvent =>
            {
                int value = Mathf.Max(changeEvent.changedProperty.intValue, 0);
                rate.intValue = value;
                
                rate.serializedObject.ApplyModifiedProperties();
                rate.serializedObject.Update();
            };

            RefreshAmount(amountContent, property);
            fieldAmount.EventChange += _ =>
            {
                RefreshAmount(amountContent, property);
            };

            return root;
        }

        private static void RefreshAmount(VisualElement content, SerializedProperty property)
        {
            SerializedProperty amount = property.FindPropertyRelative(PROP_AMOUNT);
            SerializedProperty amountConstant = property.FindPropertyRelative(PROP_AMOUNT_CONSTANT);
            SerializedProperty amountMin = property.FindPropertyRelative(PROP_AMOUNT_MIN);
            SerializedProperty amountMax = property.FindPropertyRelative(PROP_AMOUNT_MAX);
            
            content.Clear();

            switch (amount.intValue)
            {
                case 0: // Constant
                    var fieldConstant = new GameCreator.Editor.Common.PropertyTool(amountConstant, LABEL_LOOT_EMPTY);
                    fieldConstant.Bind(property.serializedObject);
                    content.Add(fieldConstant);
                    break;
                
                case 1: // Range
                    var fieldMin = new GameCreator.Editor.Common.PropertyTool(amountMin, "  Min");
                    var fieldMax = new GameCreator.Editor.Common.PropertyTool(amountMax, "  Max");
                    content.Add(fieldMin);
                    content.Add(fieldMax);
                    break;
                
                default: throw new ArgumentOutOfRangeException();
            }
            
            content.Bind(property.serializedObject);
        }
    }
}