
using Mlf.Gm.InputTypes;
using UnityEngine;

namespace Mlf.Gm
{
    public delegate void ColliderHitEvent(Collider collider);

    public class GameInputManager : MonoBehaviour
    {

        public event ColliderHitEvent OnColliderHit;
        public static GameInputManager instance;


        public void Awake()
        {
            if (instance == null)
                instance = this;
            else
            {
                Destroy(this);
                Debug.LogError("Duplicate Singleton");
            }
        }

        void Update()
        {
            //Debug.Log("Input Updating....");
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("Left Mouse Button 0 ");
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit rayHit;

                if (Physics.Raycast(ray, out rayHit, 100.0f))
                {
                    Debug.Log("============================");
                    //Debug.Log(hit.collider.gameObject.name);
                    //Debug.Log(hit.collider.gameObject.tag);
                    /*
                    if (rayHit.collider.tag == "Pickable")
                    {
                        Debug.Log("Pickable Selected");

                        return;
                    }
                    else
                    {
                        OnColliderHit?.Invoke(rayHit.collider);
                    }
                    */
                    OnColliderHit?.Invoke(rayHit.collider);


                }
                else
                {
                    Debug.LogWarning("No collider hit");
                }

            }







            if (Input.GetMouseButtonDown(1))
            {

            }
        }



    }
}



