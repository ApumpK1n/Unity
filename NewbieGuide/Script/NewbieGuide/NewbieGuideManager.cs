using UnityEngine;
using UnityEngine.UI;
using ConfigToolGenerator;
using System.Collections;
using System;
using System.Collections.Generic;

public enum NewbieGuideType
{
    Default = 0,
    FirstLogin = 1,
    EnterIsland = 2,
    IntoBucket = 8,
    GoHome = 9,
    Order = 3,
    SelectBait = 4,
    SkipTime = 5,
    ChangePit = 7,
    OpenJar = 10,
    CatchOctopus = 6,
    FinishOrder = 11,
    GoBeach = 12,
}


public class NewbieGuideManager : SingletonBehaviour<NewbieGuideManager>
{

    public NewbieGuideSequence newbieGuideSequence;
    public Camera m_camera;

    private Dictionary<NewbieGuideType, List<Tutorials>> m_NewbieGuides = new Dictionary<NewbieGuideType, List<Tutorials>>();
    public List<int> DoneNewbieGuide = new List<int>();

    public override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);

        foreach(KeyValuePair<int, Tutorials> keyValuePair in MetaDataManager.Instance.dataTutorialsMap)
        {
            NewbieGuideType type = (NewbieGuideType)keyValuePair.Value.GroupId;
            Tutorials newbieGuide = keyValuePair.Value;
            
            if (!m_NewbieGuides.ContainsKey(type))
            {
                List<Tutorials> sequence = new List<Tutorials>() { newbieGuide };
                m_NewbieGuides[type] = sequence;
            }
            else
            {
                m_NewbieGuides[type].Add(newbieGuide);
            }
        }

        DoneNewbieGuide = PlayerData.GuideData;
    }

    public bool IsDoneGuide(NewbieGuideType guideType)
    {
        return DoneNewbieGuide.Contains((int)guideType);
    }

    public void AddDoneGuide(int type)
    {
        if (DoneNewbieGuide != null)
        {
            //DoneNewbieGuide.Add(type);
            PlayerData.ChangeDoneGuideList(type);
        }

    }

    public bool CanEnterNewbieGuide(NewbieGuideType type)
    {
        if (!m_NewbieGuides.ContainsKey(type)) return false;
        if (DoneNewbieGuide != null)
        {
            if (DoneNewbieGuide.Contains((int)type)) return false;
        }
        return true;
    }

    public void StartNewbieGuide(NewbieGuideType type, List<NewbieGuideData> data)
    {
        if (!CanEnterNewbieGuide(type)) return;
        m_camera.gameObject.SetActive(true);
        
        newbieGuideSequence.StartGuide(m_NewbieGuides[type], data);
    }
    
    public void AddNewbieGuideData(NewbieGuideData data)
    {
        newbieGuideSequence.AddNewbieGuideData(data);
    }

    public void StopNowNewbieGuide()
    {
        newbieGuideSequence.EndGuide();
    }

    public void LogGuideStep(int guideId, int step)
    {
        Tutorials guide = m_NewbieGuides[(NewbieGuideType)guideId][step - 1];
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("guideid", guideId);
        parameters.Add("guidedesc", guide.Description);
        parameters.Add("guidestep", step);
        //ChannelManager.LogEvent("ohayoo_game_guide", parameters);
    }

}

public class NewbieGuideData
{
    public RectTransform target;
    public Action clickAction;
    public RectTransform highlightArea;
    public Camera targetCamera;

    public NewbieGuideData(RectTransform rect, Camera camera, Action action, RectTransform highlightArea)
    {
        target = rect;
        clickAction = action;
        this.highlightArea = highlightArea;
        targetCamera = camera;
    }

    public void Release()
    {
        target = null;
        clickAction = null;
        highlightArea = null;
        targetCamera = null;
    }

    ~NewbieGuideData()
    {
        target = null;
        clickAction = null;
        highlightArea = null;
        targetCamera = null;
    }
}