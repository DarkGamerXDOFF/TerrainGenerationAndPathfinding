using System;
using UnityEngine;

public class ResourceNode : MonoBehaviour
{

    public static event EventHandler OnResourceNodeClicked;

    [SerializeField] private ResourceType resourceType;
    [SerializeField] private int resouceAmount = 3;

    public ResourceType GrabResource()
    {
        resouceAmount--;

        if (resouceAmount <= 0)
            gameObject.SetActive(false); //Show resource node is depleted

        //Should do switch to set appropriate depleted graphic

        return resourceType;
    }
    public ResourceType GetResourceType() => resourceType;

    public bool HasResources() => resouceAmount > 0;

    public Vector2 GetPosition() => transform.position;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left Mouse Button
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                if (OnResourceNodeClicked != null)
                    OnResourceNodeClicked(this, EventArgs.Empty);
            }
        }
    }
}
