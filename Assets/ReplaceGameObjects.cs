using UnityEngine;

public class ReplaceTaggedObjects : MonoBehaviour
{
    public GameObject replacementPrefab;

    void Start()
    {
        // Find all objects with tag "Tree"
        GameObject[] trees = GameObject.FindGameObjectsWithTag("Tree");

        foreach (GameObject tree in trees)
        {
            // Skip if it's already the replacement prefab
            if (tree == replacementPrefab)
                continue;

            // Store transform data
            Vector3 pos = tree.transform.position;
            Quaternion rot = tree.transform.rotation;
            Transform parent = tree.transform.parent;

            // Instantiate replacement
            GameObject newTree = Instantiate(replacementPrefab, pos, rot, parent);
            newTree.transform.localScale = tree.transform.localScale;

            // Optionally keep the tag
            newTree.tag = "Tree";

            // Destroy the old one
            Destroy(tree);
        }
    }
}


