using System.Collections.Generic;
using TMPro;
//using UnityEditor.Rendering.LookDev;
using UnityEngine;

public class MatDetChange : MonoBehaviour
{
    public delegate void OnTriggerEnterDelegate(interType _interType);
    public delegate void OnTriggerExitDelegate(interType _interType);

    private OnTriggerEnterDelegate onTriggerEntercallback = null;
    private OnTriggerExitDelegate onTriggerExitcallback = null;

    private KeyCode debugButton = KeyCode.F3;

    public OnTriggerEnterDelegate OnTriggerEnterCallback
    {
        get { return onTriggerEntercallback; }
        set {  onTriggerEntercallback = value; }
    }
    public OnTriggerExitDelegate OnTriggerExitCallback
    {
        get { return onTriggerExitcallback; }
        set { onTriggerExitcallback = value; }
    }

    private MeshRenderer meshRenderer = null;
    private Material mat = null;

    public bool gcode = false;

    private Color noDetColor = new Color(0, 1, 0, 0.043f);
    private Color DetPlayer = new Color(0, 0, 1, 0.043f);
    private Color DetNPC = new Color(1, 0, 0, 0.043f);

    public enum interType
    {
        Player,
        RichMan,
        OldMan,
    };
    public List<interType> buffer;
    public bool isDet = false;
    public bool isPrDet = false;

    private void Awake()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        mat = meshRenderer.material;
    }

    private void Start()
    {
        mat.color = noDetColor;
        isPrDet = false;
        meshRenderer.gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        if(buffer.Contains(interType.RichMan) || buffer.Contains(interType.OldMan))
        {
            mat.color = DetNPC;
        }
        else if (buffer.Contains(interType.Player))
        {
            mat.color = DetPlayer;
        }
        else
        {
            isDet = false;
            isPrDet = false;
            mat.color = noDetColor;
        }

        if(Input.GetKeyDown(debugButton))
        {
            DebugModeSwitch();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "OldMan")
        {
            isDet = true;
            buffer.Add(interType.OldMan);
            OnTriggerEnterCallback?.Invoke(interType.OldMan);
        }
        else if (other.gameObject.name == "RichMan")
        {
            isDet = true;
            buffer.Add(interType.RichMan);
            OnTriggerEnterCallback?.Invoke(interType.RichMan);
        }
        if (other.gameObject.name == "PlayerHolder")
        {
            isDet = true;
            isPrDet = true;
            buffer.Add(interType.Player);
            OnTriggerEnterCallback?.Invoke(interType.Player);
                
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.name == "PlayerHolder")
        {
            EventManager.Subscribe(EventManager.GameEventType.Conversation5, HeardGcode);
        }
    }
    //private void OnEnable()
    //{
    //    EventManager.Subscribe(EventManager.GameEventType.Conversation5, HeardGcode);
    //}
    private void OnDisable()
    {
        EventManager.Unsubscribe(EventManager.GameEventType.Conversation5, HeardGcode);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "OldMan")
        {
            buffer.Remove(interType.OldMan);
            OnTriggerExitCallback?.Invoke(interType.OldMan);
        }
        else if (other.gameObject.name == "RichMan")
        {
            buffer.Remove(interType.RichMan);
            OnTriggerExitCallback?.Invoke(interType.RichMan);
        }
        else if (other.gameObject.name == "PlayerHolder")
        {
            if (buffer.Contains(interType.Player))
                buffer.Remove(interType.Player);
            OnTriggerExitCallback?.Invoke(interType.Player);
            EventManager.Unsubscribe(EventManager.GameEventType.Conversation5, HeardGcode);
            isPrDet = false;
        }
    }
    private void HeardGcode()
    {
        gcode = true;
    }

    private void DebugModeSwitch()
    {
        meshRenderer.gameObject.SetActive(!meshRenderer.gameObject.activeSelf);
    }
}
