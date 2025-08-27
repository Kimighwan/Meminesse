    private IEnumerator Attack(AttackType type)
    {
        //float attackTimer = 0;
        float maxScanTime = 0;
        float attackDamage = 0;

        setAttackType(type);

        // Select hitboxes
        switch (type)
        {
            case AttackType.GroundedComboAttack1:
                currentHitbox = groundAttackHitbox[0];
                maxScanTime = 0.14f;
                attackDamage = damage_GroundedComboAttack[0];
                break;
            case AttackType.GroundedComboAttack2:
                currentHitbox = groundAttackHitbox[1];
                maxScanTime = 0.12f;
                attackDamage = damage_GroundedComboAttack[1];
                break;
            case AttackType.GroundedComboAttack3:
                currentHitbox = groundAttackHitbox[2];
                maxScanTime = 0.22f;
                attackDamage = damage_GroundedComboAttack[2];
                break;
            case AttackType.CrouchAttack:
                currentHitbox = crouchAttackHitbox;
                maxScanTime = 0.12f;
                attackDamage = damage_CrouchAttack;
                break;
            case AttackType.AirAttack:
                currentHitbox = airAttackHitbox;
                // Needs to be adjusted
                maxScanTime = 0.15f;
                attackDamage = damage_AirAttack;
                break;
            case AttackType.AirHeavyAttack:
                currentHitbox = airHeavyAttackHitbox[0];
                // Needs to be adjusted
                maxScanTime = 0.15f;
                attackDamage = damage_AirHeavyAttack;
                break;
            default:
                Debug.Log("What's this?\nHow did we get here?: " + type);
                break;
        }

        currentHitbox.SetActive(true);

        // Flip the hitbox according to the player's facing direction

        if (playerSpriteRenderer.flipX)
        {
            hitboxPivot.transform.localScale = new Vector3(-1, 1, 0);
        }
        else
        {
            hitboxPivot.transform.localScale = new Vector3(1, 1, 0);
        }

        // Runs different types of hit scans according to the attack type
        switch (type)
        {
            // Attacks twice
            case AttackType.AirAttack:
                break;
            // Has three different type of attacks
            case AttackType.AirHeavyAttack:

                break;
            // Ground Combo, Crouch -> Basic hit scan
            default:
                // Perform hit scan for a given time
                yield return new WaitForSeconds(maxScanTime);
                break;
        }

        currentHitbox.SetActive(false);
        isAttacking = false;
        Debug.Log("Current attacktype: " + currentAttackType);

        yield return null;
    }

    private void setAttackType(AttackType aType)
    {
        currentAttackType = aType;
        Debug.Log("Attack type set to: " + currentAttackType + aType);
    }

    public AttackType getAttackType()
    {
        Debug.Log("Returning: " + currentAttackType);
        return currentAttackType;
    }