using UnityEngine;

public abstract class BaseObject : MonoBehaviour
{
    private Transform goTransform;
    private GameObject goInstance;
    private string goname;

    private Rigidbody goRigidbody;

    private Material goMaterial;
    private Color goColor;
    private Animator goAnimator;

    private bool isVisible;

    protected Transform GoTransform { get => goTransform; set => goTransform = value; }
    protected string GoName { get => goname; set => goname = value; }
    protected GameObject GoInstance { get => goInstance; set => goInstance = value; }
    protected Rigidbody GoRigidbody { get => goRigidbody; set => goRigidbody = value; }
    protected Material GoMaterial { get => goMaterial; set => goMaterial = value; }
    protected Color GoColor { get => goColor; set => goColor = value; }
    protected Animator GoAnimator { get => goAnimator; set => goAnimator = value; }
    protected bool IsVisible { get => isVisible; set => isVisible = value; }

    protected virtual void Awake()
    {
        GoTransform = transform;
        GoName = name;
        goInstance = gameObject;

        if (GetComponent<Animator>())
        {
            GoAnimator = GetComponent<Animator>();
        }
        else
        {
            Debug.Log("no animator " + name);
        }

        if (GetComponent<Rigidbody>())
        {
            GoRigidbody = GetComponent<Rigidbody>();
        }
        else
        {
            Debug.Log("no rigidbody " + name);
        }

        if (GetComponent<Renderer>())
        {
            GoMaterial = GetComponent<Renderer>().material;
        }
    }

    protected void FreezeZ(bool freeze)
    {
        if( freeze )
        {
            GoRigidbody.constraints |= 
                RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationZ;
        } 
        else
        {
            GoRigidbody.constraints &= 
                ~(RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationZ);
        }
    }
}
