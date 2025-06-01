05/25 ~

Bug:
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

Notes:
    Script -> Projectile (Inherit or refer to it)
    Singleton object

----------------------------------

06/01 ~