int cntr=1;
int adc=0;
void setup() {
  // put your setup code here, to run once:
  Serial.begin(9600);
  while (!Serial) {
  }
}

void loop() {
  // put your main code here, to run repeatedly:
   adc=analogRead(A0);
     Serial.print(cntr);
     Serial.print(" = ");
     Serial.print(adc);
     Serial.println("");
     cntr++;
   delay(500);
}
