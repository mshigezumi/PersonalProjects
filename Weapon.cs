using TbsFramework.AW;

namespace TbsFramework.Units
{
    /// <summary>
    /// Class representing a weapon a unit has.
    /// </summary>
    public interface Weapon
    {
        /// <summary>
        /// Name of the weapon system.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Ammount of base ammo.
        /// </summary>
        int Ammo { get; set; }

        /// <summary>
        /// Range of the weapon.
        /// </summary>
        int Range { get; set; }

        /// <summary>
        /// If the weapon fires indirectly.
        /// </summary>
        bool IndirectFire { get; set; }

        /// <summary>
        /// Attack damage of the weapon.
        /// </summary>
        int AttackFactor { get; set; }

        //Possibly add traits to the weapon here, for display and to change traits

        /// <summary>
        /// Adds the weapon to the unit.
        /// </summary>
        void Add(AWUnit unit);

        /// <summary>
        /// Removes the weapon from the unit.
        /// </summary>
        void Remove(AWUnit unit);

        /// <summary>
        /// Returns deep copy of the Weapon object.
        /// </summary>
        Weapon Clone();
    }
}