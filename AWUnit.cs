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

        [OnInspectorGUI] //PropertyOrder(int.MaxValue)
        //[SerializeField]
        public List<AWWeapon> WeaponList { get; private set; }
        private AWWeapon ActiveWeapon;

        public GameObject ActionPanel;
        public GameObject WeaponPanel;

        public UIView ActionMenu;
        public UIView WeaponMenu;
        public UIButtonListener ButtonListener;

        //private List<Button> ActionButtonList;
        //private List<TextMeshPro> ActionTextList;
        //private List<Button> WeaponButtonList;
        //private List<TextMeshPro> WeaponTextList;

        private List<UIButton> ActionButtonList;
        private List<UIButton> WeaponButtonList;

        public override void Initialize()
        {
            base.Initialize();
            transform.localPosition += new Vector3(0, 0, -0.1f);
            ActiveWeapon = WeaponList[0];
            //Button[] btn = WeaponPanel.GetComponentsInChildren<Button>();
            //foreach (Button btn in ActionPanel.GetComponentsInChildren<Button>())
            //{
            //    ActionButtonList.Add(btn);
            //    ActionTextList.Add(btn.GetComponentInChildren<TextMeshPro>());
            //}
            //foreach (Button btn in WeaponPanel.GetComponentsInChildren<Button>())
            //{
            //    WeaponButtonList.Add(btn);
            //    WeaponTextList.Add(btn.GetComponentInChildren<TextMeshPro>());
            //}

            foreach (UIButton button in ActionMenu.GetComponentsInChildren<UIButton>())//need to find better way of doing action menu, since it's based on context of selection
            {
                ActionButtonList.Add(button);
            }
            foreach (UIButton button in WeaponMenu.GetComponentsInChildren<UIButton>())
            {
                WeaponButtonList.Add(button);
            }

            //WeaponButtonList = new List<Button>( new Button[5] );
            //WeaponTextList = new List<TextMeshPro>( new TextMeshPro[5] );
            //WeaponList = new List<Weapon>( new Weapon[5] ); //don't know if I need to set it to a number of spaces since it's a list, might need to for the inspector
        }

        //need to find a way to add different weapons onto each unit that can be changed on the fly (at least on the editor)
        //can't get a list of Weapons to show in the inspector

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

        public void ActionHandler(Button button)
        {

        }

        public void WeaponHandler(Unit other) //need to find where to put this... or if it's even needed at all
        {
            int distance = Cell.GetDistance(other.Cell);
            if (distance == ActiveWeapon.Range)
            {

            }
            //foreach (AWWeapon weapon in WeaponList)
            //{
            //    if (distance == weapon.Range)
            //    {
            //        SetActiveWeapon(weapon);

            //    }
            //}
        }

        public ref AWWeapon GetActiveWeapon()
        {
            return ref ActiveWeapon;
        }

        public void SetActiveWeapon(AWWeapon weapon)
        {
            AttackRange = weapon.Range;
            AttackFactor = weapon.AttackFactor;
            ActiveWeapon = WeaponList[WeaponList.IndexOf(weapon)]; //both of these work? don't know if one is better
            //ActiveWeapon = WeaponList.Find(x => x.weapon == weapon);
            if (WeaponPanel.activeInHierarchy)
            {
                HideWeaponPanel();
            }
        }

        public void FireWeapon()
        {
            //need to find a way to pause attack action for menu, then call this for an attack
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

        public void ShowActionPanel()
        {
            //for (int i = 0; i < ActionPanel.GetComponentsInChildren<Button>().Length; i++)
            //{
            //    ActionTextList[i].text = WeaponList[i].Name;
            //    ActionButtonList[i].onClick.AddListener(() => { ActionHandler(ActionButtonList[i]); });
            //}
            //ActionPanel.SetActive(true);

            for (int i = 0; i < ActionMenu.GetComponentsInChildren<UIButton>().Length; i++)
            {
                ActionButtonList[i].SetLabelText("");//need to figure out what to put here
                
            }
            ActionMenu.Show();
        }

        public void HideActionPanel()
        {
            //ActionPanel.SetActive(false);
            ActionMenu.Hide();
        }

        public void ShowWeaponPanel()
        {
            //for (int i = 0; i < WeaponList.Count; i++)
            //{
            //    WeaponTextList[i].text = WeaponList[i].Name;
            //    WeaponButtonList[i].onClick.AddListener(() => { SetActiveWeapon(WeaponList[i]); });
            //}
            //WeaponPanel.SetActive(true);

            for (int i = 0; i < WeaponMenu.GetComponentsInChildren<UIButton>().Length; i++)
            {
                WeaponButtonList[i].SetLabelText(WeaponList[i].Name);
            }
            //need to figure out how to have a listener linked to the SetActiveWeapon()
            ButtonListener = new UIButtonListener();
            //ButtonListener.SendMessage(SetActiveWeapon());
            WeaponMenu.Show();
        }

        public void HideWeaponPanel()
        {
            //WeaponPanel.SetActive(false);
            WeaponMenu.Hide();
        }

        protected override void OnMoveFinished()
        {
            GetComponent<SpriteRenderer>().sortingOrder -= 10;
            transform.Find("marker").GetComponent<SpriteRenderer>().sortingOrder -= 10;
            transform.Find("mask").GetComponent<SpriteRenderer>().sortingOrder -= 10;
            base.OnMoveFinished();
            //need to add action panel like FE
            ShowWeaponPanel(); //add weapon panel after movement is done
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
