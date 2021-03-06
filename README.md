<p align="center"><img width="20%" src="Portable-DoS/icon.ico" /></p>

**Portable DoS** is highly-optimized application written on C# (.NET Framework 4.5) that uses parallel options to implement DoS and DDoS attacks. This is **NOT a hacking tool**, but an application for load testing of sites. That is because it is released under the [GNU GPL v3 license](LICENSE). **Portable-DoS** uses [HTTP flood](https://en.wikipedia.org/wiki/HTTP_Flood) to attack a server. Application sends big number of small HTTP packets, but such that the server responds with a packet that is hundreds of times larger in size. Even if the server's channel is ten times wider than the attacker's channel, there is still a great chance to saturate the victim's bandwidth.  

## Introduction
### DoS attack
**DoS (denial-of-service) attack** is a cyber-attack in which the perpetrator seeks to make a machine or network resource unavailable to its intended users by temporarily or indefinitely disrupting services of a host connected to the Internet. Denial of service is typically accomplished by flooding the targeted machine or resource with superfluous requests in an attempt to overload systems and prevent some or all legitimate requests from being fulfilled.

### DDoS attack
In a **DDoS (distributed denial-of-service) attack**, the incoming traffic flooding the victim originates from many different sources. This effectively makes it impossible to stop the attack simply by blocking a single source.  

## How to Use
Download and build **Portable-DoS** from [Portable-DoS](sources).  
Define target website for testing and number of requests and threads in [Attack.json](Portable-DoS/Attack.json)  

```json
// Portable DoS attack settings
{
  "config": true,
  "agent": "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)",
  "target": "https://target-address.com/",
  "requests": 100,
  "threads": 1000
}
```
Start DoS attack testing  
```
Configuration file: attack.json
User agent: Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)
Target url: https://target-address.com/
Serial requests: 100
Parallel threads: 1000

User: DESKTOP-AZF7BU9
IPv4: 148.77.34.195
Country: United States of America

Checking target https://target-address.com/...
StatusCode: 200, ReasonPhrase: 'OK', Version: 1.1, Content: System.Net.Http.HttpConnectionResponseContent, 
Headers:
{
  Server: nginx-reuseport/1.13.4
  Date: Sat, 18 Jan 2021 23:59:00 GMT
  Connection: keep-alive
  Keep-Alive: timeout=30
  Vary: Accept-Encoding
  Vary: Accept-Encoding
  Cache-Control: public, max-age=0
  X-Powered-By: WP Rocket/3.7.4
  Accept-Ranges: bytes
  Content-Type: text/html; charset=UTF-8
  Content-Length: 761612
  Expires: Sat, 18 Jan 2021 23:59:00 GMT
}
```
The application shows the current attack status and target status  
```
[ Serial requests: 100 ][ Parallel threads: 1000 ]
[ Attack: 17% ][ Target:  503 (Service Temporarily Unavailable). ] 
```
In this case target website responds **503 Service Temporarily Unavailable** which means that the server cannot handle the request, because it is overloaded or down for maintenance. But generally, this is a temporary state.  
All list of HTTP status codes is available on [wiki](https://en.wikipedia.org/wiki/List_of_HTTP_status_codes).  

## 403 (Forbidden)
If you receive a **403 (Forbidden)** error response 
```
Target: Response status code does not indicate success: 403 (Forbidden).
```
change the agent value in [Attack.json](Portable-DoS/Attack.json). Agent configurations provided in [User-Agent.ini](Portable-DoS/User-Agent.ini).  

## License
GNU GPL v3
