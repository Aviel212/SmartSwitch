#include <ESP8266WiFi.h>
#include <WiFiClient.h>
#include <ESP8266WiFiMulti.h>
#include <ESP8266mDNS.h>
#include <ESP8266WebServer.h>
#include <FS.h>   // Include the SPIFFS library

ESP8266WebServer server(80);    // Create a webserver object that listens for HTTP request on port 80

bool shouldBlink = true;
const int statusLed = D1;

void setup() {
  Serial.begin(115200);         // Start the Serial communication to send messages to the computer
  delay(10);
  pinMode(statusLed, OUTPUT);
  Serial.println('\n');

  WiFi.softAP("SmartSwitch " + WiFi.macAddress());
  /*WiFi.mode(WIFI_STA);
  WiFi.begin("Jordan's Place", "a098995242");
  Serial.println("");

  // Wait for connection
  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print(".");
  }

  Serial.println("");
  Serial.print("Connected to ");
  Serial.println("Jordan's Place");
  Serial.print("IP address: ");
  Serial.println(WiFi.localIP());

  if (MDNS.begin("esp8266")) {
    Serial.println("MDNS responder started");
  }*/
  
  SPIFFS.begin();                           // Start the SPI Flash Files System
  
  server.onNotFound([]() {                              // If the client requests any URI
    if (!handleFileRead(server.uri()))                  // send it if it exists
      server.send(404, "text/plain", "404: Not Found"); // otherwise, respond with a 404 (Not Found) error
  });

  server.on("/", HTTP_POST, handlePost);

  server.begin();                           // Actually start the server
  Serial.println("HTTP server started");
}

void loop(void) {
  server.handleClient();
  if (shouldBlink) {
    digitalWrite(statusLed, !digitalRead(statusLed));
    delay(750);
  }
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

  if (server.hasArg("connect-to-network")) {
    WiFi.disconnect(); 
    Serial.println("Disconnected");
    delay(1000);
    WiFi.mode(WIFI_AP_STA);
    Serial.print("received ssid: ");
    Serial.println(server.arg("ssid"));


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
    
    Serial.println("");
    Serial.println("Connected");
    shouldBlink = false;
    digitalWrite(statusLed, LOW);
    Serial.print("IP address: ");
    Serial.println(WiFi.localIP());
  }

  if (server.hasArg("are-you-connected")) server.send(200, "text/plain", (WiFi.status() == WL_CONNECTED) ? "yes" : "no");
}
