using System;
using System.Collections.Generic;

namespace CommandProcessing {
    public class CommandProcessor {
        #region CLASS INITIALISER
        public CommandProcessor (bool _requireChatCmd = false, bool _requireCmdPrefix = true, char _cmdPrefix ='/', char _paramDelimiter=' ' ) {

            REQUIRE_CHAT_COMMAND = _requireChatCmd;
            REQUIRE_COMMAND_PREFIX = _requireCmdPrefix;
            COMMAND_PREFIX = _cmdPrefix;
            PARAMETER_DELIMITER = _paramDelimiter;

            if ( !REQUIRE_COMMAND_PREFIX ) {
                REQUIRE_CHAT_COMMAND = true;
            }

            _commandDictionary = new Dictionary<commands_t , Action<List<string>>> ( );

            if (_INSTANCE == null ) {
                _INSTANCE = this;
            } else {
                //ERROR MESSAGE
                //TODO: Create Singleton Error Message
            }
        }
        #endregion
        #region PUBLIC STATIC VARS
        public static CommandProcessor INSTANCE {
            get {
                return _INSTANCE;
            }
        }
        public static bool REQUIRE_CHAT_COMMAND;
        public static bool REQUIRE_COMMAND_PREFIX;
        public static char COMMAND_PREFIX;
        public static char PARAMETER_DELIMITER;
        #endregion
        #region PUBLIC STATIC METHODS
        public static void StartCommandListening(commands_t _command, Action<List<string>> _listener ) {
            Action<List<string>> _thisAction;

            if (INSTANCE._commandDictionary.TryGetValue(_command, out _thisAction ) ) {
                _thisAction += _listener;
            } else {
                _thisAction = new Action<List<string>> ( _listener );
                INSTANCE._commandDictionary.Add ( _command , _thisAction );
            }
        }

        public static void StopCommandListening ( commands_t _command , Action<List<string>> _listener ) {
            if(INSTANCE == null ) { return; }

            Action<List<string>> _thisAction;
            if(INSTANCE._commandDictionary.TryGetValue(_command, out _thisAction ) ) {
                _thisAction -= _listener;
            }
        }

        public static void ParseCommandString ( string _commandString ) {
            if ( REQUIRE_COMMAND_PREFIX ) {
                if(_commandString[0] == COMMAND_PREFIX ) {
                    //IF IT IS OUR COMMAND PREFIX - MOVE ON TO HANDLE COMMAND
                    string _command = _commandString.Remove ( 0 , 1 );
                    INSTANCE.ParseCommand ( _command );
                } else {
                    if ( REQUIRE_CHAT_COMMAND ) {
                        //DISCARD MESSAGE
                        //TODO: Create Bad Command Error Message
                    } else {
                        //SEND CHAT MESSAGE TO CONSOLE
                        INSTANCE.TriggerCommand ( commands_t.CHAT , new List<string> ( ) { _commandString } );
                    }
                }
            } else {
                //IF WE DON'T REQUIRE COMMAND PREFIX, LETS PARSE THE COMMAND
                INSTANCE.ParseCommand ( _commandString );
            }
        }
        #endregion
        //PUBLIC VARS
        #region  PUBLIC METHODS
        public void ParseCommand(string _commandString ) {
            string [ ] _words = _commandString.Split ( PARAMETER_DELIMITER );

            commands_t _receivedCommand;

            if(Enum.TryParse(_words[0], true, out _receivedCommand ) ) {
                TriggerCommand ( _receivedCommand , ParseParamters ( _commandString.Substring ( _words [ 0 ].Length + 1 ) ) );
            } else {
                //TODO: Invalid Command Error Message
            }
        }
        #endregion
        //PROTECTED STATIC VARS
        //PROTECTED STATIC METHODS
        //PROTECTED VARS
        //PROTECTED METHODS
        #region PRIVATE STATIC VARS
        private static CommandProcessor _INSTANCE;
        #endregion
        //PRIVATE STATIC METHODS
        #region PRIVATE VARS
        private Dictionary <commands_t, Action <List<string>>> _commandDictionary;
        #endregion
        #region PRIVATE METHODS
        private void TriggerCommand ( commands_t _command , List<string> _params ) {
            Action<List<string>> _thisAction;
            if ( INSTANCE._commandDictionary.TryGetValue ( _command , out _thisAction ) ) {
                _thisAction.Invoke ( _params );
            }
        }

        private List<string> ParseParamters(string _params ) {
            return new List<string> ( _params.Split ( PARAMETER_DELIMITER ) );
        }
        #endregion
    }
}