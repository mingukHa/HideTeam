using System.Collections.Generic;
using UnityEngine;

public class MatDetChange : MonoBehaviour
{
    private MeshRenderer meshRenderer = null;
    private Material mat = null;

    private Color noDetColor = new Color(0, 1, 0, 0.043f);
    private Color DetPlayer = new Color(0, 0, 1, 0.043f);
    private Color DetNPC = new Color(1, 0, 0, 0.043f);

    public List<int> interactObj;
    public bool isVIP = false;
    public bool isPlayer = false;
    public bool isOLDMan = false;

    private void Awake()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        mat = meshRenderer.material;
    }

    private void Start()
    {
        mat.color = noDetColor;
    }

    private void FixedUpdate()
    {
        if(interactObj.Contains(2))
        {
            mat.color = DetNPC;
        }
        else if (interactObj.Contains(1))
        {
            mat.color = DetPlayer;
        }
        else
        {
            mat.color = noDetColor;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayer = true;
            interactObj.Add(1);
        }
        else if (other.gameObject.name == "RichMan")
        {
            isVIP = true;
            interactObj.Add(2);
        }
        else if (other.gameObject.name == "OldMan")
        {
            isOLDMan = true;
            interactObj.Add(2);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayer = false;
            interactObj.Remove(1);
        }
        else if (other.gameObject.name == "RichMan")
        {
            isVIP = false;
            interactObj.Remove(2);
        }
        else if (other.gameObject.name == "OldMan")
        {
            isOLDMan = false;
            interactObj.Remove(2);
        }
    }
}
