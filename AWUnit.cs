using System.Collections;
using System.Collections.Generic;
using TbsFramework.Cells;
using TbsFramework.Units;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using TMPro;
using Doozy.Engine.UI;

namespace TbsFramework.AW
{
    public class AWUnit : Unit
    {
        public string UnitName;

        private readonly MenuManager MenuManagerIn = MenuManager.Instance;

        [OnInspectorGUI]
        public List<AWWeapon> WeaponList { get; private set; }
        private AWWeapon ActiveWeapon;

        public GameObject ActionPanel;
        public GameObject WeaponPanel;

        public UIView ActionMenu;
        public UIView WeaponMenu;
        public UIButtonListener ButtonListener;

        private List<UIButton> ActionButtonList;
        private List<UIButton> WeaponButtonList;

        public override void Initialize()
        {
            base.Initialize();
            transform.localPosition += new Vector3(0, 0, -0.1f);
            ActiveWeapon = WeaponList[0];
        }

        //need to find a way to add different weapons onto each unit that can be changed on the fly (at least on the editor)

        public void AddWeapons() //probably need to add this to a class based off this genaric unit, unless I find a way to add any weapon to this class
        {
            AWWeapon mg1 = new AWWeapon("HMG", 10, 1, false, 5);
            mg1.Add(this);
            //WeaponList.Add(mg1);
        }

        public override void MarkAsAttacking(Unit other)
        {
            StartCoroutine(AttackAnimation(other));
        }

        public override void MarkAsDefending(Unit other)
        {
            StartCoroutine(DefenceAnimation());
        }

        public override void MarkAsDestroyed()
        {
        }

        public override void MarkAsFinished()
        {
            SetColor(new Color(0.5f, 0.5f, 0.5f, 0.5f));
        }

        public override void MarkAsFriendly()
        {
            SetColor(new Color(0, 1, 0, 0.25f));
        }

        public override void MarkAsReachableEnemy()
        {
            SetColor(new Color(1, 0, 0, 0.5f));
        }

        public override void MarkAsSelected()
        {
        }

        public override void UnMark()
        {
            SetColor(new Color(0, 0, 0, 0));
        }

        public ref AWWeapon GetActiveWeapon()
        {
            return ref ActiveWeapon;
        }

        public void SetActiveWeapon(AWWeapon weapon)
        {
            AttackRange = weapon.Range;
            AttackFactor = weapon.AttackFactor;
            ActiveWeapon = WeaponList[WeaponList.IndexOf(weapon)]; //both of these work don't know if one is better
            //ActiveWeapon = WeaponList.Find(x => x.weapon == weapon);
            if (WeaponPanel.activeInHierarchy)
            {
                HideWeaponPanel();
            }
        }

        public void FireWeapon()
        {
            //pause attack action for menu, then call this for an attack
        }

        public void UseAmmo()
        {
            ActiveWeapon.Ammo--;
        }

        public override void OnUnitSelected()
        {
            base.OnUnitSelected();
            MenuManagerIn.SetSelectedUnit(this);//replace this with the UnitSelected event already used by TbsFramework?
        }
        //Need to find a way to select a weapon and use that, additionally call to reduce that ammo by 1
        //DealDamage only indicates damage to deal and action points taken
        protected override AttackAction DealDamage(Unit other)
        {
            int distance = Cell.GetDistance(other.Cell);
            if (distance == 1)
            {
                UseAmmo();
                return new AttackAction(AttackFactor, 1);
            }
            else
            {
                UseAmmo();
                return new AttackAction(AttackFactor, 1);
            }
        }

        //Damage calcuation goes here
        protected override int Defend(Unit other, int damage)
        {
            return damage - (Cell as AWSquare).DefenceBoost;
        }

        public override void Move(Cell destinationCell, List<Cell> path)
        {
            GetComponent<SpriteRenderer>().sortingOrder += 10;
            transform.Find("marker").GetComponent<SpriteRenderer>().sortingOrder += 10;
            transform.Find("mask").GetComponent<SpriteRenderer>().sortingOrder += 10;
            base.Move(destinationCell, path);
        }

        protected override void OnMoveFinished()
        {
            GetComponent<SpriteRenderer>().sortingOrder -= 10;
            transform.Find("marker").GetComponent<SpriteRenderer>().sortingOrder -= 10;
            transform.Find("mask").GetComponent<SpriteRenderer>().sortingOrder -= 10;
            base.OnMoveFinished();
            //need to add action panel like FE
            MenuManagerIn.ShowWeaponMenu(); //add weapon panel after movement is done
        }

        public override bool IsCellTraversable(Cell cell)
        {
            return base.IsCellTraversable(cell) || (cell.CurrentUnit != null && cell.CurrentUnit.PlayerNumber == PlayerNumber);
        }

        private void SetColor(Color color)
        {
            var highlighter = transform.Find("marker");
            var spriteRenderer = highlighter.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.color = color;
            }
        }

        private IEnumerator AttackAnimation(Unit other)
        {
            var heading = other.transform.position - transform.position;
            var direction = heading / heading.magnitude;
            float startTime = Time.time;

            while (startTime + 0.25f > Time.time)
            {
                transform.position = Vector3.Lerp(transform.position, transform.position + (direction / 5f), ((startTime + 0.25f) - Time.time));
                yield return 0;
            }
            startTime = Time.time;
            while (startTime + 0.25f > Time.time)
            {
                transform.position = Vector3.Lerp(transform.position, transform.position - (direction / 5f), ((startTime + 0.25f) - Time.time));
                yield return 0;
            }
            transform.position = Cell.transform.position + new Vector3(0, 0, -0.1f);
        }

        private IEnumerator DefenceAnimation()
        {
            var rnd = new System.Random();

            for (int i = 0; i < 5; i++)
            {
                var heading = new Vector3((float)rnd.NextDouble() - 0.5f, (float)rnd.NextDouble() - 0.5f, 0);
                var direction = heading / heading.magnitude;
                float startTime = Time.time;

                while (startTime + 0.05f > Time.time)
                {
                    transform.position = Vector3.Lerp(transform.position, transform.position + direction, ((startTime + 0.05f) - Time.time));
                    yield return 0;
                }
                startTime = Time.time;
                while (startTime + 0.05f > Time.time)
                {
                    transform.position = Vector3.Lerp(transform.position, transform.position - direction, ((startTime + 0.05f) - Time.time));
                    yield return 0;
                }
                transform.position = Cell.transform.position + new Vector3(0, 0, -0.1f);
            }
        }
    }
}
