WHAT DOES THIS APPLICATION DO ?

This project is a maze generator made with Unity.
It generates mazes using different algorithms :
	- Prim
	- DFS
	- Wilson
For each of them, you can visualize the process step by step.

You can change various properties through the application menu : number of lines/columns of the maze, shape of the cells (squares or hexagons), activate teleporters generation and change the algorithm used for generation.
There is also a character you can play with to move into the maze by using the mouse.



ON WHICH PLATFORMS ?

There are builds for Android and PC in the Builds folder.



CODE STRUCTURE AND CHOICES OF IMPLEMENTATION

I distinguish the model, view and the controller of the maze generation in 3 distinct folders.

My maze model is a grid of cells (RegularCell or QuadCell, but QuadCell is the old version). The cells can be of different shapes and we can interact with them to connect to other cells (basically the surrounding cells) so they are aware of their neighbours. Each neighbouring cells have a Wall in common. This is a pair of "corner points" (the points around the core : 4 corner points for a square, 6 for an hexagon).

No matter the class we use in the generation algorithm (QuadCell or RegularCell), they both implement the same interface (GraphVertex) used by those algorithms. So we can reuse them with other cell implementations...

Once the maze is generated, it is basically a set of GraphVertex (IEnumerable<GraphVertex>) that we display on scene. The MazeViewer iterates through all the cells to display the walls around that cell and, if the current cell has a teleporter, we instantiates that teleporter.

To controll that maze generation, there are 2 classes. Manager is the class used to generate a maze and clear the scene. UIManager is the interface between the user and the Manager. Indeed, interacting with the UI elements changes the properties of the Manager so it changes the next maze which will be generated.


I have chosen a generic and reusable data structure (RegularCell) to make possible the generation of new shapes. However, it was harder than I expected to work with hexagons and even if it generates a beautiful grid, this is not a perfect maze (there are loops and closed areas). I also wanted to add octogons but I clearly don't have time for that.


I hope you will enjoy !
