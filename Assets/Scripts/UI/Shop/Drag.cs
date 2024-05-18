using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Drag : MonoBehaviour,  IDragHandler, IBeginDragHandler, IEndDragHandler
{
	[SerializeField] private Image _image;
    public Vector3 _oldPosition;

    //[SerializeField] private GameObject 

    // Start is called before the first frame update
    void Start()
    {
        _image = GetComponent<Image>();
        _oldPosition = _image.rectTransform.position;
        Debug.Log(_oldPosition);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void OnDrag(PointerEventData eventData)
	{
        Debug.Log(transform.position);
		transform.position = eventData.position;
	}

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Begin Drag");
        _image.raycastTarget = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("End Drag");
        transform.position = _oldPosition;
		_image.raycastTarget = true;
    }
	public void SetNewPosition(Vector3 newPosition)
	{
		_oldPosition = newPosition;
	}
}
