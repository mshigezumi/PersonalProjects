using TbsFramework.Units;
using UnityEngine;
using Sirenix.OdinInspector;

namespace TbsFramework.AW
{
    [CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon", order = 1)]
    //[System.Serializable]
    public class AWWeapon : ScriptableObject, Weapon
    {
        [BoxGroup("Basic Info")]
        [LabelWidth(100)]
        public string Name;

        [BoxGroup("Basic Info")]
        [LabelWidth(100)]
        [TextArea]
        public string Description;

        //reference "How to Use Odin Inspector with Scriptable Objects?" on youtube for odin options
        [VerticalGroup("Game Data")]
        public int Ammo;
        [VerticalGroup("Game Data")]
        public int Range;
        [VerticalGroup("Game Data")]
        public bool IndirectFire;
        [VerticalGroup("Game Data")]
        public int AttackFactor;

        public AWWeapon()
        {
            Name = "New Weapon";
            Ammo = 0;
            Range = 1;
            IndirectFire = false;
            AttackFactor = 0;
        }

        public AWWeapon(string name, int ammo, int range, bool indirectFire, int attackFactor)
        {
            Name = name;
            Ammo = ammo;
            Range = range;
            IndirectFire = indirectFire;
            AttackFactor = attackFactor;
        }

        public AWWeapon weapon { get; set; }
        string Weapon.Name { get; set; }
        int Weapon.Ammo { get; set; }
        int Weapon.Range { get; set; }
        bool Weapon.IndirectFire { get; set; }
        int Weapon.AttackFactor { get; set; }

        public void UseAmmo(AWUnit unit)//Should this method be part of the unit instead? since the weapon is attached to that specific unit
        {
            for (int i = 0; i < unit.WeaponList.Count; i++)
            {
                if (unit.WeaponList[i].Name == Name)//compare something different? like an ID?
                {
                    unit.WeaponList[i].Ammo--;
                }
            }
        }

        public void Add(AWUnit unit)
        {
            unit.WeaponList.Add(weapon);
        }

        public void Remove(AWUnit unit)
        {
            unit.WeaponList.Remove(weapon);
        }

        public Weapon Clone()
        {
            return new AWWeapon(Name, Ammo, Range, IndirectFire, AttackFactor);
        }
    }
}