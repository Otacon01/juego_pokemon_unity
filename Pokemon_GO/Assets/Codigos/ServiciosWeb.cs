using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ServiciosWeb : MonoBehaviour
{
    public DatosServicio servicioRegistroUsuario;
    public RespuestaRegistroUsuario registroUsuario = new RespuestaRegistroUsuario();
    void Start()
    {
        DatosRegistroUsuario datosRegistro = new DatosRegistroUsuario();
        datosRegistro.cedula = "4";
        datosRegistro.email = "danigmail.com";
        datosRegistro.nombre = "Daniel";
        datosRegistro.edad = 36;

        StartCoroutine(RegistrarUsuario(datosRegistro));
    }

    public IEnumerator RegistrarUsuario(DatosRegistroUsuario datosRegistro)
    {
        var registroJSON = JsonUtility.ToJson(datosRegistro);

        var solicitud = new UnityWebRequest();

        solicitud.url = servicioRegistroUsuario.url;

        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(registroJSON);
        solicitud.uploadHandler = new UploadHandlerRaw(bodyRaw);
        solicitud.downloadHandler = new DownloadHandlerBuffer();
        solicitud.method = UnityWebRequest.kHttpVerbPOST;
        solicitud.SetRequestHeader("Content-Type", "application/json");
        
        solicitud.timeout = 10;

        yield return solicitud.SendWebRequest();

        if (solicitud.result == UnityWebRequest.Result.ConnectionError)
        {
             servicioRegistroUsuario.respuesta = "Conexion fallida";
        }
        else
        {           
            registroUsuario = (RespuestaRegistroUsuario)JsonUtility.FromJson(solicitud.downloadHandler.text, typeof(RespuestaRegistroUsuario));
            servicioRegistroUsuario.respuesta = registroUsuario.mensaje;
        }
        solicitud.Dispose();
        servicioRegistroUsuario.evento.Invoke();
    }
}
