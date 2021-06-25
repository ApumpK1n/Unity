using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using ConfigToolGenerator;
using System.Collections;
using System;
using UnityEngine.EventSystems;

public class NewbieGuideSequence : SingletonBehaviour<NewbieGuideSequence>
{

    public Material RectMaterial;
    public Material CircleMaterial;

    public Image Mask;
    public GameObject Display;
    public Text Description;

    private List<Tutorials> guides = new List<Tutorials>();
    private int index;

    private Tutorials currentInfo;
    private Canvas canvas;
    private Camera m_camera;
    private Camera m_targetCamera;
    private List<NewbieGuideData> m_newbieGuideData;
    private RectTransform currentTarget;

    private CircleGuidanceController circleGuidanceController;
    private RectGuidanceController rectGuidanceController;

    private Guidance currentGuidance;

    private Vector3 m_center;
    private Coroutine nextStep;
    private bool End = true;
    private RectTransform currentShowTarget;
    private float longPressTimer = 0f;
    private bool down = false;
    private float AllAreaCanClickTimer = 0f;

    public override void Awake()
    {
        base.Awake();
        canvas = gameObject.GetComponent<Canvas>();
        m_camera = gameObject.GetComponentInParent<Camera>();

        m_camera.gameObject.SetActive(false);

        circleGuidanceController = new CircleGuidanceController();
        rectGuidanceController = new RectGuidanceController();
    }

    public void StartGuide(List<Tutorials> guides, List<NewbieGuideData> newbieGuideData)
    {
        m_camera.gameObject.SetActive(true);
        m_newbieGuideData = newbieGuideData;
        this.guides = guides;

        End = false;
        NextStep(true);

    }

    public void EndGuide()
    {
        End = true;
        m_camera.gameObject.SetActive(false);
        currentTarget.gameObject.SetActive(false);
        currentShowTarget.gameObject.SetActive(false);
        Mask.gameObject.SetActive(false);
        Display.SetActive(false);
        StopNextStep();
        for (int i = 0; i < m_newbieGuideData.Count; i++)
        {
            m_newbieGuideData[i].Release();
        }
        m_newbieGuideData.Clear();

        m_newbieGuideData = null;

        m_targetCamera = null;

        index = 0;
        if (guides.Count > 0)
        {
            NewbieGuideManager.Instance.AddDoneGuide(guides[0].GroupId);
        }
    }

    public void AddNewbieGuideData(NewbieGuideData data)
    {
        m_newbieGuideData.Add(data);
    }

    public void NextStep(bool start = false)
    {
        if (start)
        {
            index = 0;
        }
        else
        {
            index += 1;
        }
        longPressTimer = 0f;
        AllAreaCanClickTimer = 0f;

        Init();
        RefreshMask();
    }

    public Tutorials GetNewbieGuideInfo()
    {
        if (index < guides.Count)
        {
            return guides[index];
        }
        return null;
    }

    private RectTransform GetTargetTransform()
    {
        if (index < guides.Count)
        {
            return m_newbieGuideData[index].target;
        }
        return null;
    }

    private Camera GetTargetCamera()
    {
        if (index < guides.Count)
        {
            return m_newbieGuideData[index].targetCamera;
        }
        return null;
    }

    public void SkipStep()
    {
        if (nextStep == null)
        {
            ClickNextStep();
        }
    }

    private void Update()
    {
        if (currentInfo == null) return;
        int triggerType = currentInfo.TriggerType;
        if (triggerType == 0)
        {
            if (Input.GetMouseButtonDown(0) && currentGuidance.IsInActiveArea(Input.mousePosition) && nextStep == null)
            {
                ClickNextStep();
            }
        }
        else if (triggerType == 1)
        {
            AllAreaCanClickTimer += Time.unscaledDeltaTime;
            if (AllAreaCanClickTimer >= 2f && Input.GetMouseButtonDown(0) && nextStep == null)
            {
                ClickNextStep();
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0) && currentGuidance.IsInActiveArea(Input.mousePosition))
            {
                down = true;
                longPressTimer = 0;
            }
            if (down && currentGuidance.IsInActiveArea(Input.mousePosition))
            {
                longPressTimer += Time.unscaledDeltaTime;

                if (longPressTimer >= 0.5f && nextStep == null)
                {
                    ClickNextStep();
                    longPressTimer = 0f;
                }

            }
            if (Input.GetMouseButtonUp(0))
            {
                down = false;
                longPressTimer = 0f;
            }
        }
    }

    private void ClickNextStep()
    {
        if (currentInfo != null)
        {
            ReportManager.ReportGameGuide(currentInfo.id, currentInfo.Name);
        }
        currentTarget.gameObject.SetActive(false);
        currentShowTarget.gameObject.SetActive(false);
        Mask.gameObject.SetActive(false);

        Action clickAction = m_newbieGuideData[index].clickAction;
        if (clickAction != null)
        {
            clickAction?.Invoke();
        }
        m_newbieGuideData[index].Release();
        nextStep = StartCoroutine(StartNextStep());
    }

    private IEnumerator StartNextStep()
    {
        while (m_newbieGuideData.Count <= index + 1 && index + 1 < guides.Count)
        {
            yield return null;
        }
        nextStep = null;
        if (index + 1 >= guides.Count)
        {
            EndGuide();
            yield break;
        }
        else
        {
            NextStep();
        }
    }

    public bool IsInGuide()
    {
        return !End;
    }

    private void StopNextStep()
    {
        if (nextStep != null)
        {
            StopCoroutine(nextStep);
        }

        nextStep = null;
    }

    private void Init()
    {
        currentInfo = GetNewbieGuideInfo();

        float configY = currentInfo.PositionY;
        float shipei = 1920;
        float height = configY / shipei * Screen.height;
        Vector3 screenPos = new Vector3(Screen.width / 2, height, 0);

        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, screenPos, m_camera, out position);
        //Vector3 pos = Guidance.ScreenPosToCanvasPos(canvas.GetComponent<RectTransform>(), screenPos);
        Display.GetComponent<RectTransform>().anchoredPosition = position;
        Display.SetActive(currentInfo.Display == 1);

        Description.text = currentInfo.Description;

        currentTarget = GetTargetTransform();

        m_targetCamera = GetTargetCamera();

        currentShowTarget = Instantiate(currentTarget, canvas.transform, false);

        Vector3 targetPos = m_targetCamera.WorldToScreenPoint(currentTarget.position);
        Vector2 targetLocal;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, targetPos, m_camera, out targetLocal);
        currentShowTarget.gameObject.layer = Layers.Guide.layer;
        currentShowTarget.anchoredPosition3D = new Vector3(targetLocal.x, targetLocal.y, 100);

        Transform[] childs = currentShowTarget.GetComponentsInChildren<Transform>(true);
        for (int i = 0; i < childs.Length; i++)
        {
            childs[i].gameObject.layer = Layers.Guide.layer;
            childs[i].gameObject.SetActive(true);
        }

        currentTarget.gameObject.SetActive(false);
        currentShowTarget.gameObject.SetActive(true);

        // 先做物体创建，再挪动相机。
        m_camera.transform.position = m_targetCamera.transform.position;

        if (m_targetCamera.orthographic)
        {
            m_camera.orthographic = true;
            m_camera.orthographicSize = m_targetCamera.orthographicSize;
        }
        else
        {
            m_camera.orthographic = false;
            m_camera.fieldOfView = m_targetCamera.fieldOfView;
        }

        m_camera.farClipPlane = m_targetCamera.farClipPlane;
    }

    public void RefreshMask()
    {
        Vector3 targetPos = m_targetCamera.WorldToScreenPoint(currentTarget.position);
        Vector2 targetLocal;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, targetPos, m_camera, out targetLocal);
        currentShowTarget.anchoredPosition3D = new Vector3(targetLocal.x, targetLocal.y, 100);

        Mask.gameObject.SetActive(true);

        Material material;
        if (currentInfo.GuidanceType == 1)
        {
            material = RectMaterial;
            currentGuidance = rectGuidanceController;
        }
        else
        {
            material = CircleMaterial;
            currentGuidance = circleGuidanceController;
        }
        Mask.material = material;
        currentGuidance.SetCamera(m_targetCamera);
        currentGuidance.SetMaterial(Mask.material);
        currentGuidance.SetCanvas(canvas);
        currentGuidance.SetTarget(m_newbieGuideData[index].highlightArea);
        currentGuidance.RefreshMask();
    }
}