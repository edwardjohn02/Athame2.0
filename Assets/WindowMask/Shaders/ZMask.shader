Shader "Custom/ZMask" {
 
    Properties {
    }
 
    SubShader {
        LOD 100
   
        Tags { "Queue"="Transparent-1"}
       
        Pass {
            Blend Zero One
        }
    }
}
