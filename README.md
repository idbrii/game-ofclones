About
=====

This project refactors the "[Unity Projects: 2D Platformer](https://www.assetstore.unity3d.com/en/#!/content/11228)"
from Unity Asset Store to reduce code duplication and engineer it to follow the
component entity pattern. This pattern is essentially the same concept that
Unity exposes: you have a GameObject (your entity) and can add scripts
(Components) to it. We can abstract out common behaviors into a common class:
Instead of PlayerHealth and Enemy containing its own health, we have a Vitality
component that handles both cases.

One exception is that Unity provides Components with an Update function instead
of having a corresponding System that updates its components. You could still
write a System using FindObjectsOfType(typeof(Vitality)), but the documentation
warns "this function is very slow". Alternatively, you could have Components
register/deregister themselves with the System on in Start/OnDestroy. Neither
of these methods seem to follow the core intent of Unity's design and have not
been implemented.


Read more about Component Entity System
=======================================

* [Description of Component-Entity-Systems](http://www.gamedev.net/page/resources/_/technical/game-programming/understanding-component-entity-systems-r3013).
* [A concrete example](http://www.chris-granger.com/2012/12/11/anatomy-of-a-knockout/) of developing a game using Component-Entity pattern (in ClojureScript).
* Long articles about the [what](http://www.richardlord.net/blog/what-is-an-entity-framework) and [why](http://www.richardlord.net/blog/why-use-an-entity-framework) of Component Entity System (code is ActionScript).


Other changes
=============

I've also made some minor style changes:
* Use ToolTips instead of comments on public variables.
 * These show up in the inspector to make them useful without diving into the code.
* Use CompareTag instead of string comparison.
 * I think the function call is as readable as string comparison and it's supposed to be [better for performance](http://answers.unity3d.com/questions/200820/is-comparetag-better-than-gameobjecttag-performanc.html).
* Four spaces for indent.
* No space after method names.

