using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleForSpaceResources.ShipComponents
{
    public class ArmorPlate
    {
        public float armor;
        public float armorRegen;
        public float maxArmor;
        public int armorType;
        public ArmorPlate(int type, float maxArmor, float armorRegen)
        {
            armorType = type;
            this.armor = maxArmor;
            this.maxArmor = maxArmor;
            this.armorRegen = armorRegen;
        }
    }
}
