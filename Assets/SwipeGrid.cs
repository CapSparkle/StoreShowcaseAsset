using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeGrid : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public float scrollSpeed = 5f; //scroll speed
    public Vector2 size = new Vector2(5, 5); //size of the grid
    public Transform grid; //reference to the grid transform

    private Vector2 gridPos; //position of the grid

    private float dragStartTime; //time when drag begins
    private Vector2 dragStartPos; //position where drag begins

    public EventTrigger trigger;

    void Start()
    {
        //initialize grid position
        gridPos = grid.position;

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.Drag;
        entry.callback.AddListener((data) => { OnDrag((PointerEventData)data); });
        trigger.triggers.Add(entry);

        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.EndDrag;
        entry.callback.AddListener((data) => { OnEndDrag((PointerEventData)data); });
        trigger.triggers.Add(entry);
    }

    public void OnDrag(PointerEventData eventData)
    {
        print("drag");
        //update drag position and time
        gridPos += PointerDelta(eventData) * Time.deltaTime * scrollSpeed;
        dragStartTime = Time.time;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        print("endDrag");
        //calculate drag velocity and direction
        float dragTime = Time.time - dragStartTime;
        Vector2 dragDelta = PointerDelta(eventData) / dragTime;

        //calculate final position of the grid based on drag direction
        float dragDistance = PointerDelta(eventData).y;
        if (Mathf.Abs(dragDistance) > size.y / 3)
        {
            if (dragDelta.y < 0) gridPos.y += size.y;
            else gridPos.y -= size.y;
        }
    }

    private Vector2 PointerDelta(PointerEventData eventData)
    {
        //calculate position difference between current and previous frame
        return (eventData.position - eventData.pressPosition);
    }
}
