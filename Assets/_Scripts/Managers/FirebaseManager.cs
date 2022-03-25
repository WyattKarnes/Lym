using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using TMPro;

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

            // We are issuing an asynchronous task to check dependencies so Firebase can work. 

            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
            {
                dependencyStatus = task.Result;

                if (dependencyStatus == DependencyStatus.Available)
                {
                // If Firebase can be initialized on this device: 
                InitializeFirebase();

                }
                else
                {
                    Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);           
                }


            });

            
        }

        private void InitializeFirebase()
        {
            Debug.Log("Setting up Firebase Auth");
            auth = FirebaseAuth.DefaultInstance;
        }

        public void LoginButton()
        {
            StartCoroutine(Login(emailLoginField.text, passwordLoginField.text));
        }

        public void RegisterButton()
        {
            StartCoroutine(Register(emailRegisterField.text, passwordRegisterField.text, usernameRegisterField.text));
        }

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

                yield return new WaitForSeconds(2);

                // Swap to the user data view

                UIManager.instance.UserHomepageScreen();
                confirmLoginText.text = "";
            }
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


        public string GetUsername()
        {
            if (user.DisplayName.Equals(""))
            {
                string trimmedEmail = user.Email.Substring(0, user.Email.IndexOf('@'));
                return trimmedEmail;
            } else
            {
                return user.DisplayName;
            }
            
        }

        public string GetUserID()
        {
            return user.UserId;
        }

        // functions to publish a message to the database

    }

}
