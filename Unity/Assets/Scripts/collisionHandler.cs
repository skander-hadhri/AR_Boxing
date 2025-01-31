using UnityEngine;
using System.Collections;

public class collisionHandler : MonoBehaviour
{
    public GameObject target;
    public Material dissolveMaterial; // Matériau de dissolution (assigné dans Unity)

    private Material originalMaterial;
    private Renderer objectRenderer;
    private bool isDissolving = false;
    private float dissolveAmount = 0f;
    public float dissolveSpeed = 1.5f; // Contrôle la vitesse de disparition

    void Start()
    {
        // Récupérer le Renderer et stocker le matériau original
        if (target != null)
        {
            objectRenderer = target.GetComponent<Renderer>();
            if (objectRenderer != null)
            {
                originalMaterial = objectRenderer.material; // Stocke le matériau d'origine
            }
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (!isDissolving) // Empêche plusieurs dissolutions simultanées
        {
            if (target.tag == "green_target" && (collider.tag == "right_hand" || collider.tag == "left_hand"))
            {
                Debug.Log("Vous avez touché une cible verte !");
                StartCoroutine(DissolveAndDestroy());
            }
            else if (target.tag == "red_target" && collider.tag == "head")
            {
                Debug.Log("Vous avez été touché !");
                StartCoroutine(DissolveAndDestroy());
            }
        }
    }

    IEnumerator DissolveAndDestroy()
    {
        isDissolving = true;

        // Changer le matériau par le Dissolve Material
        if (dissolveMaterial != null && objectRenderer != null)
        {
            objectRenderer.material = new Material(dissolveMaterial); // Crée une instance unique pour éviter les conflits
        }

        // Vérifier si le matériau possède la propriété "_DissolveAmount"
        if (objectRenderer != null && objectRenderer.material.HasProperty("_DissolveAmount"))
        {
            while (dissolveAmount < 1.0f)
            {
                dissolveAmount += Time.deltaTime * dissolveSpeed;
                objectRenderer.material.SetFloat("_DissolveAmount", dissolveAmount);
                yield return null;
            }
        }

        Destroy(target); // Détruire l'objet après la dissolution
    }
}
