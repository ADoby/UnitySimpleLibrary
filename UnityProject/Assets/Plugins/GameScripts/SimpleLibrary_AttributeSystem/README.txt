This is SimpleLibrary AttributeSystem

It adds an Attribute class, including a nice PropertyDrawer.
Just add an attribute to your class like this:

//
//Code before

public Attribute MyAttribute;

//Code after
//

Or, to have more control in editor:

//
//Code before
//Name will be visible in the editor
//DebugMode enables buttons to change attribute progress in editor

public Attribute MyAttribute = new Attribute() { Name = "Health", DebugMode = true };

//Code after
//

Also adds an AttributeManager MonoBehaviour, including a nice custom Editor.
Usage:
1. Create empty GameObject
2. Add Component => AttributeManager
3. Set up categories and types
4. Click on Attributes
5. Set up your Attributes (Name ist visual only)
