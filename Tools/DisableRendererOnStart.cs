using UnityEngine;

[RequireComponent (typeof(Renderer))]
public class DisableRendererOnStart : MonoBehaviour
{
	void Start ()
	{
        GetComponent<Renderer>().enabled = false;
	}
}
