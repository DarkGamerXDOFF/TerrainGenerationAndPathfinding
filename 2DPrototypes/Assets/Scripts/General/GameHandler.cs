using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public static GameHandler I;

    [SerializeField] private GathererAI[] gathererAIArray;
    private GathererAI selectedGathererAI;

    [SerializeField] private Transform[] woodNodeTransforms;
    [SerializeField] private Transform[] stoneNodeTransforms;
    [SerializeField] private Transform storageTransform;

    private List<ResourceNode> resourceNodeList;

    [SerializeField] private float nearbyNodesDistance = 3f;

    private void Awake()
    {
        if (I == null)
            I = this;
        else
            Destroy(this);

        GameResources.Init();

        resourceNodeList = new List<ResourceNode>();

        foreach (Transform node in woodNodeTransforms)
        {
            resourceNodeList.Add(node.GetComponent<ResourceNode>());
        }
        foreach (Transform node in stoneNodeTransforms)
        {
            resourceNodeList.Add(node.GetComponent<ResourceNode>());
        }

        ResourceNode.OnResourceNodeClicked += ResourceNode_OnResourceNodeClicked;
        GathererAI.OnGathererClicked += GathererAI_OnGathererClicked;
    }

    private void GathererAI_OnGathererClicked(object sender, System.EventArgs e)
    {
        GathererAI gathererAI = sender as GathererAI;
        
        if (selectedGathererAI != null)
            selectedGathererAI.SetSelectedSprite(false);
        
        selectedGathererAI = gathererAI;
        
        if (selectedGathererAI != null)
            selectedGathererAI.SetSelectedSprite(true);
    }

    private void ResourceNode_OnResourceNodeClicked(object sender, System.EventArgs e)
    {
        ResourceNode resourceNode = sender as ResourceNode;
        
        if (selectedGathererAI != null)
            selectedGathererAI.SetResourceNode(resourceNode);
    }

    public ResourceNode GetResourceNode()
    {
        List<ResourceNode> temp = new List<ResourceNode>(resourceNodeList);
        for (int i = 0; i < temp.Count; i++)
        {
            if (!temp[i].HasResources())
            {
                temp.RemoveAt(i);
                i--;
            }
        }

        if (temp.Count > 0)
            return temp[Random.Range(0, temp.Count)];
        else
            return null;
    }
    
    public ResourceNode GetResourceNodeNearPosition(Vector3 position)
    {
        List<ResourceNode> temp = new List<ResourceNode>(resourceNodeList);
        for (int i = 0; i < temp.Count; i++)
        {
            if (!temp[i].HasResources() || Vector2.Distance(position, temp[i].GetPosition()) > nearbyNodesDistance)
            {
                temp.RemoveAt(i);
                i--;
            }
        }

        if (temp.Count > 0)
            return temp[Random.Range(0, temp.Count)];
        else
            return null;
    }

    public Transform GetStorageTransform()
    {
        return storageTransform;
    }
}
