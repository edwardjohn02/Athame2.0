using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using GameCreator.Runtime.Inventory.UnityUI;
using UnityEngine;

namespace GameCreator.Runtime.Inventory
{
    [Version(1, 0, 1)]

    [Title("Use Equipment")]
    [Description("Use the item equipped item in the specified slot.")]

    [Category("Inventory/Use Equipment")]

    [Parameter("Equipment", "Equipment slot to use")]
    [Parameter("Bag", "The targeted Bag component")]

    [Keywords("Use", "Activate", "Consume")]
    [Keywords("Bag", "Inventory", "Consumable")]

    [Image(typeof(IconEquipment), ColorTheme.Type.Blue, typeof(OverlayDot))]

    [Serializable]
    public class UseEquipment : Instruction
    {
        [SerializeField] private EquipmentIndex m_EquipmentIndex = new EquipmentIndex();
        [SerializeField] private PropertyGetGameObject Bag = GetGameObjectPlayer.Create();

        protected override Task Run(Args args)
        {
            var bag = this.Bag.Get<Bag>(args);
            if (bag == null) return DefaultResult;
                
            IBagContent content = bag.Content;
            IBagEquipment equipment = bag.Equipment;
            
            IdString runtimeItemID = equipment.GetSlotRootRuntimeItemID(this.m_EquipmentIndex.Index);
            if (string.IsNullOrEmpty(runtimeItemID.String)) return DefaultResult;

            Vector2Int position = content.FindPosition(runtimeItemID);
            bag.Content.Use(position);
            return DefaultResult;
        }
    }
}
