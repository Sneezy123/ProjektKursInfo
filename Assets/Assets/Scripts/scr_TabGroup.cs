using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scr_TabGroup : MonoBehaviour
{
    public List<scr_TabButton> tabButtons;
    public Sprite tabIdle;
    public Sprite tabHover;
    public Sprite tabActive;
    public scr_TabButton selectedTab;

    public void Subscribe(scr_TabButton button)
    {
        if (tabButtons == null)
        {
            tabButtons = new List<scr_TabButton>();
        }

        tabButtons.Add(button);
    }

    public void OnTabEnter(scr_TabButton button)
    {
        if (selectedTab != null)
        {
            selectedTab.Deselect();
        }

        selectedTab = button;

        selectedTab.Select();

        ResetTabs();
        if (selectedTab == null || button != selectedTab)
        {
            button.background.sprite = tabHover;
        }
    }

    public void OnTabExit(scr_TabButton button)
    {
        ResetTabs();
    }

    public void OnTabSelect(scr_TabButton button)
    {
        selectedTab = button;
        ResetTabs();
        button.background.sprite = tabActive;
    }

    public void ResetTabs()
    {
        foreach (scr_TabButton button in tabButtons)
        {
            if (selectedTab != null && button == selectedTab) { continue; }
            button.background.sprite = tabIdle;
        }
    }
}
