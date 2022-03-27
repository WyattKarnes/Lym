using System.Collections;
using UnityEngine;

public class LocationServicesUtility : MonoBehaviour
{

    public static LocationServicesUtility instance;

    public float latitude = 0;
    public float longitude = 0;

    public bool isPC = false;

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
        if (Input.location.status == LocationServiceStatus.Running)
        {
            // We now have access to GPS values
            Debug.Log("GPS Working");

            latitude = Input.location.lastData.latitude;
            longitude = Input.location.lastData.longitude;
            // altitude
            // horizontal accuracy

            Debug.Log("latitude: " + latitude + " Longitude: " + longitude);
            return true;

        }
        else
        {
            // GPS Services have stopped
            Debug.Log("GPS Services have not started.");
            return false;
        }

    }

    private IEnumerator InitLocationServices()
    {
        // wait for Unity Remote when testing that way
#if UNITY_EDITOR

        yield return new WaitWhile(() => !UnityEditor.EditorApplication.isRemoteConnected);
        yield return new WaitForSecondsRealtime(5f);

#endif

        // now we are going to actually initialize location services
#if UNITY_EDITOR
        //nothing to do here
#elif UNITY_ANDROID

        //seek permissions
        if(!Android.Permission.HasUserAuthorizedPermission(Android.Permission.CoarseLocation))
        {

            Android.Permission.RequestUserPermission(Android.Permission.CoarseLocation);

        }

        if (!Input.location.isEnabledByUser)
        {
            Debug.Log("Android, and Location not enabled");
            yield break;
        }

#elif UNITY_IOS

        if (!Input.location.isEnabledByUser)
        {
            Debug.Log("Location not enabled");
            yield break;
        }

#endif

        // start location service and then query
        Input.location.Start(5, 5);

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
            yield break;
        }

        // connection to location services failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {

            Debug.Log("Cannot get device location");
            yield break;
        }
        else
        {
            Debug.Log("GPS Working");
            Debug.Log(Input.location.lastData.latitude);
            Debug.Log(Input.location.lastData.longitude);
            // Access Granted

        }

    }

}
