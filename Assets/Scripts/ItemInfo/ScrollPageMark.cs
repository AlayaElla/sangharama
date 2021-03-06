﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ScrollPageMark : MonoBehaviour
{
    public ScrollInfo scrollPage;
    public ToggleGroup toggleGroup;
    public Toggle togglePrefab;

    public List<Toggle> toggleList = new List<Toggle>();
	
    void Awake()
    {
        scrollPage.OnPageChanged = OnScrollPageChanged;

        foreach (Transform t in toggleGroup.transform)
        {
            if (t.gameObject.activeSelf)
                Destroy(t.gameObject);
        }
    }
	
	public void OnScrollPageChanged(int pageCount, int currentPageIndex)
    {
        if(pageCount!=toggleList.Count)
        {
            if(pageCount>toggleList.Count)
            {
                int cc = pageCount - toggleList.Count;
                for(int i=0; i< cc; i++)
                {
                    toggleList.Add(CreateToggle());
                }
            }
            else if(pageCount < toggleList.Count)
            {
                while(toggleList.Count > pageCount)
                {
                    Toggle t = toggleList[toggleList.Count - 1];
                    toggleList.Remove(t);
                    DestroyImmediate(t.gameObject);
                }
            }
        }

        if(currentPageIndex>=0)
        {
            toggleList[currentPageIndex].isOn = true;
        }
    }

    Toggle CreateToggle()
    {
        Toggle t = GameObject.Instantiate<Toggle>(togglePrefab);
        t.gameObject.SetActive(true);
        t.transform.SetParent(toggleGroup.transform, false);
        t.transform.localScale = Vector3.one;
        t.transform.localPosition = Vector3.zero;
        return t;
    }
}
