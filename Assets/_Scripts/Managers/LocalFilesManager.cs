using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;


namespace Lym {

    public class LocalFilesManager : MonoBehaviour
    {
        public static LocalFilesManager instance;

        private string path;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            } else
            {
                Destroy(this.gameObject);
            }
        }

        private void Start()
        {
            path = Application.persistentDataPath + "/users/";
        }

        public bool CharacterSaveExists(User user)
        {
            return File.Exists(path + user.id + ".txt");
        }

        public void SaveCharacter()
        {
            User user = FirebaseManager.instance.userData;

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(path + user.id + ".txt");

            if (user.character != null)
            {
                bf.Serialize(file, user.character);
            }
            else
            {
                bf.Serialize(file, new Character());
                user.character = new Character();
            }

            file.Close();
        }

        public void LoadCharacter()
        {
            User user = FirebaseManager.instance.userData;

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open((path + user.id + ".txt"), FileMode.Open);
            user.character = (Character)bf.Deserialize(file);
            file.Close();
        }



    }

}
