05/25 ~

BUGS:
    Sprites (Height Adjustment)                                             - Fixed
        - Character not facing the right direction                          - Fixed

    Crouch
        - State change not functioning while Horizontal input is held       - Fixed
            Add InputManager() method to Update()

        - Fall -> Crouch animation transition not working                   - Fixed
        - Dash into crouch not working (Crouch needs to be disabled)        - Fixed
    
    Attack
        - Can move while attacking                                          - Fixed
        - ComboAttack3 to Dash/Backdash animation transition not working    - Fixed
        - Move animation plays when crouching is followed directly after    - Fixed
        - Attack animation not working when leaving crouch                  - Fixed
        - Dash into AirHeavyAttack animation does not work                  - 

TODO:
    Polish comboAttack buffering                                            - Implemented
    Implement Attack() method
    AirAttack                                                               - Implemented
    AirHeavy                                                                - Implemented

NOTES:
    Script -> Projectile (Inherit or refer to it)
    Singleton object

----------------------------------

07/24 ~

TODO:
    Implement method: Attack()                                              - Implemented 08/14
    Create individual hitbox objects for every kind of attack motions       - Implemented 07/31

BUGS:
    Attack hitboxes not disappearing even when coroutine is terminated      - Fixed 07/24
    Attack hitboxes not refreshing during combo attacks                     - Fixed 07/24
    Attack hitboxes not facing the right direction                          - Fixed 07/31
        - Used pivot point to rotate the hitboxes
    Attack coroutine not detecting enemies                                  - Fixed 07/31
        - Changed OnCollisionEnter2D to OnTriggerEnter2D
    Hitboxes being moved around weirdly                                     - Fixed 08/14
        - Removed rigidbody components from hitboxes
    Attack types not updating correctly on PlayerAttackHitbox.cs            - Fixed 08/14
        - Pushed the values instead of pulling from PAH.cs

MISC:
    Fixed an awkward animation transition which happens when entering running from comboattack1 and comboattack2 (07/31)