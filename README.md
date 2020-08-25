# Location Tracker

Location tracker is a **WPF** based tool used to parse data coming in from the device and show travel path and distance. There are two modes for travel, walk and drive. They use different strategies to display and calculate travel path.

## Getting Started

1. Clone project from https://github.com/MantasVa/LocationTracker.
2. Open project solution.
3. Start **LocationTracker.Client** project.
4. Insert one of sample packets into the textbox and press Decode.

**Sample Packets:**
```
TCP Packet:
00000000000002CE04
08
00000173CB41FBF000 018194B8 034239D9 08EA00000C0000
00000 173CB45A570 00 0181AD4B 03423CA9 08EA00000C0000
00000 173CB486490 00 0181A525 0342523A 08EA00000C0000
00000 173CB4A3950 00 0181A188 03425D09 08EA00000C0000
00000 173CB4C0E10 00 01819DA9 034274EC 08EA00000C0000
00000 173CB4DE2D0 00 018162F3 03427C7B 08EA00000C0000
00000 173CB50A1F0 00 018160A0 0342949B 08EA00000C0000
00000 173CB5276B0 00 018174EC 0342AA4E 08EA00000C0000
08
00005252

UDP Packet: 
Work In Progress
```


### Prerequisites

- .Net Core SDK 3.x
- .Net Core runtime 3.x
- Any environmental to run .Net Core SDK 3.x (e.g. Visual Studio 2019, Visual Code ect..)


## Built With
* [GMap.NET](https://github.com/judero01col/GMap.NET) - Maps
* **SOLID**

## Used Following Patterns
* **Visitor**
* **Strategy**
* **Factory**
* **Composite**
* **Template Method**

## Authors

* **Aidanas Naugzemis**  - [Profile](https://github.com/Aidanas93)
* **Mantas Valuckas** - [Profile](https://github.com/MantasVa)


