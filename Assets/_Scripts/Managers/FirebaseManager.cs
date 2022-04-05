using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Auth;
using com.draconianmarshmallows.geofire;
using com.draconianmarshmallows.geofire.core;
using TMPro;
using System;

namespace Lym
{
    public class FirebaseManager : MonoBehaviour
    {
        public static FirebaseManager instance;

        // Firebase variables
        [Header("Firebase")]
        public DependencyStatus dependencyStatus;
        public FirebaseAuth auth;
        public FirebaseUser user;
        public DatabaseReference mainDBReference;
        public DatabaseReference geofireDBReference;
        public GeoFire geoFire;
        public GeoQuery query;

        // Login UI Variables
        [Header("Login")]
        public TMP_InputField emailLoginField;
        public TMP_InputField passwordLoginField;
        public TMP_Text warningLoginText;
        public TMP_Text confirmLoginText;

        // Registration UI Variables
        [Header("Register")]
        public TMP_InputField usernameRegisterField;
        public TMP_InputField emailRegisterField;
        public TMP_InputField passwordRegisterField;
        public TMP_InputField passwordRegisterVerifyField;
        public TMP_Text warningRegisterText;

        // User Page Fields
        public User userData;

        // Events
        public event UserMessagesLoaded OnUserMessagesLoaded;
        public event NearbyMessagesLoaded OnNearbyMessagesLoaded;

        private void Awake()
        {
            // singleton setup
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != null)
            {
                Debug.Log("Instance already exists, destroying object!");
                Destroy(this);
            }

        }

        private void Start()
        {
            StartCoroutine(CheckAndFixDependencies());
        }

        #region Setup

        private IEnumerator CheckAndFixDependencies()
        {
            var checkAndFixDependenciesTask = FirebaseApp.CheckAndFixDependenciesAsync();

            yield return new WaitUntil(predicate: () => checkAndFixDependenciesTask.IsCompleted);

            var dependencyResult = checkAndFixDependenciesTask.Result;

            if(dependencyResult == DependencyStatus.Available)
            {
                InitializeFirebase();
            } else
            {
                Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyResult}");
            }
        }

        private void InitializeFirebase()
        {
            Debug.Log("Setting up Firebase Auth");

            auth = FirebaseAuth.DefaultInstance;
            auth.StateChanged += AuthStateChanged;
            AuthStateChanged(this, null);
            StartCoroutine(CheckAutoLogin());

            mainDBReference = FirebaseDatabase.DefaultInstance.RootReference;

            geofireDBReference = mainDBReference.Child("geofire");
            geoFire = new GeoFire(geofireDBReference);

            
        }

        private IEnumerator CheckAutoLogin()
        {
            yield return new WaitForEndOfFrame();

            if(user != null)
            {
                var reloadUserTask = user.ReloadAsync();

                yield return new WaitUntil(() => reloadUserTask.IsCompleted);

                AutoLogin();
            } else
            {
                // no user, normal login
            }
        }

        private void AutoLogin()
        {
            if(user != null)
            {
                // go to homepage
            } else
            {
                // go to login
            }
        }

        private void AuthStateChanged(object sender, EventArgs e)
        {
            if(auth.CurrentUser != user)
            {
                bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;

                if(!signedIn && user != null)
                {
                    Debug.Log("Signed out");
                    // any logic that should happen when the player logs out
                }

                user = auth.CurrentUser;

                if (signedIn)
                {
                    Debug.Log($"Signed in: {user.DisplayName}");
                }
            }
        }

        #endregion

        #region Buttons

        public void LoginButton()
        {
            StartCoroutine(Login(emailLoginField.text, passwordLoginField.text));
        }

        public void LogoutButton()
        {
            auth.SignOut();
            UIManager.instance.LoginScreen();
            //clear registration fields
            //clear login fields
        }

        public void RegisterButton()
        {
            StartCoroutine(Register(emailRegisterField.text, passwordRegisterField.text, usernameRegisterField.text));
        }

        #endregion

        #region Login Functions

        private IEnumerator Login(string email, string password)
        {
            // Call the Firebase auth sign in function, passing the email and password
            var LoginTask = auth.SignInWithEmailAndPasswordAsync(email, password);

            // Wait until the task is completed
            yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);

            if (LoginTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to register task with {LoginTask.Exception}");
                FirebaseException firebaseEx = LoginTask.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

                string message = "Login Failed!";

                switch (errorCode)
                {
                    case AuthError.MissingEmail:
                        message = "Missing Email";
                        break;

                    case AuthError.MissingPassword:
                        message = "Missing Password";
                        break;

                    case AuthError.WrongPassword:
                        message = "Wrong Password";
                        break;

                    case AuthError.InvalidEmail:
                        message = "Invalid Email";
                        break;

                    case AuthError.UserNotFound:
                        message = "User not found";
                        break;
                }

                // Display the error to the user
                warningLoginText.text = message;

            }
            else
            {
                // The user is now logged in
                // Get the result
                user = LoginTask.Result;
                Debug.LogFormat("User signed in successfully: {0} ({1})", user.DisplayName, user.Email);
                warningLoginText.text = "";
                confirmLoginText.text = "Logged In";

                //yield return new WaitForSeconds(2);

                // set up the userData object 
                SetUpUser();

                confirmLoginText.text = "";
            }
        }

        /// <summary>
        /// Get a list of all of the User's messages from the database
        /// </summary>
        /// <returns></returns>
        private void SetUpUser()
        {
            // create the user object (this is just to keep a local reference so we can query the database less)
            userData = new User(user.UserId);


            //go to the home screen
            UIManager.instance.UserHomepageScreen();

        }

        private IEnumerator Register(string email, string password, string username)
        {
            if (username.Equals(""))
            {
                // If the username field is blank, show a warning
                warningRegisterText.text = "Missing Username";

            }
            else if (!passwordRegisterField.text.Equals(passwordRegisterVerifyField.text))
            {
                // If the password cannot be verified, show a warning
                warningRegisterText.text = "Password does not match!";

            }
            else
            {

                // Call the Firebase Auth signin function passing the email and password
                var RegisterTask = auth.CreateUserWithEmailAndPasswordAsync(email, password);

                // Wait until the task is complete
                yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);

                if (RegisterTask.Exception != null)
                {

                    // If there are errors, handle them
                    Debug.LogWarning(message: $"Failed to register task with {RegisterTask.Exception}");
                    FirebaseException firebaseEx = RegisterTask.Exception.GetBaseException() as FirebaseException;
                    AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

                    string message = "Register Failed";

                    switch (errorCode)
                    {
                        case AuthError.MissingEmail:
                            message = "Missing Email";
                            break;

                        case AuthError.MissingPassword:
                            message = "Missing Password";
                            break;

                        case AuthError.WeakPassword:
                            message = "Weak Password";
                            break;

                        case AuthError.EmailAlreadyInUse:
                            message = "Email is already in use";
                            break;

                    }

                    warningRegisterText.text = message;

                }
                else
                {

                    // User has been created
                    // Get the result from Firebase
                    user = RegisterTask.Result;

                    if (user != null)
                    {
                        // Create a user profile and set the username
                        UserProfile profile = new UserProfile { DisplayName = username };

                        // Call the Firebase auth update user profile function and pass the profile with the username
                        var ProfileTask = user.UpdateUserProfileAsync(profile);

                        // Wait for the task to complete
                        yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

                        if (ProfileTask.Exception != null)
                        {
                            Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
                            FirebaseException firebaseEx = ProfileTask.Exception.GetBaseException() as FirebaseException;
                            AuthError errorCode = (AuthError)(firebaseEx.ErrorCode);
                            warningRegisterText.text = "Failed to update Username!";

                        }
                        else
                        {
                            // The Username has been set
                            // Return to login screen
                            UIManager.instance.LoginScreen();
                            warningRegisterText.text = "";
                        }
                    }

                }

            }
        }

        #endregion


        #region User Data Retrieval

        /// <summary>
        /// Retrieve the display name of a user, or most of their email if display name not available.
        /// </summary>
        /// <returns></returns>
        public string GetUsername()
        {
            if (user.DisplayName.Equals(""))
            {
                string trimmedEmail = user.Email.Substring(0, user.Email.IndexOf('@'));
                return trimmedEmail;
            }
            else
            {
                return user.DisplayName;
            }

        }

        /// <summary>
        /// Fetch a user's unique ID
        /// </summary>
        /// <returns></returns>
        public string GetUserID()
        {
            return user.UserId;
        }

        #endregion

        #region MessagePublishing

        /// <summary>
        /// Attempt to generate a message and then begin upload to database. Requires location services to be running. 
        /// </summary>
        public void PublishMessage()
        {
            Message msg = MessageBuilder.instance.GenerateMessage();

            if (msg != null)
            {

                Debug.Log(msg.ToString());
                StartCoroutine(UpdateMessageDatabase(msg));

            }
            else
            {
                Debug.Log("Message failed to generate.");
                PopupUtility.instance.DisplayPopup("Could not publish message, location could not be retrieved.");
            }




        }

        /// <summary>
        /// Upload user message to database.
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        private IEnumerator UpdateMessageDatabase(Message msg)
        {
            // store geofire locations
            geoFire.setLocation(msg.id, new GeoLocation(msg.latitude, msg.longitude), null);

            // create a broad geohash for the message location (which should be the user's current location)
            GeoHash geoHash = new GeoHash(msg.latitude, msg.longitude, 3);

            // get the 3 digit geohash
            string hashy = geoHash.getGeoHashString();

            // store the message in the main database
            var DBTask = mainDBReference.Child("messages").Child(hashy).Child(userData.id).Child(msg.id).SetRawJsonValueAsync(msg.ToString());

            yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

            if (DBTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
                PopupUtility.instance.DisplayPopup("Message failed to register with the database. Please try again later.");
                // if we couldn't create the message, delete the location from geofire
                geoFire.removeLocation(msg.id);
            }
            else
            {
                // Database username is now updated
                PopupUtility.instance.DisplayPopup("Your message was successfully uploaded.");
            }
        }

        #endregion


        #region Message Retrieval

        /// <summary>
        /// Starts a coroutine that downloads user messages from the database. 
        /// </summary>
        public void UpdateUserMessages()
        {
            StartCoroutine(FetchUserMessages());
        }

        /// <summary>
        /// Downloads user messages from database. When complete, notifies HomepageView
        /// </summary>
        /// <returns></returns>
        private IEnumerator FetchUserMessages()
        {
            if (LocationServicesUtility.instance.UpdateGPSData())
            {
                // find out where the user is, get only their messages for current region
                GeoHash geoHash = new GeoHash(LocationServicesUtility.instance.latitude, LocationServicesUtility.instance.longitude, 3);
                string hashy = geoHash.getGeoHashString();

                // create an asynchronus task to retrieve all messages that the user has
                var userMessagesTask = mainDBReference.Child("messages").Child(hashy).Child(user.UserId).GetValueAsync();

                yield return new WaitUntil(predicate: () => userMessagesTask.IsCompleted);

                if (userMessagesTask.Exception != null)
                {
                    // failed to complete task
                    Debug.LogWarning(message: $"Failed to register task with {userMessagesTask.Exception}");
                }
                else
                {
                    // task succeeded

                    // wipe user's current messages so we don't get duplicates
                    userData.ClearMessages();

                    // loop over all of the user's messages
                    foreach (DataSnapshot snap in userMessagesTask.Result.Children)
                    {
                        Debug.Log(snap.GetRawJsonValue() + "From database");
                        Message m = JsonUtility.FromJson<Message>(snap.GetRawJsonValue());
                        userData.AddMessage(m);
                        Debug.Log(snap.GetRawJsonValue() + "After conversion");
                    }

                    PopupUtility.instance.DisplayPopup("Loaded " + userData.messages.Count + " messages.");
                }

                // notify homepage view that messages were loaded
                OnUserMessagesLoaded?.Invoke();


            } else
            {
                PopupUtility.instance.DisplayPopup("Could not retrieve your messages in this region, location services are not working.");
            }
        }

        private IEnumerator FetchNearbyMessage(string messageID)
        {

            if (LocationServicesUtility.instance.UpdateGPSData())
            {
                // get the geohash for the region
                GeoHash geoHash = new GeoHash(LocationServicesUtility.instance.latitude, LocationServicesUtility.instance.longitude, 3);
                string hashy = geoHash.getGeoHashString();

                // get the message's owner ID from the key
                string ownerID = messageID.Substring(0, 28);

                // create an asynchronus task to retrieve all messages in a region
                var nearbyMessageTask = mainDBReference.Child("messages").Child(hashy).Child(ownerID).Child(messageID).GetValueAsync();

                yield return new WaitUntil(predicate: () => nearbyMessageTask.IsCompleted);

                if (nearbyMessageTask.Exception != null)
                {
                    // failed to complete task
                    Debug.LogWarning(message: $"Failed to register task with {nearbyMessageTask.Exception}");
                }
                else
                {
                    // task succeeded

                    // wipe current nearby messages to avoid duplicates
                    NearbyMessageView.instance.ClearMessages();

                    //Debug.Log("From firebase: " + nearbyMessageTask.Result.GetRawJsonValue());

                    Message m = JsonUtility.FromJson<Message>(nearbyMessageTask.Result.GetRawJsonValue());
                    NearbyMessageView.instance.AddMessage(m);

                    //Debug.Log("After conversion: " + m.ToString());
                }
            } else
            {
                PopupUtility.instance.DisplayPopup("Could not retrieve messages in your region, location services are not working.");
            }
        }

        #endregion

        #region Geofire Queries

        public void BeginGeofireQuery()
        {

            if (LocationServicesUtility.instance.UpdateGPSData())
            {

                float lat = LocationServicesUtility.instance.latitude;
                float lng = LocationServicesUtility.instance.longitude;

                // query location in 6 meters
                query = geoFire.queryAtLocation(new GeoLocation(lat, lng), .07f);

                query.geoQueryReadyListeners += OnGeoQueryReady;
                query.geoQueryErrorListeners += OnGeoQueryError;
                query.keyEnteredListeners += OnKeyEntered;
                query.keyExitedListeners += OnKeyExited;
                query.keyMovedListeners += OnKeyMoved;

                query.initializeListeners();

            }
            else
            {

                Debug.Log("Big sad, location not work");

            }
        }

        public void EndGeofireQuery()
        {
            Debug.Log("The query would stop if this was implemented");
        }

        private void OnGeoQueryReady()
        {
            Debug.Log("Geo-query ready.");
        }

        private void OnGeoQueryError(DatabaseError error)
        {
            Debug.LogError("Geo query error: " + error);
            PopupUtility.instance.DisplayPopup("Geo query had a problem.");
        }

        private void OnKeyEntered(string key, GeoLocation location)
        {
            // key substring is 0, 29 to get user id
            Debug.LogFormat("Geo query ENTER: {0} :: {1}", key, location.toString());
            // debug popup
            PopupUtility.instance.DisplayPopup("There is a message within 20 feet of you!");

            // start coroutine for getting nearby messages
            StartCoroutine(FetchNearbyMessage(key));
        }

        private void OnKeyExited(string key)
        {
            Debug.Log("Geo query EXITED : " + key);
            PopupUtility.instance.DisplayPopup("You left the key");
        }

        private void OnKeyMoved(string key, GeoLocation location)
        {
            Debug.LogFormat("Geo query moved: {0} :: {1}", key, location);
        }

        #endregion


        #region Delegates

        public delegate void UserMessagesLoaded();

        public delegate void NearbyMessagesLoaded();

        #endregion

        #region Username Updating (Unused?)

        /// <summary>
        /// Update a user's displayname in the authorization system
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        private IEnumerator UpdateUsernameAuth(string username)
        {
            // Create a user profile and set the username
            UserProfile profile = new UserProfile { DisplayName = username };

            // Update the Firebase user auth
            var profileTask = user.UpdateUserProfileAsync(profile);

            // Wait for the task to complete
            yield return new WaitUntil(predicate: () => profileTask.IsCompleted);

            if (profileTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to register task with {profileTask.Exception}");
            }
            else
            {
                // Auth username is now updated
            }
        }

        /// <summary>
        /// Update a user's displayname in the database
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        private IEnumerator UpdateUsernameDatabase(string username)
        {
            var DBTask = mainDBReference.Child("users").Child(user.UserId).Child("username").SetValueAsync(username);

            yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

            if (DBTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
            }
            else
            {
                // Database username is now updated
            }
        }

        #endregion
    }

}
