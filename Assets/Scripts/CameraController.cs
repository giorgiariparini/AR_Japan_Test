using UnityEngine;

namespace Assets.Scripts
{
    class CameraController : MonoBehaviour
    {
        [SerializeField] private float _rotateSpeed = 100.0f;
        public float angleMax = 90.0f;

        public Transform target;

        [SerializeField] private float _distanceMax = 1.5f;
        [SerializeField] private float _distanceMin = 0.8f;

        private Vector3 initialVector = Vector3.forward;

        // Use this for initialization
        void Start()
        {
            initialVector = transform.position - target.position;
            initialVector.y = 0;
        }

        // Update is called once per frame
        void Update()
        {
            float rotateDegrees = 0f;
            rotateDegrees += Input.GetAxis("Mouse X") * _rotateSpeed * Time.deltaTime;

            Vector3 currentVector = transform.position - target.position;
            var distance = currentVector.magnitude;

            if (Input.touchSupported)
            {
                // Pinch to zoom
                if (Input.touchCount == 2)
                {

                    // get current touch positions
                    Touch tZero = Input.GetTouch(0);
                    Touch tOne = Input.GetTouch(1);
                    // get touch position from the previous frame
                    Vector2 tZeroPrevious = tZero.position - tZero.deltaPosition;
                    Vector2 tOnePrevious = tOne.position - tOne.deltaPosition;

                    float oldTouchDistance = Vector2.Distance(tZeroPrevious, tOnePrevious);
                    float currentTouchDistance = Vector2.Distance(tZero.position, tOne.position);

                    // get offset value
                    float touchDeltaDistance = oldTouchDistance - currentTouchDistance;
                    if (touchDeltaDistance > 0f && distance < _distanceMax) // backwards
                    {
                        transform.position *= 1.2f;
                    }
                    else if (touchDeltaDistance < 0f && distance > _distanceMin) // forward
                    {
                        transform.position /= 1.2f;
                    }
                }
            } 
            else
            {
                var scroll = Input.GetAxis("Mouse ScrollWheel");

                if (scroll > 0f && distance < _distanceMax) // backwards
                {
                    transform.position *= 1.2f;
                }
                else if (scroll < 0f && distance > _distanceMin) // forward
                {
                    transform.position /= 1.2f;
                }
            }

            currentVector.y = 0;
            float angleBetween = Vector3.Angle(initialVector, currentVector) * (Vector3.Cross(initialVector, currentVector).y > 0 ? 1 : -1);
            float newAngle = Mathf.Clamp(angleBetween + rotateDegrees, -angleMax, angleMax);
            rotateDegrees = newAngle - angleBetween;

            transform.RotateAround(target.position, Vector3.up, rotateDegrees);
            transform.LookAt(target);
        }
    }
}