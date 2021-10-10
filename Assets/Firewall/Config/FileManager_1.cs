using UnityEngine;
using System.IO;

//�@Json�t�@�C���̑���E����ŁH
// �ړ����@2019�N12��23��
// �ړ����@2020�N4��6��

public class FileManager_1 {
    static FileManager_1 instance = null;
    string filePath = "";

    SettingClass setting;

    public readonly string extension = ".json";

    void Set() // �������B
    {   // �t�H���_�̊m�F�B
        if (!Directory.Exists(filePath)) {
            Debug.Log("FileManager�F����N���ł��B�t�H���_���쐬���܂��B�t�@�C���̕ۑ��ꏊ��:" + filePath);
            Directory.CreateDirectory(filePath);
        }

        SettingClass.Load();
        Setting.PlayCount++;
        Setting.PlayTime += Setting.PlayEndTime;
        SettingClass.Save();
    }

    // public
    public static FileManager_1 Instance {   // �ȈՃV���O���g��
        get {
            if (instance == null) {
                instance = new FileManager_1();
            }
            return instance;
        }
    }

    public string FilePath {   // �p�X���擾�ł��܂��B
        get {
            if (filePath == "") {
#if PrefabArrangement
                Log("�����������܂��B");
#endif
                filePath = Application.dataPath + "/.SaveData/";
#if PrefabArrangement
                Log("�t�@�C���̕ۑ��ꏊ��:" + filePath);
#endif
                Set();
            }
            return filePath;
        }

        set {
            filePath = value;
            Set();
#if PrefabArrangement
            Log("�t�@�C���̏ꏊ��ύX���܂����F" + filePath);
#endif
        }
    }

    public string[] FileList() {   // �t�H���_�����擾�B
        string[] _strs = Directory.GetFiles(FilePath, "*" + extension);
        for (int i = 0; i < _strs.Length; i++) {
            _strs[i] = Path.GetFileNameWithoutExtension(_strs[i]);
        }
        return _strs;
    }

    public void SaveFile<Type>(Type save, string _fileName) {
        string json = JsonUtility.ToJson(save);

        StreamWriter streamWriter = new StreamWriter(FilePath + _fileName + extension);
        streamWriter.Write(json);
        streamWriter.Flush();
        streamWriter.Close();

#if PrefabArrangement
        Log("save file>" + _fileName + " - " + save.ToString());
#endif
    }

    public bool LoadFile<Type>(ref Type type, string _fileName) {
        if (File.Exists(FilePath + _fileName + extension)) {
            StreamReader streamReader;
            streamReader = new StreamReader(FilePath + _fileName + extension);
            string data = streamReader.ReadToEnd();
            streamReader.Close();

            type = JsonUtility.FromJson<Type>(data);
#if PrefabArrangement
            Debug.Log("load file>" + _fileName + " - " + type.ToString());
#endif
            return true;
        }
        return false;
    }

    public bool IsFile(string _str) {
        return File.Exists(FilePath + _str + extension);
    }

#if PrefabArrangement
    public void Log(string _str)
    {
        Debug.Log("PrefabArrangement:" + _str);
    }
#endif

    public SettingClass Setting {
        get {
            if (setting == null) { SettingClass.Load(); }
            return setting;
        }
    }

    public class SettingClass {   // �I�v�V�����i�[�N���X�B
        static readonly string filename = "setting";

        // playData
        public int PlayCount = 0;
        public float PlayTime = 0;
        // �N�����ɑO��̎��Ԃ̍��𑫂��B
        public float PlayEndTime = 0;

        //sound
        public float MainVolume = 1.0f;
        public float MusicVolume = 1.0f;



        // static
        static public void Save() {
            Instance.SaveFile<SettingClass>(Instance.setting, filename);
        }

        static public void Load() {
            if (!Instance.LoadFile<SettingClass>(ref Instance.setting, filename)) {
                Instance.setting = new SettingClass();
            }
        }
    }
}