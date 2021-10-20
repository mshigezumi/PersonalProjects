using Doozy.Engine.UI;
using System.Collections;
using System.Collections.Generic;
using TbsFramework.AW;
using TbsFramework.Units;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    //Implementation of singleton pattern with auto create and auto delete (if there is more than one)
    private static MenuManager _instance;
    public static MenuManager Instance
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            _instance.Initialize();
            //DontDestroyOnLoad(this.gameObject);//Use this if the singleton is to presist across scenes
        }
    }
    
    //This should allow a seperate instance of this singleton to be used on each scene
    //Get rid of this if I want the same instance to be used
    private void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }

    public AWUnit SelectedUnit;
    public UIView ActionMenu;
    public UIView WeaponMenu;
    public UIButtonListener ButtonListener;

    private List<UIButton> ActionButtonList = new List<UIButton>();
    private List<UIButton> WeaponButtonList = new List<UIButton>();

    public void Initialize()
    {
        GameObject[] go = GameObject.FindGameObjectsWithTag("UI");
        foreach (GameObject obj in go)
        {
            if (obj.name == "View - Action Menu" && ActionMenu == null)
            {
                ActionMenu = obj.GetComponent<UIView>();
            }
            if (obj.name == "View - Weapon Menu" && WeaponMenu == null)
            {
                WeaponMenu = obj.GetComponent<UIView>();
            }
        }
        foreach (UIButton button in ActionMenu.GetComponentsInChildren<UIButton>())//need to find better way of doing action menu, since it's based on context of selection
        {
            ActionButtonList.Add(button);//have to add all possible actions buttons needed
        }
        foreach (UIButton button in WeaponMenu.GetComponentsInChildren<UIButton>())
        {
            WeaponButtonList.Add(button);
        }
    }

    public void SetSelectedUnit(AWUnit unit)//replace this with the UnitSelected event already used by TbsFramework?
    {
        SelectedUnit = unit;
    }

    public void ActionHandler(UIButton button)
    {
        if (button.TextMeshProLabel.text == "")//Use this to check which action is called... or use Game Event?
        {

        }
    }

    public void WeaponHandler(int number)
    {
        SelectedUnit.SetActiveWeapon(SelectedUnit.WeaponList[number]);
        SelectedUnit.FireWeapon();
        //int distance = Cell.GetDistance(other.Cell);
        //if (distance == SelectedUnit.GetActiveWeapon().Range)
        //{

        //}

        //foreach (AWWeapon weapon in WeaponList)
        //{
        //    if (distance == weapon.Range)
        //    {
        //        SetActiveWeapon(weapon);

        //    }
        //}
    }

    public void ShowActionMenu()//must find way to determine which actions to be shown and which to hide in additon to dynamic formating menu
    {
        //for (int i = 0; i < ActionPanel.GetComponentsInChildren<Button>().Length; i++)
        //{
        //    ActionTextList[i].text = WeaponList[i].Name;
        //    ActionButtonList[i].onClick.AddListener(() => { ActionHandler(ActionButtonList[i]); });
        //}
        //ActionPanel.SetActive(true);
        
        string text;
        for (int i = 0; i < ActionMenu.GetComponentsInChildren<UIButton>().Length; i++)
        {
            if (true)//need to figure out what to put here
            {
                text = "";//Actions include: Fire, Wait, Capture, Weapon, Ability(?), Info, End Turn
            }
            else if (true)
            {
                text = "";
            }
            ActionButtonList[i].SetLabelText(text);
        }
        ActionMenu.Show();
    }

    public void HideActionMenu()
    {
        ActionMenu.Hide();
    }

    public void ShowWeaponMenu()
    {
        //for (int i = 0; i < WeaponList.Count; i++)
        //{
        //    WeaponTextList[i].text = WeaponList[i].Name;
        //    WeaponButtonList[i].onClick.AddListener(() => { SetActiveWeapon(WeaponList[i]); });
        //}
        //WeaponPanel.SetActive(true);

        for (int i = 0; i < WeaponButtonList.Count; i++)
        {
            if (i < SelectedUnit.WeaponList.Count)
            {//Called when there are weapons left in the list
                WeaponButtonList[i].SetLabelText(SelectedUnit.WeaponList[i].Name);
                WeaponButtonList[i].EnableButton();
            }
            else
            {//Called when there are no more weapons in the list
                WeaponButtonList[i].SetLabelText("");
                WeaponButtonList[i].DisableButton();
            }
        }
        WeaponMenu.Show();
    }

    public void HideWeaponMenu()
    {
        WeaponMenu.Hide();
    }
}
