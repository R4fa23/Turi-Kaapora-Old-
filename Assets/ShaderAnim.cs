using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderAnim : MonoBehaviour
{
    public Texture2D texture;
    public MeshRenderer meshRenderer;
    Material mat;
    void Start()
    {
        meshRenderer.material = new Material(meshRenderer.material);
        mat = meshRenderer.material;
    }

    // Update is called once per frame
    void Update()
    {
        mat.SetTexture("_Eye", texture);
    }

}
