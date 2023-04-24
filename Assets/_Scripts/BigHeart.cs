using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BigHeart : MonoBehaviour
{
    [SerializeField] float rotateSpeed = 1f;  
    int canFX = 0;
    bool enableNotFinish =false;
    bool canRotate = true; 
    [SerializeField]GameObject partical;
    
    [SerializeField] GameObject heart;
    private Love targetLove;
    

    Material heartMat;
    private void Awake() {
        heartMat=heart.GetComponent<MeshRenderer>().materials[0];
        heartMat.SetVector("_Dissolve_Dir",new Vector3(0,-2.5f,0));
        //Color heartColor = new Color(255,0,0,0);
        //heartMat.SetColor("Color 0",heartColor);
    }
     void OnEnable() 
     {
        enableNotFinish= true;
        heartMat.DOVector(new Vector3(0,0,0),"_Dissolve_Dir",2f);
        heartMat.DOFade(.6f,"_BaseColor",2f).OnComplete(()=>{enableNotFinish=false;});
    }
    public void UnLove() 
    {
        heartMat.DOFade(0f,"_BaseColor",2f);
        heartMat.DOVector(new Vector3(0,2.5f,0),"_Dissolve_Dir",2f).OnComplete(()=>{Destroy(this.gameObject);});
        
    }
    public void Broken() 
    {
        //heartMat.DOFade(0f,"_BaseColor",2f);
        //heartMat.DOVector(new Vector3(0,-2.5f,0),"_Dissolve_Dir",2f).OnComplete(()=>{Destroy(this.gameObject);});
        
    }
    public void Broken(bool broke)
    {
        if(enableNotFinish==true)
        {
            canFX = (broke==true)?2:1;
        }
        else
        {

             if(broke == true)
        {
            canRotate=false;
            heartMat.SetVector("_Dissolve_Dir",new Vector3(1.5f,0,0));
            GameObject heart2 = Instantiate(heart,this.transform);
            heart2.transform.position=heart.transform.position;
            heart2.transform.rotation=heart.transform.rotation;
            heart2.transform.Rotate(new Vector3(0,180f,0),Space.World);
            Material heart2Mat = heart2.GetComponent<MeshRenderer>().materials[0];
            heartMat.DOVector(new Vector3(3f,0,0),"_Dissolve_Dir",2f).OnComplete(()=>{
                heartMat.SetFloat("_Dissolve",0.5f);
                heartMat.DOFloat(0f,"_Dissolve",2f).OnComplete(()=>{
                    Destroy(heart.gameObject);
                });
                });
            heartMat.SetFloat("_Dissolve",0.5f);
            heart2Mat.DOVector(new Vector3(3f,0,0),"_Dissolve_Dir",2f).OnComplete(()=>{
                heart2Mat.DOFloat(0f,"_Dissolve",2f).OnComplete(()=>{
                    Destroy(heart2.gameObject);Destroy(this.gameObject);
                });
                });
            heart2Mat.SetFloat("_Dissolve",0.5f);
        }
        else
        {
            partical.SetActive(true);
            LevelManager.Instance.currentScore=LevelManager.Instance.currentScore+1;
            Debug.Log("Score+1");
        }
        }
    
    }
    // Update is called once per frame
    void Update()
    {
        if(canFX> 0 && enableNotFinish==false)
        {
            bool b = (canFX>1)?true:false;
            Broken(b);
            if(canFX!=0)canFX=0;
        }
        if(canRotate)this.transform.Rotate(Vector3.up*rotateSpeed*Time.deltaTime,Space.World);
        
    }
}
