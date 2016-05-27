# Capture the flag 
## Breaking the walls between virtual gaming and real world with IoT devices and Microsoft Azure

* Combine Unity 3D Game development with IOT devices and utilizing Azure Service Fabric as a backend
* We install on the wall two Red & Blue LED matrix one on each side of the screen
* Audience downloads the game from mobile store or play on browser
* Divide audience into two groups - red and blue upon login into created session
* All members enters a 3d virtual world game
* Each group member need to convert opponent flag to his group's color
* All data of the members and their actions are collected into the cloud
* IOT hub transmit command to local gateway (raspberry pi) to convert light from red to blue accordingly
* We collect data from sensors inside the hall like noise level ect.
* Game ends when all flags turned into one color or time elapsed (displayed on screen or big 7 Segment controlled by RPi)
* during the game we collect and analyze the data with stream analytics and Power BI and display statistics like best scoring member, average group location on map, noise level from crowd based on how many flags turned, and so on…
* Controlling the game logic using Azure Service Fabric, and demonstrate how logic can be changed during the game is running by upgrading one of the services - how many players need to touch a flag simultaneously to turn it's color.
