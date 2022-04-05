using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpPanel : MonoBehaviour
{
    public Transform pagesParent;
    public Button previousPageButton;
    public Button nextPageButton;

    private int currentPageIndex;
    private int pagesCount;

    private GameObject currentPage;
    private GameObject[] pages;
    
    private void Start()
    {
        previousPageButton.onClick.AddListener(PreviousPage);
        nextPageButton.onClick.AddListener(NextPage);
    }
    
    public void SetHelpPanel()
    {
        pagesCount = pagesParent.childCount;
        currentPageIndex = 0;
        pages = new GameObject[pagesCount];

        for (int i = 0; i < pagesCount; i++)
        {
            pages[i] = pagesParent.GetChild(i).gameObject;
            pages[i].SetActive(false);
        }
        
        SetPage(currentPageIndex);
        UpdateButtons();
    }

    private void NextPage()
    {
        if (currentPageIndex < pagesCount-1)
        {
            AudioBox.instance.AudioPlay(AudioName.Click);
            currentPageIndex++;
            SetPage(currentPageIndex);
            UpdateButtons();
        }
    }

    private void PreviousPage()
    {
        if (currentPageIndex > 0)
        {
            AudioBox.instance.AudioPlay(AudioName.Click);
            currentPageIndex--;
            SetPage(currentPageIndex);
            UpdateButtons();
        }
    }
    
    private void UpdateButtons()
    {
        previousPageButton.interactable = currentPageIndex > 0;
        nextPageButton.interactable = currentPageIndex < pagesCount - 1;
    }

    private void SetPage(int number)
    {
        if (currentPage != null)
        {
            currentPage.SetActive(false);
        }

        currentPage = pages[number];
        currentPage.SetActive(true);
    }
}
