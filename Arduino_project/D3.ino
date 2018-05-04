#define ENCODER_A_PIN 2
#define ENCODER_B_PIN 3
#define SWITCH_PIN    4
#define SPEAKER_PIN 5
#define LED_PIN 6
long time1, time2;
String inputString = "";         // a String to hold incoming data
boolean stringComplete = false;  // whether the string is complete
boolean stage = false;
char _start = '1';
char _stop = '0';
int count = 0;
 
void setup(){
  pinMode(ENCODER_A_PIN, INPUT);
  pinMode(ENCODER_B_PIN, INPUT);
  pinMode(SWITCH_PIN, INPUT);
  pinMode(SPEAKER_PIN, OUTPUT);
  pinMode(LED_PIN, OUTPUT);
 
 attachInterrupt(0, read_quadrature, CHANGE);
  //setup our serial 初始化Arduino串口
  Serial.begin(9600);
  inputString.reserve(200);
}

void loop()
{
  //if (digitalRead(SWITCH_PIN) == LOW)
  //{
    //delay(10);
    //if (digitalRead(SWITCH_PIN) == LOW){
    //Serial.println("Switch Pressed");
    //}
  //}
  if (stringComplete) 
  {
    //Serial.println(inputString);
    // clear the string:
    inputString = "";
    stringComplete = false;
  }
  if (stage)
  {
    if (time1 != time2)
    {
      time2 = time1;
      count = 0;
      digitalWrite(LED_PIN, LOW);
    }
    else
    {
      count++;
    }
  }
  else
  {
  digitalWrite(LED_PIN, LOW);
  }
  if (count>30)
  {
    Serial.println("E");
      digitalWrite(LED_PIN, HIGH);
      for (long i = 0; i <  10; i ++) 
      {
        digitalWrite(SPEAKER_PIN, HIGH);
        delayMicroseconds(1000);
        digitalWrite(SPEAKER_PIN, LOW);
        delayMicroseconds(1000);
      }
  }
  else
  {
    digitalWrite(LED_PIN, LOW);
  }
delay(1000);
} 


 
void read_quadrature(){ time1++;
  }

void serialEvent() {
  while (Serial.available()) {
    char inChar = (char)Serial.read();
    if( inChar == _start)
    {
      //Serial.println("true");
      stage = true;
    }
    else if(inChar == _stop)
    {
      //Serial.println("false");
      stage = false;
    }
    inputString += inChar;  //拼接的字符串无结束符有大问题
    if (inChar == '\n')
    {
      stringComplete = true;
    }
  }
}
