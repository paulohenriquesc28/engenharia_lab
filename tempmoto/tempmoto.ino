#include <TinyGPS++.h>
#include <Adafruit_SSD1306.h>
#include "DHT.h"
#include <HTTPClient.h>
#include "SD.h"


#define SCREEN_WIDTH 128 // OLED display width, in pixels
#define SCREEN_HEIGHT 32 // OLED display height, in pixels
const char* serverName = "https://tempmotoweb.azurewebsites.net/api/medicoes";

//On ESP32: GPIO-21(SDA), GPIO-22(SCL)
#define OLED_RESET -1 //Reset pin # (or -1 if sharing Arduino reset pin)
#define SCREEN_ADDRESS 0x3C //See datasheet for Address
Adafruit_SSD1306 display(SCREEN_WIDTH, SCREEN_HEIGHT, &Wire, OLED_RESET);

#define RXD2 16
#define TXD2 17
HardwareSerial neogps(1);

TinyGPSPlus gps;

double Latitude;
double Longitude;
double Velocidade;
double Altitude;
int Num_Satelites;
int tempo = 15000;

unsigned long ultima = 0;
const int pino_dht = 4; // pino onde o sensor DHT está conectado
float temperatura; // variável para armazenar o valor de temperatura
float umidade; // variável para armazenar o valor de umidade
DHT dht(pino_dht, DHT11); // define o pino e o tipo de DHT*/

void limpaDisplay();
boolean leGps();
boolean leDht();
void salva();
void escreve();
void posta();

void setup() {
  // inicializa o serial
  Serial.begin(115200);

  // inicializa o gps
  neogps.begin(9600, SERIAL_8N1, RXD2, TXD2);

  // inicializa display
  if(!display.begin(SSD1306_SWITCHCAPVCC, SCREEN_ADDRESS)) {
    Serial.println(F("SSD1306 allocation failed"));
    for(;;); // Don't proceed, loop forever
  }
  limpaDisplay();

  // inicializa DHT
  dht.begin();
  display.println("DHT inicializado");
  display.display();
  // inicializa o leitor SD
  display.println("Inicializado SD");
  display.display();
  if(!SD.begin()){
    Serial.println("Falha ao ler SD");
    display.println("Falha ao ler SD");
  }
  uint8_t cardType = SD.cardType();
  if(cardType == CARD_NONE){
    Serial.println("SD nao encontrado");
    display.println("SD nao encontrado");
  }
  display.display();
  delay(3500);

  limpaDisplay();

  // conecta ao wifi
  display.println("Conectando wifi");
  display.display();

  WiFi.mode(WIFI_STA);
  WiFi.disconnect();
  
  WiFi.begin("Teste", "spha4466");
  //limpaDisplay();
  display.println("Conectando ao WiFi");
  display.display();
  delay(1500);
  //limpaDisplay();
  while(WiFi.status() < WL_CONNECTED){
    display.print(".");
    display.display();
    Serial.println("status: "+ String(WiFi.status()));
    delay(1000);
  }
  Serial.println("status: "+ String(WiFi.status()));
  if(WiFi.status() == WL_CONNECTED){
    Serial.println("Conectado: "+ String(WiFi.localIP()));
    display.println("Conectado");// + String(WiFi.localIP())
  }else{
    display.println("Erro ao conectar");
  }
  display.display();
  delay(1500);

}

void loop() {
    limpaDisplay();
    //for (unsigned long start = millis(); millis() - start < 5000;){
    boolean leuGps = leGps();
    boolean leuDht = leDht();
      if(leuGps && leuDht) salva();
    //}
    delay(1000);
}

void limpaDisplay(){
  display.clearDisplay();
  display.setTextColor(SSD1306_WHITE);
  display.setCursor(0, 0);
  display.setTextSize(1);
}

boolean leGps()
{
  boolean newData = false;
  for (unsigned long start = millis(); millis() - start < 1000;)
  {
    while (neogps.available())
    {

      char c = neogps.read();
      //Serial.print(c);
        if (gps.encode(c))
        {
          newData = true;
        }
    }
  }
  if(!newData){
    display.println("Sem conexao com GPS"); 
    display.display();
    return false;
  }
  if (gps.location.isValid())
  {
    if(gps.location.isUpdated()){
      Latitude = gps.location.lat();
      Longitude = gps.location.lng();
      Velocidade = gps.speed.kmph();
      Altitude = gps.altitude.meters();
      Num_Satelites = gps.satellites.value();
    
      display.setCursor(0, 0);
      display.print("Lat: ");
      //display.setCursor(, 0);
      display.println(Latitude,6);

      //display.setCursor(25, 20);
      display.print("Lng: ");
      //display.setCursor(50, 20);
      display.println(Longitude,6); 
      display.display(); 
      
      return true;
    }else{
      display.println("Dados do GPS desatualizados"); 
      display.display();
      return false;
    }
  }
  else
  {
    display.println("Sem conexao GPS");
    display.display();
    return false;
  } 
}

boolean leDht(){
  temperatura = dht.readTemperature(); // lê a temperatura em Celsius
  umidade = dht.readHumidity(); // lê a umidade
  
  // Se ocorreu alguma falha durante a leitura
  if (isnan(umidade) || isnan(temperatura)) {
    //Serial.println("Falha na leitura do Sensor DHT!");
    display.println("Falha na leitura do Sensor DHT ");
    display.display();
    return false;
  }
  else { // Se não
    // Imprime o valor de temperatura  
    display.print("Temp: ");
    display.print(temperatura);
    display.println(" *C ");
    display.display();
    
    // Imprime o valor de umidade
    display.print("Umi: ");
    display.print(umidade);
    display.print(" %"); 
    display.display();

    return true;
  
  }
}

void salva(){
  
  if(millis() - ultima < tempo) return;
  
  ultima = millis();
  
  limpaDisplay();
  if(WiFi.status() != WL_CONNECTED){ 
    display.println("Sem conexao");
    display.display();
    delay(1500);
    escreve();
  }else{
    posta();
  }
}

void posta(){
  display.println("postando medicoes");
  display.display();
  
  WiFiClientSecure *client = new WiFiClientSecure;
  client->setInsecure(); //don't use SSL certificate
  HTTPClient https;
  https.begin(*client, serverName);
  
  String httpRequestData = "{\"Latitude\": "+String(Latitude,6)+",\"Longitude\": "+String(Longitude,6)+",\"Temperatura\": "+String(temperatura,2)+",\"Umidade\": "+String(umidade,2)+",\"Num_Satelites\": "+String(Num_Satelites)+",\"Velocidade\": "+String(Velocidade)+",\"Altitude\": "+String(Altitude,2)+"}";
  
  Serial.print("httpRequestData: ");
  Serial.println(httpRequestData);
  
  https.addHeader("Content-Type", "application/json");
  int httpResponseCode = https.POST(httpRequestData);
  
  display.print("Cod Resp:");
  display.print(httpResponseCode);
  display.display();
  
  if (httpResponseCode>=200 && httpResponseCode<300) {
    Serial.print("HTTP Response code: ");
    Serial.println(httpResponseCode);
  }
    else {
    Serial.print("Error code: ");
    Serial.println(httpResponseCode);
    
    display.print("Erro postagem:");
    display.println(httpResponseCode);
    display.display();
    delay(2000);
    escreve();
  }
  https.end();
}

void escreve(){
  display.println("Salvando no SD");
  display.display();
  delay(1000);
  limpaDisplay();
  String date = "";
  String time = "";
  date +=String(gps.date.day())+"-";
  date +=String(gps.date.month())+"-";
  date +=String(gps.date.year());
  time +=String(gps.time.hour()-3)+":";
  time +=String(gps.time.minute())+":";
  time +=String(gps.time.second());
  String csv = "";
  csv += String(Latitude,6)+","; 
  csv += String(Longitude,6)+",";
  csv += String(temperatura)+",";
  csv += String(umidade)+",";
  csv += String(Num_Satelites)+",";
  csv += String(Velocidade)+",";
  csv += String(Altitude)+",";
  csv += String(date)+" "+String(time)+"\n";
  String arq = "/"+String(date)+".csv";
  if(!SD.exists(arq)){
    Serial.println("criando arquivo: "+arq);
    display.println("criando arquivo: "+arq);
    display.display();

    File file = SD.open(arq, FILE_WRITE);
    if(!file){
      Serial.println("Erro ao criar arquivo!");
      display.println("Erro ao criar arquivo!");
      display.display();
      delay(1000);
      return;
    }
    if(file.print(csv)){
      Serial.println("Salvo!");
      display.println("Salvo!");
      display.display();
    } else {
      Serial.println("Erro ao escrever!");
      display.println("Erro ao escrever!");
      display.display();
      delay(1000);
      return;
    }
    file.close();
  }
  else{
    File file = SD.open(arq, FILE_APPEND);
    if(!file){
      Serial.println("Erro ao escrever!");
      display.println("Erro ao escrever!");
      display.display();
      delay(1000);
      return;
    }
    if(file.print(csv)){
      Serial.println("Salvo com sucesso!");
      display.println("Salvo com sucesso!");
      display.display();
    } else {
      Serial.println("Erro ao escrever!");
      display.println("Erro ao escrever!");
      display.display();
      delay(1000);
      return;
    }
    file.close();
  }
  delay(1500);
  return;
}
