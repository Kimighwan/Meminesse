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
    Implement skills
        - HolySlash                                                         - Implemented 08/21
        - LightCut                                                          - Implemented 08/21
        - Buff
    Camera Movement                                                         - Implemented 08/26
        - Used cinemachine

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
    Animations and attacks not aligning                                     - Fixed 08/21
        - Fixed by changing decimals to sexagesimals
    Hitboxes occasionally not appearing while using skills                  - Fixed 08/21
        - Seems like a visual issue? Increased hitbox lifetime for prevention
    Animation bug when exiting from crouch                                  - Fixed 08/26
        - Happens when the transition is too fast
        - Changed transition conditions
    Animation / invincibility / movement input bug when player is hurt/dead - Fixed 08/26
        - Modified conditions on flow control

MISC:
    Fixed an awkward animation transition which happens when entering running from comboattack1 and comboattack2 (07/31)

----------------------------------
방학 개발 정리본:

플레이어 동작:
    개발 내용
    - 서로 중복될 수 없는 state들을 사용하여 다양한 동작 정의
    - 플레이어의 모든 동작은 coroutine을 사용 (사용자 입력을 계속 받을 수 있어야 함)
    - 사용자 입력에 따라 state가 변화하며, state끼리의 전환은 boolean 변수들로 제한함
        ex: 플레이어가 공중에 있을 때 웅크리기로 전환할 수 없음
    - 일부 state에서는 사용자의 입력이 제한됨
        ex: 적에게 피격 시 일정 시간 행동 불가
    - State마다 할당된 애니메이션이 있으며, state가 변화할 때 Unity의 animator 변수도 업데이트되어 해당 상태에 맞는 애니메이션을 출력함

    문제점
    - State의 갯수가 많아지면서 발생하는 state 전환 간 문제
        1. 사용자 입력 처리: 사용자의 입력이 동시에 여러 개 주어졌을 때의 문제
            - 입력을 처리하던 조건문을 여러개로 분할함
            - 조건문이 분할됨으로 인하여 state가 중복될 수 있으므로 state 전환 가능 여부를 판단하는 boolean 변수들을 사용
        2. 애니메이션 전환: 애니메이션이 전환되지 않거나 어색하게 전환되는 문제
            - State 변환 시 반드시 Unity의 animator 변수가 업데이트되게 함
            - 길이가 길거나 유동성이 필요한 애니메이션의 경우 짧은 애니메이션을 이어붙인 형태로 parsing함 (한국말이 기억이안남;;)
                ex: enterCrouch -> crouching -> exitCrouch로 웅크리기 애니메이션을 3분할하였음
            - 사용자의 입력이 너무 짧은(1프레임: 1/60초 내 변화) 경우를 대비하여 연속되는 애니메이션의 경우 조건을 널널하게 설정함
                ex: 1프레임 내에 웅크리기 키를 눌렀다 떼는 경우 enterCrouch -> crouching -> exitCrouch에서 crouching -> exitCrouch로 전환 감지가 불가능, crouching -> exitCrouch의 조건을 완화함(기존: 웅크리기 키가 떼지는 시점에 exitCrouch 진입, 완화: 웅크리기 키가 눌러져 있지 않을 때 exitCrouch 진입)
        3. 의도하지 않은 효과: 해당 state에서 가능/불가능해야 할 동작들이 블가능/가능한 문제
            ex: 적에게 피격 시 일정 시간 행동 불가 상태에 빠져야 하지만 이동이 가능
            - 클린 코드가 가장 중요함
            - 상태 전환을 담당하는 boolean 변수들을 한 곳에서 관리
            - 사용자 입력을 조건문을 통해서만 처리하지 않고 각종 boolean 변수로 허용/금지 입력 관리

    - 타 객체(지형, 적)와의 상호작용
        1. 지형
            - Unity의 Raycasting 기능을 사용하여 지형을 탐지 후 플레이어의 state를 업데이트함
        2. 적
            - 타 조원과의 협동이 필요, 서로 상호작용시 필요한 메소드들의 형식을 정의한 후 그에 맞추어 개발 진행
            - 플레이어의 다양한 공격 동작에 맞춰 적 탐지 범위를 변경, 해당 범위 내에 적 감지 시 적 객체의 정보를 불러와 대미지를 입히며, 대미지 누적 시 사망 이벤트(적 객체 비활성화) 발생
            - 적의 공격 범위 내 플레이어가 존재 시 플레이어는 대미지를 입으며, 대미지 누적 시 사망 이벤트(플레이어 객체 비활성화) 발생

스킬 기획:
    - 유저가 다양한 게임 플레이 방식 중 하나를 채택할 수 있도록 설계함
    - 쉽게 획득할 수 있는 일반 패시브 스킬들과 어느정도 게임 진행 시 얻을 수 있는 상위 패시브 스킬로 구별
    - 일반 패시브 스킬들은 탐험을 통하여 얻을 수 있으며, 단순히 각종 수치(체력, 공격력)들만을 늘려주는 역할을 함
    - 상위 패시브 스킬은 고위험/높은 보상, 적당한 위험/적당한 보상, 저위험/낮은 보상 셋 중에 하나를 선택 가능, 선택 시 게임 플레이 방식이 변화함

게임 카메라(사용자 시점):
    - Cinemachine을 활용하여 다양한 카메라 움직임을 구현 가능

----------------------------------

09/19 ~

TODO:
    Implement MapController                                                 - Implemented 09/19
    Implement Portals                                                       - Implemented 09/19
    Implement PlayerDataControl                                             - Implemented 10/09
    Implement barrier with def                                              - Implemented 10/09
    Implement sideways invincibility barrier                                - Implemented 10/12
    Implement more respawn point functionalities for mapcontroller          - Implemented 10/12
    Skill cooldowns                                                         - Implemented 10/12
    Limit vertical velocity when falling                                    - Implemented 11/09

BUGS:
    Player not being teleported                                             - Fixed 09/28
        - Code was not being executed properly because of bad import
    Player can't exit out of jump when on tilemaps                          - Fixed 09/28
        - Changed composite collider 'Geometry Type' to polygons
    Player wall scan raycasts are too far apart                             - Fixed 10/09
        - Added additional raycasts
    Player input not being received                                         - Fixed 10/12
        - Fixed naming issues in playercontroller and added indices in SettingDataManager
    Player being able to turn around when paused
        - Added timescale checks on movement-related code                   - Fixed 11/09

MISC:
    Levels Q/A