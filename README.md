I made really basic physics system for unity 2d. I used Unity 2018.4 version. I used BoxCastNonAlloc for chekcing collisions. I didnt prefer rigidbody.cast.
There are two main functions for movement. "Horizontal", "Vertical".In each fixedUpdate frame one cast goes only vertical and one cast goes horizontal. Otherwise
When object try to move horizontaly when it is on ground object jittering and cant move. I made seperately those movements.
