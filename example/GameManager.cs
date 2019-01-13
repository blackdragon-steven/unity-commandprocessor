using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommandProcessing;

public class GameManager : MonoBehaviour {

    public void ChatHandler (List<string> _chatMessage ) {
        string _message = "<CHAT MESSAGE FROM PLAYER> ";
        foreach(string s in _chatMessage ) {
            _message += s;
            _message += ' ';
        }
        Debug.Log ( _message );
    }

    private CommandProcessor _commandProcessor;

    private void Start ( ) {
        _commandProcessor = new CommandProcessor ( );
        CommandProcessor.StartCommandListening ( commands_t.CHAT , ChatHandler );
        CommandProcessor.ParseCommandString ( "/chat Hello There" );
        CommandProcessor.ParseCommandString ( "Hello There" );
    }
}
