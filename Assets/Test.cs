using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class Test : MonoBehaviour
{
    public Text text;
    MyZip zip = new MyZip();
    string directory;
    void Start()
    {
        text = text.GetComponent<Text>();
        StartCoroutine(Copy());
    }

    string testFileLength;
    void OnGUI()
    {
        if (GUI.Button(new Rect(0, 0, Screen.width * 0.2f, Screen.height * 0.1f), "zip"))
            zip.ZipFile(Application.persistentDataPath + "/original", Application.persistentDataPath + "/zip/test.zip");
        if (GUI.Button(new Rect(0, Screen.height * 0.2f, Screen.width * 0.2f, Screen.height * 0.1f), "unzip"))
            zip.UnZipFile(Application.persistentDataPath + "/zip/test.zip", Application.persistentDataPath + "/unzip");


        GUI.Label(new Rect(Screen.width * 0.9f, 0, Screen.width * 0.2f, Screen.height * 0.1f), testFileLength);

    }
    float testProgressOverall;
    float testProgress;
    void Update()
    {
        if ((testProgressOverall != zip.progressOverall) || (testProgress != zip.progress))
        {
            testProgressOverall = zip.progressOverall;
            testProgress = zip.progress;
            text.text = string.Format("总进度: {0}%,单个文件进度: {1}%", zip.progressOverall, zip.progress);
        }
    }
    /// <summary>
    /// for test
    /// </summary>
    /// <returns></returns>
    IEnumerator Copy()
    {
#if UNITY_EDITOR || UNITY_IPHONE
        string path = "file://" + Application.streamingAssetsPath + "/test.pdf";
#else
        string path =  Application.streamingAssetsPath + "/test.pdf";
#endif
        WWW www = new WWW(path);
        while (!www.isDone)
            yield return www;
        if (Directory.Exists(Application.persistentDataPath + "/original"))
            Directory.Delete(Application.persistentDataPath + "/original", true);
        Directory.CreateDirectory(Application.persistentDataPath + "/original");
        if (File.Exists(Application.persistentDataPath + "/original/test.pdf"))
            File.Delete(Application.persistentDataPath + "/original/test.pdf");
        using (FileStream stream = File.Create(Application.persistentDataPath + "/original/test.pdf"))
        {
            stream.Write(www.bytes, 0, www.bytes.Length);
            stream.Close();
        }
        if (File.Exists(Application.persistentDataPath + "/original/test.pdf"))
        {
            FileInfo fileInfo = new FileInfo(Application.persistentDataPath + "/original/test.pdf");
            testFileLength = fileInfo.Length.ToString();
        }
    }
}