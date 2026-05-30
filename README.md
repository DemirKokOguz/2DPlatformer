I created a basic kinematic player controller. Unity version that I used for the project is Unity 2018.4 LTS. There are 2 classes. "CustomPhysics" and "PlayerMovement". The PlayerMovement class inherits from CustomPhysics

PlayerMovement takes inputs and moves the player.

CustomPhysics controls the physics system. It checks for collisions by casting vertical and horizontal rays.
