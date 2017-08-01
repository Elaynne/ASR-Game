using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;

//linux 
/*endereço de ip 192.168.25.233
endereço de broadcast  192.168.25.255
rota padrão/dns primário 192.168.25.1
*/

public class ClientSkt
{
    public static IEnumerator SendVoice(byte[] audByte)
    {
        string recognized = null;
        // Create a TcpClient.
        // connected to the same address as specified by the server,
        //port combination
        //"179.187.35.29"; // ipexterno "177.135.0.208" ipinterno windows "192.168.25.226" linux "192.168.25.233"

        string strEnderecoIp = "yourTCP-IP.ddns.net";
        // Create a TCP/IP  socket.
        TcpClient clientSock = new TcpClient();
        Debug.Log("Connectando");

        IAsyncResult result = clientSock.BeginConnect(strEnderecoIp, 8080, null, null);

        while (!result.IsCompleted)
        {
            yield return new WaitForEndOfFrame();
        }

        if (!clientSock.Connected)
        {
            clientSock.Close();
        }
        else
        {
            Debug.Log("Reconhecendo");
            NetworkStream sktStream = clientSock.GetStream();
            byte[] buffer = BitConverter.GetBytes(audByte.Length);

            sktStream.BeginWrite(buffer, 0, buffer.Length, null, null);
            sktStream.BeginWrite(audByte, 0, audByte.Length, null, null);
            byte[] responseSize = new byte[4];

            bool recebido = false;
            sktStream.BeginRead(responseSize, 0, 4, a => { recebido = true; }, null);
            while (!recebido)
            {
                yield return new WaitForEndOfFrame();
            }

            int size = BitConverter.ToInt32(responseSize, 0);

            byte[] response = new byte[size];
            recebido = false;
            sktStream.BeginRead(response, 0, size, a => { recebido = true; }, null);
            while (!recebido)
            {
                yield return new WaitForEndOfFrame();
            }

            recognized = Encoding.ASCII.GetString(response, 0, response.Length);
            recognized = recognized.Replace("\0", string.Empty);
            Debug.Log("Reconheceu: " + recognized);
            GameControler.instance.OnRecognize(recognized);
            //GameControlador.instance.debug2.text = ("Reconheceu: " + recognized).ToString();
            sktStream.Close();
            clientSock.Close();
        }


    }


    //return recognized;
}	
	
