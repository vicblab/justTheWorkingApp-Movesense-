# justTheWorkingApp-Movesense-
Unity project with working default scenes for Android on the old version of the Movesense Unity plugin. 

It also contains a small example scene (which is unfinished, and I am planning to expand on other repository to allow for raspberry bt connection) that when built connects to the first movesense device it founds (even though I didn't delete the known MACaddress list, but it is currently useless) and displays its linear acceleration data. The code for the example can be found on Assets/MovesensePlugin/ Scripts/ TUASexapmle.cs .

It is probable that it doesn't work fine on IOS since I had to compress a large document (Assets/Movesense Plugin/ Scripts/ Movesense/ IOS / libmds.a) to be able to upload the repository. If you want to use it you can unzip it on the same directory.
