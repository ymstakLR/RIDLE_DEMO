//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using YakSE;

//public class SettingManager_1 : MonoBehaviour {
//    public static SettingManager_1 Instance { get; private set; }
//    bool open = true;

//    VolumeController MainSoundVolume;
//    VolumeController MusicVolume;

//    public Transform[] page;
//    public GameObject[] ScrollViews;

//    public Text BackText;
//    public Text PageText;

//    Text[] KeyConfigButtonText;

//    int pageNum = 0;

//    public bool Ready = false;

//    void Awake() {
//        if (Instance) { Debug.LogError("ï°êîÇ†ÇËÇ‹Ç∑ÅB", transform); }
//        Instance = this;
//    }

//    void Start() {
//        StartCoroutine(SetUp());

//        KeyConfig.Set();
//    }

//    void Update() {
//        if (Ready) {
//            if (Input.GetKeyDown(KeyCode.Escape)) {
//                Open = !open;
//            }


//            bool isSave = false;

//            if (FileManager_1.Instance.Setting.MainVolume != MainSoundVolume.Value) {
//                isSave = true;
//                FileManager_1.Instance.Setting.MainVolume = MainSoundVolume.Value;
//                SoundPlayer.Instance.Volume = FileManager_1.Instance.Setting.MainVolume * FileManager_1.Instance.Setting.MusicVolume;
//            }

//            if (FileManager_1.Instance.Setting.MusicVolume != MusicVolume.Value) {
//                isSave = true;
//                FileManager_1.Instance.Setting.MusicVolume = MusicVolume.Value;
//                SoundPlayer.Instance.Volume = FileManager_1.Instance.Setting.MainVolume * FileManager_1.Instance.Setting.MusicVolume;
//            }

//            if (isSave) {
//                FileManager_1.SettingClass.Save();
//            }
//        }
//    }

//    IEnumerator SetUp() {
//        yield return null;

//        int num = 0;

//        ButtonController addButton(string name, UnityEngine.Events.UnityAction ev, int _page) {
//            ButtonController script = Instantiate(UIManager.Instance.ButtonControllerPrefab, transform);
//            script.Set(name, ev);
//            script.Rect.anchoredPosition = new Vector2(0, num);
//            num -= UIManager.Instance.ButtonHightSpace;
//            script.transform.SetParent(page[_page]);
//            return script;
//        }

//        BackText.text = "\nOption\n" + "PlayCount:" + FileManager_1.Instance.Setting.PlayCount + " PlayTime:" + FileManager_1.Instance.Setting.PlayTime;

//        // sound volume
//        yield return null;
//        MainSoundVolume = Instantiate(UIManager.Instance.VolumeControllerPrefab, page[0]);
//        MainSoundVolume.Set();
//        MainSoundVolume.Rect.anchoredPosition = new Vector2(0, num - 5);
//        MainSoundVolume.Value = FileManager_1.Instance.Setting.MainVolume;
//        MainSoundVolume.Text.Text.text = "MainSoundVolume";
//        num -= UIManager.Instance.VolumeControllerSpace;

//        yield return null;
//        MusicVolume = Instantiate(UIManager.Instance.VolumeControllerPrefab, page[0]);
//        MusicVolume.Set();
//        MusicVolume.Rect.anchoredPosition = new Vector2(0, num - 5);
//        MusicVolume.Value = FileManager_1.Instance.Setting.MusicVolume;
//        MusicVolume.Text.Text.text = "MusicVolume";
//        num -= UIManager.Instance.VolumeControllerSpace;

//        yield return null;

//        //page2:keyconfig
//        int keyNum = System.Enum.GetNames(typeof(KeyConfig_1.Key)).Length;
//        num = 50;
//        page[1].GetComponent<RectTransform>().sizeDelta += new Vector2(0, UIManager.Instance.ButtonHightSpace * keyNum * 1.5f);
//        KeyConfigButtonText = new Text[keyNum];
//        for (int i = 0; i < keyNum; i++) {
//            ButtonController controller = addButton(System.Enum.GetName(typeof(KeyConfig_1.Key), i).ToString() + ":" + KeyConfig_1.KeyCodes[i], ButtonController.blank, 1);
//            int tmp = i;
//            controller.button.onClick.AddListener(() => { StartChangeKeyConfig(tmp); });
//            KeyConfigButtonText[i] = controller.GetComponentInChildren<Text>();
//        }


//        for (int i = 0; i < transform.childCount; i++) {
//            transform.GetChild(i).gameObject.SetActive(true);
//        }
//        PageNum = 0;

//        Open = false;
//        Ready = true;

//        //

//        SoundPlayer.Instance.Volume = FileManager.Instance.Setting.MainVolume * FileManager.Instance.Setting.MusicVolume;
//    }

//    // keyconfig
//    public void StartChangeKeyConfig(int _num) {
//        StartCoroutine(ChangeKeyConfig(_num));
//    }

//    IEnumerator ChangeKeyConfig(int _num) {
//        Debug.Log("ChangeKeyConfig:" + _num.ToString());
//        while (true) {
//            yield return null;
//            if (Input.anyKeyDown) {
//                foreach (KeyCode code in System.Enum.GetValues(typeof(KeyCode))) {
//                    if (Input.GetKeyDown(code)) {
//                        KeyConfig_1.KeyCodes[_num] = code;
//                    }
//                }
//                KeyConfig_1.Save();
//                KeyConfigButtonText[_num].text = System.Enum.GetName(typeof(KeyConfig_1.Key), _num).ToString() + ":" + KeyConfig_1.KeyCodes[_num];
//                break;
//            }
//        }
//    }

//    //
//    public bool Open {
//        get { return open; }
//        set {
//            if (open != value) {
//                if (value) {
//                    GameManager.Instance.CloseUIAction = Close;
//                } else {
//                    GameManager.Instance.CloseUIAction = null;
//                }

//                gameObject.SetActive(value);
//                open = value;
//            }
//        }
//    }

//    public int PageNum {
//        get { return pageNum; }
//        set {
//            pageNum = Mathf.Abs(value + page.Length) % page.Length;
//            for (int i = 0; i < page.Length; i++) {
//                ScrollViews[i].SetActive(pageNum == i);
//            }
//            switch (pageNum) {
//                case 0:
//                    PageText.text = "Sound";
//                    break;
//                case 1:
//                    PageText.text = "KeyConfig";
//                    break;
//                case 2:
//                    PageText.text = "Graphic";
//                    break;
//            }
//        }
//    }

//    public void NextPage() {
//        PageNum++;
//    }
//    public void PrevPage() {
//        PageNum--;
//    }

//    public void Close() {
//        Open = false;
//    }

//    public void OpenFunc() {
//        Open = true;
//    }
//}



