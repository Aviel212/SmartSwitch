#include <ESP8266WiFi.h>
#include <WiFiClient.h>
#include <ESP8266mDNS.h>
#include <ESP8266WebServer.h>
#include <FS.h>   // Include the SPIFFS library

ESP8266WebServer server(80);    // Create a webserver object that listens for HTTP request on port 80

bool shouldBlink = false; // indicates whether the status led should blink
unsigned long blinked = 0; // the time from the last blink of the status led [ms]
const int statusLed = D1;

const int touchSensor = D7;
bool touched = false; // for loop logic
unsigned long touchedAt; // the time (from the moment the program started running) the sensor was touched [ms]
const int pressDuration = 5000; // time from the moment a user touches the touch sensor until its action is activated [ms]

const int muxControl = D0; // multiplexer control line
const bool voltageFromMux = LOW;
const bool currentFromMux = HIGH;

const int load = D2; // the electrical device switch

void setup() {
  Serial.begin(115200);         // Start the Serial communication to send messages to the computer

  pinMode(statusLed, OUTPUT);
  
  pinMode(D5, OUTPUT); // GND for the touch sensor
  pinMode(D6, OUTPUT); // VCC for the touch sensor
  pinMode(touchSensor, INPUT);

  pinMode(muxControl, OUTPUT);

  pinMode(load, OUTPUT);

  digitalWrite(D5, LOW); // GND for the touch sensor
  digitalWrite(D6, HIGH); // VCC for the touch sensor
  
  Serial.println('\n');

  WiFi.mode(WIFI_OFF);
  
  SPIFFS.begin();                           // Start the SPI Flash Files System
  
  server.onNotFound([]() {                              // If the client requests any URI
    if (!handleFileRead(server.uri()))                  // send it if it exists
      server.send(404, "text/plain", "404: Not Found"); // otherwise, respond with a 404 (Not Found) error
  });

  server.on("/", HTTP_POST, handlePost);

  server.begin();                           // Actually start the server
  Serial.println("HTTP server started");
}

void loop() {
  server.handleClient();
  if (shouldBlink && (millis() - blinked) >= 1000) {
    blinked = millis();
    digitalWrite(statusLed, !digitalRead(statusLed));
  }
  
  if (digitalRead(touchSensor)) {
    Serial.println(millis());
    if (touched && (millis() - touchedAt) >= pressDuration) {
      startConnecting();
      touched = false;
    }
    else if(!touched) {
      touched = true;
      touchedAt = millis();
      digitalWrite(statusLed, HIGH);
    }
  } else {
    touched = false;
    if (!shouldBlink) digitalWrite(statusLed, LOW);
  }
  
}

void turnLoad(String state) {
  if (state == "on") digitalWrite(load, HIGH);
  else if (state == "off") digitalWrite(load, LOW);
}

double getVoltage() {
  digitalWrite(muxControl, voltageFromMux);
  // TODO
}

void startConnecting() {
  WiFi.mode(WIFI_AP);
  WiFi.softAP("SmartSwitch Yarden" + WiFi.macAddress());
  shouldBlink = true;
}

String getContentType(String filename) { // convert the file extension to the MIME type
  if (filename.endsWith(".html")) return "text/html";
  else if (filename.endsWith(".css")) return "text/css";
  else if (filename.endsWith(".js")) return "application/javascript";
  else if (filename.endsWith(".ico")) return "image/x-icon";
  return "text/plain";
}

bool handleFileRead(String path) { // send the right file to the client (if it exists)
  Serial.println("handleFileRead: " + path);
  if (path.endsWith("/")) path += "index.html";         // If a folder is requested, send the index file
  String contentType = getContentType(path);            // Get the MIME type
  if (SPIFFS.exists(path)) {                            // If the file exists
    File file = SPIFFS.open(path, "r");                 // Open it
    size_t sent = server.streamFile(file, contentType); // And send it to the client
    file.close();                                       // Then close the file again
    return true;
  }
  Serial.println("\tFile Not Found");
  return false;                                         // If the file doesn't exist, return false
}

void handlePost() {
  server.sendHeader("Access-Control-Allow-Origin", "*");

  // query server for available wifi
  if (server.hasArg("give-wifi-networks")) {
    int networksAmount = WiFi.scanNetworks();
    String str = "[\"";
    for (int i = 0; i < networksAmount; ++i) {
      str += "{\\\"name\\\":\\\"" + WiFi.SSID(i) + "\\\"";
      str += ",";
      str += "\\\"strength\\\":";
      str += WiFi.RSSI(i);
      str += ",";
      str += "\\\"hasEncryption\\\":";
      str += (WiFi.encryptionType(i) != ENC_TYPE_NONE);
      str += "}\"";
      if (i != networksAmount - 1) str += ",\"";
    }
    str += "]";
    server.send(200, "application/json", str);
    Serial.print("Sending to ");
    Serial.println(server.client().remoteIP());
  }

  // tell the device to connect to a given wifi
  if (server.hasArg("connect-to-network")) {
    WiFi.disconnect(); 
    Serial.println("Disconnected");
    WiFi.mode(WIFI_AP_STA);
    Serial.print("received ssid: ");
    Serial.println(server.arg("ssid"));

    digitalWrite(statusLed, HIGH);
    
    if (server.hasArg("pass")) {
      Serial.print("received pass: ");
      Serial.println(server.arg("pass"));
      WiFi.begin(server.arg("ssid"), server.arg("pass"));
    } else {
      WiFi.begin(server.arg("ssid"));
    }
    Serial.println("Connecting");

    for (int waiting = 0; WiFi.status() != WL_CONNECTED; ++waiting) {
      delay(500);
      Serial.print(".");
      if (waiting > 20) return;
    }

    shouldBlink = false;
    digitalWrite(statusLed, LOW);
    
    Serial.println("");
    Serial.println("Connected");
    Serial.print("IP address: ");
    Serial.println(WiFi.localIP());
  }

  // checking if the server is connected to wifi
  if (server.hasArg("are-you-connected")) server.send(200, "text/plain", (WiFi.status() == WL_CONNECTED) ? "yes" : "no");

  if (server.hasArg("username-given")) {
    // userneme is server.arg("username")
    // TODO
    return;
  }
}
