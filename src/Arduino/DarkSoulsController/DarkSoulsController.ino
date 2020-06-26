#include <SoftwareSerial.h>
#include <SerialCommand.h>      // Steven Cogswell ArduinoSerialCommand library from http://GitHub.com
#include <Keyboard.h>
#define ONE_WIRE_BUS 2
#define LED_LOW      3
#define LED_HIGH     6
#define DBGMSG(A)    if (dbg){ Serial.print("DBG: "); Serial.println(A);}

//
// Globals
//
SerialCommand     serialCommand;
boolean           dbg = true;

//
// Initialize the serial command table, I/O pins, and the temperature sensor
//
void setup() {
  Serial.begin(9600);
  serialCommand.addCommand("rightLight", RightLightAttack );
  serialCommand.addCommand("rightHeavy", RightHeavyAttack );
  serialCommand.addCommand("leftLight", LeftLightAttack );
  serialCommand.addCommand("leftHeavy", LeftHeavyAttack);
  serialCommand.addCommand("roll", Roll );
  serialCommand.addCommand("item", Item );
  serialCommand.addCommand("swapRight", SwapRight );
  serialCommand.addCommand("swapLeft", SwapLeft);
  serialCommand.addCommand("forwards", Forward );
  serialCommand.addCommand("left", Left );
  serialCommand.addCommand("right", Right );

  serialCommand.addCommand("debug", SetDebug );
}

//
// Read and respond to commands recieved over the serial port
//
void loop() {
  serialCommand.readSerial();
}

void RightLightAttack() {
  SendBasicKeyCommand('p');
}
void RightHeavyAttack() {
  SendBasicKeyCommand('o');
}
void LeftLightAttack() {
  SendBasicKeyCommand('l');
}
void LeftHeavyAttack() {
  SendBasicKeyCommand('k');
}
void Roll() {
  SendBasicKeyCommand(' ');
}
void Item() {
  SendBasicKeyCommand('r');
}
void SwapRight() {
  SendBasicKeyCommand('4');
}

void SwapLeft() {
  SendBasicKeyCommand('3');
}
void Prone() {
  Keyboard.begin();
  Keyboard.press(KEY_LEFT_CTRL);
  delay(500);
  Keyboard.releaseAll();
  Keyboard.end();
}
void Armor() {
  SendBasicKeyCommand('4');
}
void Left() {

  char *arg = serialCommand.next();

  Keyboard.begin();
  Keyboard.press('a');
  
  if (arg != NULL) {
    delay(2500);    
  } else {
    delay(2500);    
  }
  Keyboard.releaseAll();
  Keyboard.end();
}

void Right() {

  char *arg = serialCommand.next();

  Keyboard.begin();
  Keyboard.press('d');
   if (arg != NULL) {
    delay(2500);    
  } else {
    delay(2500);    
  }
  Keyboard.releaseAll();
  Keyboard.end();
}
void Forward() {

  char *arg = serialCommand.next();

  Keyboard.begin();
  Keyboard.press('w');
  
  if (arg != NULL) {
    delay(2500);    
  } else {
    delay(2500);    
  }
  Keyboard.releaseAll();
  Keyboard.end();
}

void Backwards() {

  char *arg = serialCommand.next();

  Keyboard.begin();
  Keyboard.press('s');
   if (arg != NULL) {
    delay(2500);    
  } else {
    delay(2500);    
  }
  Keyboard.releaseAll();
  Keyboard.end();
}
void SendBasicKeyCommand(char key) {
  Keyboard.begin();
  Keyboard.press(key);
  delay(10);   
  Keyboard.releaseAll();
  Keyboard.end();
}
//
// Enable or disable debug messages from being printed
// on the serial terminal
//
void SetDebug() {
  char *arg = serialCommand.next();

  if (arg != NULL) {
    if ( strcmp(arg, "on" ) == 0) {
      dbg = true;
      DBGMSG(F("Turn on debug"));
    }
    if ( strcmp(arg, "off" ) == 0) {
      DBGMSG(F("Turn off debug"));
      dbg = false;
    }
  }
}

//
// An unrecognized command was recieved
//
void UnrecognizedCommand() {
  DBGMSG(F("Unrecognized command"));
  DBGMSG(F(" ledon 3  - turn on led connected to digital I/O 3"));
  DBGMSG(F(" ledon 4  - turn on led connected to digital I/O 4"));
  DBGMSG(F(" ledon 5  - turn on led connected to digital I/O 5"));
  DBGMSG(F(" ledon 6  - turn on led connected to digital I/O 6"));
  DBGMSG(F(" ledoff 3 - turn off led connected to digital I/O 3"));
  DBGMSG(F(" ledoff 4 - turn off led connected to digital I/O 4"));
  DBGMSG(F(" ledoff 5 - turn off led connected to digital I/O 5"));
  DBGMSG(F(" ledoff 6 - turn off led connected to digital I/O 6"));
  DBGMSG(F(" temp     - read temperature" ));
  DBGMSG(F(" debug on - turn on debug messages" ));
  DBGMSG(F(" debug off- turn off debug messages" ));
}
