using System.Collections;
using UnityEngine;

    public class LocationServicesUtility : MonoBehaviour
    {

        public static LocationServicesUtility instance;

        public float latitude = 0;
        public float longitude = 0;

        // set up singleton
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(instance);
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(InitLocationServices());
        }

        public bool UpdateGPSData()
        {
            if (Input.location.status.Equals(LocationServiceStatus.Running))
            {
                // We now have access to GPS values
                Debug.Log("GPS Working");

                latitude = Input.location.lastData.latitude;
                longitude = Input.location.lastData.longitude;
                // altitude
                // horizontal accuracy

                return true;

            }
            else
            {

                return false;
            }

        }

        private IEnumerator InitLocationServices()
        {

            //only in unity editor
#if UNITY_EDITOR
            yield return new WaitWhile(() => !UnityEditor.EditorApplication.isRemoteConnected);
            yield return new WaitForSecondsRealtime(5f);
#endif

#if UNITY_ANDROID

            // seek permissions
            if (!UnityEngine.Android.Permission.HasUserAuthorizedPermission(UnityEngine.Android.Permission.FineLocation))
            {

                UnityEngine.Android.Permission.RequestUserPermission(UnityEngine.Android.Permission.FineLocation);
                UnityEngine.Android.Permission.RequestUserPermission(UnityEngine.Android.Permission.CoarseLocation);

            }

            if (!Input.location.isEnabledByUser)
            {
                Debug.Log("Android, and Location not enabled");
                PopupUtility.instance.DisplayPopup("You have not enabled location services for LYM");
                yield break;
            }

#elif UNITY_IOS

        if(!UnityEngine.Input.location.isEnabledByPlayer){
            
            Debug.Log("IOS, and location not enabled;
            yield break;
        }

#endif

            // Attempt to start location services
            Input.location.Start();

            int maxWait = 20;

            // wait for location services to initialize
            while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
            {
                yield return new WaitForSeconds(1);
                maxWait--;
            }

            // this is an extra wait due to a potential bug in the editor preventing initialization
#if UNITY_EDITOR
            int editorMaxWait = 15;

            while (Input.location.status == LocationServiceStatus.Stopped && editorMaxWait > 0)
            {
                yield return new WaitForSecondsRealtime(1);
                editorMaxWait--;
            }

#endif

            // if location services took too long to initialize (20 seconds)
            if (maxWait < 1)
            {
                Debug.Log("GPS Timeout");
                PopupUtility.instance.DisplayPopup("Location services timed out");
                yield break;
            }

            // connection to location services failed
            if (Input.location.status == LocationServiceStatus.Failed)
            {

                Debug.Log("Cannot get device location");
                PopupUtility.instance.DisplayPopup("Connection to location services failed");
                yield break;
            }
            else
            {
                Debug.Log("GPS Working");
                Debug.Log(Input.location.status);
                Debug.Log(Input.location.lastData.latitude);
                Debug.Log(Input.location.lastData.longitude);
                
                // Access Granted

            }

        }

    }