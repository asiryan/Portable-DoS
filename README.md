<p align="center"><img width="20%" src="sources/icon.ico" /></p>

**Portable DoS** is highly-optimized application written on C# (.NET Core) that uses parallel options to implement DoS and DDoS attacks. This is **NOT a hacking tool**, but an application for load testing of sites. That is because it is released under the [GNU GPL v3 license](LICENSE). **Portable-DoS** uses [HTTP flood](https://en.wikipedia.org/wiki/HTTP_Flood) to attack a server. Application sends big number of small HTTP packets, but such that the server responds with a packet that is hundreds of times larger in size. Even if the server's channel is ten times wider than the attacker's channel, there is still a great chance to saturate the victim's bandwidth.  

## Introduction
### DoS attack
**DoS (denial-of-service) attack** is a cyber-attack in which the perpetrator seeks to make a machine or network resource unavailable to its intended users by temporarily or indefinitely disrupting services of a host connected to the Internet. Denial of service is typically accomplished by flooding the targeted machine or resource with superfluous requests in an attempt to overload systems and prevent some or all legitimate requests from being fulfilled.

### DDoS attack
In a **DDoS (distributed denial-of-service) attack**, the incoming traffic flooding the victim originates from many different sources. This effectively makes it impossible to stop the attack simply by blocking a single source.  

## How to Use
Download and build **Portable-DoS** from [sources](sources).  
Define target website for testing and number of requests and threads in [Attack.json](sources/Attack.json).  

```json
// Portable DoS attack settings
{
  "config": true,
  "target": "https://target-adress.com/",
  "requests": 100,
  "threads": 1000
}
```
Start DoS attack testing using *Portable-DoS.exe*.
```
[ Serial requests: 100 ][ Parallel threads: 1000 ]
[ Attack: 17% ][ Target:  503 (Service Temporarily Unavailable). ] 
```
In this case target website respones 503 error which means that the server cannot handle the request, because it is overloaded or down for maintenance. But generally, this is a temporary state.  
All list of HTTP status codes is available on [wiki](https://en.wikipedia.org/wiki/List_of_HTTP_status_codes).  

## License
GNU GPL v3
