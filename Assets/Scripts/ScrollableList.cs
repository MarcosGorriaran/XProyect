using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static System.TimeZoneInfo;

public enum ScrollableListDirection
{
    BottomTop,
    TopBottom,
    LeftRight,
    RightLeft
}
public abstract class ScrollableList : MonoBehaviour
{
    [SerializeField]
    private float _transitionTime;
    [SerializeField]
    private float _distanceBetweenBoxes;
    [SerializeField]
    ScrollableListDirection _scrollDirection;
    private List<SelectableBox> _scrollList = new List<SelectableBox>();
    private SelectableBox _selectedBox;
    private SelectableBox SelectedBox
    {
        get
        {
            return _selectedBox;
        }
        set
        {
            Button buttonElement;
            if (_selectedBox != null)
            {
                if(_selectedBox.TryGetComponent<Button>(out buttonElement))
                    buttonElement.GetComponent<Button>().interactable = false;
            }
            _selectedBox = value;
            value.SelectAction();
            if (_selectedBox.TryGetComponent<Button>(out buttonElement))
                buttonElement.GetComponent<Button>().interactable = true;
        }
    }

    protected List<SelectableBox> ScrollList
    {
        get { return _scrollList; }
    }

    protected virtual void Start()
    {
        if(_scrollList.Count > 0)
        {
            SelectedBox = _scrollList.First();
        }
    }
    public virtual void SelectUpElement()
    {
        if (ScrollList.Count == 0) return;
        SelectableBox actualBox;
        float distance;
        try
        {
            actualBox = _scrollList[_scrollList.IndexOf(SelectedBox) + 1];
            distance = Vector2.Distance(actualBox.transform.localPosition, new Vector2(0, 0)) * -1;
        }
        catch (Exception)
        {
            actualBox = _scrollList.First();
            distance = Vector2.Distance(actualBox.transform.localPosition, new Vector2(0, 0));
        }
        foreach (SelectableBox box in _scrollList)
        {
            box.SmoothlyMoveTowards(new Vector2(box.transform.localPosition.x, box.transform.localPosition.y + distance), _transitionTime);
        }
        SelectedBox = actualBox;
    }
    public virtual void SelectDownElement()
    {
        if (ScrollList.Count == 0) return;
        SelectableBox actualBox = null;
        float distance = 0;
        try
        {
            switch (_scrollDirection)
            {
                case ScrollableListDirection.BottomTop:
                case ScrollableListDirection.LeftRight:
                    actualBox = _scrollList[_scrollList.IndexOf(SelectedBox) - 1];
                    distance = Vector2.Distance(actualBox.transform.localPosition, new Vector2(0, 0));
                    break;
                case ScrollableListDirection.TopBottom:
                case ScrollableListDirection.RightLeft:
                    actualBox = _scrollList[_scrollList.IndexOf(SelectedBox) + 1];
                    distance = Vector2.Distance(actualBox.transform.localPosition, new Vector2(0, 0)) * -1;
                    break;
            }
            
        }
        catch (Exception)
        {
            switch (_scrollDirection)
            {
                case ScrollableListDirection.BottomTop:
                case ScrollableListDirection.LeftRight:
                    actualBox = _scrollList.Last();
                    distance = Vector2.Distance(actualBox.transform.localPosition, new Vector2(0, 0)) * -1;
                    break;
                case ScrollableListDirection.TopBottom:
                case ScrollableListDirection.RightLeft:
                    actualBox = _scrollList.First();
                    distance = Vector2.Distance(actualBox.transform.localPosition, new Vector2(0, 0));
                    break;
            }
            actualBox = _scrollList.Last();
            distance = Vector2.Distance(actualBox.transform.localPosition, new Vector2(0, 0)) * -1;
        }
        foreach (SelectableBox box in _scrollList)
        {
            box.SmoothlyMoveTowards(new Vector2(box.transform.localPosition.x, box.transform.localPosition.y + distance), _transitionTime);
        }
        SelectedBox = actualBox;
    }
    public void AddBox(SelectableBox box)
    {
        box.transform.parent = transform;
        
        Vector2 newBoxLocalPosition = new Vector2(0,0);
        switch (_scrollDirection)
        {
            case ScrollableListDirection.BottomTop:
                
                if (_scrollList.Count > 0)
                {
                    newBoxLocalPosition = _scrollList.Last().transform.localPosition;
                    newBoxLocalPosition = new Vector2(newBoxLocalPosition.x, newBoxLocalPosition.y + _distanceBetweenBoxes);
                }
                else
                {
                    newBoxLocalPosition = new Vector2(0, 0);
                }
                break;
            case ScrollableListDirection.TopBottom:
                if (_scrollList.Count > 0)
                {
                    newBoxLocalPosition = _scrollList.Last().transform.localPosition;
                    newBoxLocalPosition = new Vector2(newBoxLocalPosition.x, newBoxLocalPosition.y - _distanceBetweenBoxes);
                }
                else
                {
                    newBoxLocalPosition = new Vector2(0, 0);
                }
                break;
            case ScrollableListDirection.LeftRight:
                if (_scrollList.Count > 0)
                {
                    newBoxLocalPosition = _scrollList.Last().transform.localPosition;
                    newBoxLocalPosition = new Vector2(newBoxLocalPosition.x + _distanceBetweenBoxes, newBoxLocalPosition.y);
                }
                else
                {
                    newBoxLocalPosition = new Vector2(0, 0);
                }
                break;
            case ScrollableListDirection.RightLeft:
                if (_scrollList.Count > 0)
                {
                    newBoxLocalPosition = _scrollList.Last().transform.localPosition;
                    newBoxLocalPosition = new Vector2(newBoxLocalPosition.x - _distanceBetweenBoxes, newBoxLocalPosition.y);
                }
                else
                {
                    newBoxLocalPosition = new Vector2(0, 0);
                }
                break;
        }
        box.GetComponent<RectTransform>().localPosition = newBoxLocalPosition;
        if (SelectedBox == null)
        {
            SelectedBox = box;
        }

        _scrollList.Add(box);

    }
    protected virtual void SelectBox(SelectableBox element)
    {
        SelectedBox = element;
    }
}
