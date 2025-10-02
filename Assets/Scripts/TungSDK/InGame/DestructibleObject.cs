using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[AddComponentMenu("WASD/Physics/Destructible")]
public class DestructibleObject : TickBehaviour
{
    [Header("Destructible Settings")]
    [Range(0, 50)]
    public float Strength;
    [Header("Fractured Object Prefab Spawn")]
    public GameObject FracturedObject;
    public Vector3 PositionOffset;
    public float TimeToDestroy = 15;
    private bool isFractured = false;
    [Header("Destroy Events")]
    public bool DoSlowmotionWhenDestroy;

    [Header("FX")]
    public float TimeToFracture = 0f;
    public GameObject DestructionFX;
    IEnumerator IE_DestroyObject()
    {
        if(isFractured == false)
        {
            if(FracturedObject != null)
            {
                Invoke(nameof(FractureThisObject), TimeToFracture);

                if(DestructionFX != null)
                {
                    Instantiate(DestructionFX, transform.position, transform.rotation, transform);
                }
            }
            else
            {
                Debug.LogWarning("There is no 'Fractured Object' linked in " + gameObject.name);
            }

            if(DoSlowmotionWhenDestroy)
            {
                SlowMotion.DoSlowMotion(0.1f, 5f);
            }
        }
        yield return new WaitForEndOfFrame();
    }
    public void Fracture()
    {
        StartCoroutine(IE_DestroyObject());
    }
    /// <summary>
    /// Destroy the gameobject and instantiate the fractured prefab
    /// </summary>
    void FractureThisObject()
    {
        if(isFractured == true) return;

        //Instantiate fracture
        var fractured_obj = (GameObject)Instantiate(FracturedObject, transform.position + PositionOffset, transform.rotation);
        fractured_obj.SetActive(true);
        //Destroy this
        Destroy(this.gameObject, 0.01f);

        //Destroy fracture timer
        Destroy(fractured_obj, TimeToDestroy);

        //Check the bool
        isFractured = true;
    }
}