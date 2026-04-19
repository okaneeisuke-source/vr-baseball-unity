using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RFootMarkerControler : MonoBehaviour
{
  [SerializeField] private GameObject Target;
  public bool isTargeted = false;
  // Start is called before the first frame update
  void Start()
  {
      isTargeted = false;
  }

  // Update is called once per frame
  void Update()
  {

  }
  private void OnTriggerEnter(Collider other)
  {
    // 触れた瞬間だけ true にする
    if (other.gameObject == Target)
    {
      isTargeted = true;
      Debug.Log("Right Foot Targeted");

    }
  }
}
