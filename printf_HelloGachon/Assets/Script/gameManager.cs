using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using Cinemachine;

public class gameManager : MonoBehaviour
{
    private CinemachineVirtualCamera cmCamera;    
    public TalkManager talkManager;
    public QuestManager questManager;
    public GameObject talkPanel;
    public GameObject talkPanel2;
    public GameObject mudangDown;
    public Text talkText;
    public Text talkText2;
    public GameObject scanObject;
    public bool isAction;
    public bool objectDetect = false; 
    public int talkIndex;
    public Image portraitImg;
    public GameObject cmVcam;   // cm카메라 오브젝트
    private int num=0;


  
    void Awake() {
        cmCamera = cmVcam.GetComponent<CinemachineVirtualCamera>();
       
        Debug.Log(questManager.CheckQuest());
    }

    void Start(){
        TestSub();
    }


    public void TestSub(){
        string talkData2 = talkManager.GetTalk(7000, talkIndex);

        //End Talk
        if(talkData2 ==null){
            objectDetect = false;
            talkIndex = 0;
            talkPanel.SetActive(false);
            // Debug.Log(questManager.CheckQuest(id));
            return; //void 에서 return 가능(강제 종료 기능)-> return 뒤에 아무것도 안쓰면 됌

        }
        
        talkText.text = talkData2.Split(':')[0];
        portraitImg.sprite = talkManager.GetPortrait(7000,int.Parse(talkData2.Split(':')[1]));
        portraitImg.color = new Color(1,1,1,1); //맨 끝이 투명도로, npc일때만 이미지가 나오도록 설정

        objectDetect = true;

        talkPanel.SetActive(true);
        talkIndex++;
        
    }
    
    // 조사대상이 있을 때만 대화창 띄우기
    public void Action(GameObject scanObj)
    {
       
        scanObject = scanObj;
        ObjectData objData =scanObject.GetComponent<ObjectData>();
        Talk(objData.id, objData.isNpc);
        
        talkPanel.SetActive(isAction);
    }

    public void Talk(int id, bool isNpc)
    {
        //Set Talk Data
        int questTalkIndex = questManager.GetQuestTalkIndex(id);
        string talkData = talkManager.GetTalk(id+questTalkIndex, talkIndex);

        //End Talk
        if(talkData ==null){
            isAction = false;
            talkIndex = 0;
            Debug.Log(questManager.CheckQuest(id));
            return; //void 에서 return 가능(강제 종료 기능)-> return 뒤에 아무것도 안쓰면 됌
        }

        if(isNpc){
            talkText.text = talkData.Split(':')[0];
            portraitImg.sprite = talkManager.GetPortrait(id,int.Parse(talkData.Split(':')[1]));
            portraitImg.color = new Color(1,1,1,1); //맨 끝이 투명도로, npc일때만 이미지가 나오도록 설정
        }else{
            talkText.text = talkData;
            portraitImg.color = new Color(1,1,1,0);
        }

        isAction = true;
        talkIndex++;
    }
 //마우스 클릭시 퀘스트 마크가 팝업
    public void questionMark1(){
        talkPanel2.SetActive(true);
        talkText2.text = "무당이를 타면 더 빨리 이동할 수 있습니다.";
       
    }
 //마우스 클릭 후 떼어낼때 퀘스트 마크가 팝다운
    public void questionMark2(){
        talkPanel2.SetActive(false);
       
       
    }
//마우스 클릭시 무당이를 내림
    public void noneMudang(){
        mudangDown.SetActive(false);
    }


    public void SetCameraTarget(GameObject followTarget)
    {
        if(cmCamera == null)
        {
            Debug.Log("cmCamera is null");                        
        }

        if(followTarget == null) Debug.Log("followTarget is null");
        cmCamera.Follow = followTarget.transform;
        cmCamera.LookAt = followTarget.transform;        
    }    

}