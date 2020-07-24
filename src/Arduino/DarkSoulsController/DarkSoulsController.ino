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
  serialCommand.addCommand("backwards", Backwards );
  serialCommand.addCommand("quit", QuickQuit);

  serialCommand.addCommand("rotateItem", RotateItem);
  serialCommand.addCommand("rotateSpell", RotateSpell);
  serialCommand.addCommand("block", Block);
  serialCommand.addCommand("run", Run);
  serialCommand.addCommand("jump", Jump);
  serialCommand.addCommand("runJump", RunJump);
  serialCommand.addCommand("snipe", Snipe);
  serialCommand.addCommand("gitgud", GitGud);
  serialCommand.addCommand("interact", Interact);
  serialCommand.addCommand("kick", Kick);
  serialCommand.addCommand("lock", Lock);

  
  serialCommand.addCommand("debug", SetDebug );
}

//
// Read and respond to commands recieved over the serial port
//
void loop() {
  serialCommand.readSerial();
}

void Snipe() {
  
}
void Kick() {
   Keyboard.begin();
  
  
  Keyboard.press('w');
  Keyboard.press('p');
  delay(50);
  Keyboard.releaseAll();
  delay(50);

  Keyboard.end();
}
void GitGud() {
  LeftHeavyAttack();
  delay(3000);
  RightLightAttack();
}

void RotateItem() {
  SendBasicKeyCommand(KEY_DOWN_ARROW);
}

void RotateSpell() {
  SendBasicKeyCommand(KEY_UP_ARROW);
}
void Lock() {
  SendBasicKeyCommand('q');
}


void Interact() {
  SendBasicKeyCommand('e');
}

void Jump() {
  SendBasicKeyCommand(' ');
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
void QuickQuit() {
  Keyboard.begin();
  
  Keyboard.press(KEY_ESC);
  delay(50);
  Keyboard.releaseAll();
  delay(50);
  
  Keyboard.press(KEY_LEFT_ARROW);
  delay(50);
  Keyboard.releaseAll();
  delay(50);
  
  Keyboard.press('e');
  delay(50);
  Keyboard.releaseAll();
  delay(50);

  
  Keyboard.press(KEY_LEFT_SHIFT);
  Keyboard.press(KEY_LEFT_ARROW);
  delay(50);
  Keyboard.releaseAll();
  delay(50);
  
  Keyboard.press('e');
  delay(50);
  Keyboard.releaseAll();
  delay(50);

  Keyboard.press(KEY_LEFT_ARROW);
  delay(50);
  Keyboard.releaseAll();
  delay(50);

  Keyboard.press('e');
  delay(100);
  Keyboard.releaseAll();
  delay(50);
  

  Keyboard.press('e');
  delay(100);
  Keyboard.releaseAll();
  delay(50);
  
  Keyboard.end();
}
void Armor() {
  SendBasicKeyCommand('4');
}
void Block() {

  char *arg = serialCommand.next();

  Keyboard.begin();
  Keyboard.press('l');
  delay(2500);  
  Keyboard.releaseAll();
  Keyboard.end();
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
void Run() {

  char *arg = serialCommand.next();

  Keyboard.begin();
  Keyboard.press(' ');
    delay(2500);    
  Keyboard.releaseAll();
  Keyboard.end();
}
void RunJump() {

  char *arg = serialCommand.next();

  Keyboard.begin();
  Keyboard.press(' ');
  delay(1000);  
  Keyboard.releaseAll();
  delay(10);  
  Keyboard.press(' ');
  delay(10);  
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
  Keyboard.releaseAll();
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
