using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Heart : MonoBehaviour
{
    [SerializeField] float rotateSpeed = 1f;  
    [SerializeField] GameObject heart;
    public Transform target;
    private Love targetLove;
    [SerializeField]AudioClip aC;

    Material heartMat;
    private void Awake() {
        heartMat=heart.GetComponent<MeshRenderer>().materials[0];
        heartMat.SetVector("_Dissolve_Dir",new Vector3(0,2.5f,0));
        //Color heartColor = new Color(255,0,0,0);
        //heartMat.SetColor("Color 0",heartColor);
    }
     void OnEnable() 
     {
        AudioSource aS= this.gameObject.AddComponent<AudioSource>();
                aS.clip=aC;
                aS.Play();
        heartMat.DOVector(new Vector3(0,0,0),"_Dissolve_Dir",2f);
        heartMat.DOFade(.1f,"_BaseColor",2f);
    }
    public void UnLove() 
    {
        heartMat.DOFade(0f,"_BaseColor",2f);
        heartMat.DOVector(new Vector3(0,2.5f,0),"_Dissolve_Dir",2f).OnComplete(()=>{Destroy(this.gameObject);});
        
    }
    public void BigHeart() 
    {
        heartMat.DOFade(0f,"_BaseColor",2f);
        heartMat.DOVector(new Vector3(0,-2.5f,0),"_Dissolve_Dir",2f).OnComplete(()=>{Destroy(this.gameObject);});   
    }
    // Update is called once per frame
    void Update()
    {
        if(targetLove==null)targetLove= target.gameObject.GetComponent<Love>();
        if(targetLove.heart!=this ||targetLove.heart == null)Destroy(this.gameObject);
        this.transform.position = target.transform.position;
        heart.transform.Rotate(Vector3.up*rotateSpeed*Time.deltaTime,Space.World);
    }
}
