using UnityEngine;
using System.Collections;

namespace Logic
{
    public class CameraSmoothFollow : MonoBehaviour
    {
        [SerializeField]
        private float _followSpeed;

        private GameObject _targetObject;

        private void Awake()
        {
            
        }

        public void SetTarget(GameObject target)
        {
            this._targetObject = target;
        }

        private void FixedUpdate()
        {
            if(_targetObject == null)
            {
                return;
            }

            Vector3 objectivePosition = _targetObject.transform.position;
            objectivePosition.y = 0;
            objectivePosition.z = 0;

            objectivePosition.x += 3f;
            Vector3 cameraPosition2D = transform.position;
            cameraPosition2D.z = 0;

            Vector3 diffPosition = objectivePosition - cameraPosition2D;

            transform.Translate(diffPosition * _followSpeed * Time.deltaTime, Space.World);
        }
    }
}