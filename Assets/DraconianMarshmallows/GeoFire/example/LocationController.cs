using com.draconianmarshmallows.geofire;
using Firebase;
using Firebase.Database;
//using Firebase.Unity.Editor;
using UnityEngine;

public class LocationController : MonoBehaviour
{
    [SerializeField] private string firebaseDbUrl;

    private GeoQuery query;

	private void Start()
    {
        // Used for testing in Unity editor::
        // FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(firebaseDbUrl);

        // Setup DB references and a GeoFire instance::
        var mainDbReference = FirebaseDatabase.DefaultInstance.RootReference;
        var geofireDbReference = mainDbReference.Child("geofire");
        var geoFire = new GeoFire(geofireDbReference);

        // Store test locations::
        Debug.Log("Storing locations...");

        geoFire.setLocation("enemy_1", new GeoLocation(37.589680, -122.477000), null);
        geoFire.setLocation("enemy_2", new GeoLocation(37.589694, -122.477111), null);

        // Setup query to find objects in a radius around a location::
        Debug.Log("Querying objects in an error...");

        query = geoFire.queryAtLocation(new GeoLocation(37.589694, -122.477111), .6d);

        query.geoQueryReadyListeners += onGeoQueryReady;
        query.geoQueryErrorListeners += onGeoQueryError;
        query.keyEnteredListeners += onKeyEntered;
        query.keyExitedListeners += onKeyExited;
        query.keyMovedListeners += onKeyMoved;

        query.initializeListeners();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.M)) // For testing objects exited radius. 
        {
            var center = query.getCenter();
            center.longitude += .00005D;
            query.setCenter(center);
            //Debug.Log("Moving query location to : " + center.toString());
        }
    }

    private void onGeoQueryReady()
    {
        Debug.Log("Geo-query ready.");
    }

    private void onGeoQueryError(DatabaseError error)
    {
        Debug.LogError("Geo query error: " + error);
    }

    private void onKeyEntered(string key, GeoLocation location)
    {
        Debug.LogFormat("Geo query ENTER: {0} :: {1}", key, location.toString());
    }

    private void onKeyExited(string key)
    {
        Debug.Log("Geo query EXITED : " + key);
    }

    private void onKeyMoved(string key, GeoLocation location)
    {
        Debug.LogFormat("Geo query moved: {0} :: {1}", key, location);
    }
}
