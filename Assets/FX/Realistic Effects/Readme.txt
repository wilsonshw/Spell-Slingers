For using effects in runtime, use follow code:

	"Instantiate(prefabEffect, position, rotation);"

Using projectile collision detection:
	
	Just add follow script on prefab of effect.

	void Start () {
        var physicsMotion = GetComponentInChildren<RFX4_PhysicsMotion>(true);
        if (physicsMotion != null) physicsMotion.CollisionEnter += CollisionEnter;

	    var raycastCollision = GetComponentInChildren<RFX4_RaycastCollision>(true);
        if(raycastCollision != null) raycastCollision.CollisionEnter += CollisionEnter;
    }

    private void CollisionEnter(object sender, RFX4_PhysicsMotion.RFX4_CollisionInfo e)
    {
        Debug.Log(e.HitPoint); //a collision coordinates in world space
        Debug.Log(e.HitGameObject.name); //a collided gameobject
        Debug.Log(e.HitCollider.name); //a collided collider :)
    }

------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
Effect modification:

All prefabs of effect have "EffectSetting" script with follow settings:

ParticlesBudget (range 0 - 1, default 1) 
Allow change particles count of effect prefab. For example, particleBudget = 0.5 will reduce the number of particles in half
	
UseLightShadows (does not work when used mobile build target)
Some effect can use shadows and you can disable this setting for optimisation. Disabled by default for mobiles.

UseFastFlatDecalsForMobiles (works only when used mobile build target)
If you use non-flat surfaces or  have z-fight problems you can use screen space decals instead of simple quad decals.
Disabled parameter will use screen space decals but it required depth texture!
    
UseCustomColor
You can override color of effect by HUE. (new color will used only in play mode)
If you want use black/white colors for effect, you need manualy change materials of effects. 

IsVisible 
Disable this parameter in runtime will smoothly turn off an effect. 

FadeoutTime 
Smooth turn off time


Follow physics settings visible only if type of effect is projectile

UseCollisionDetection 
You can disable collision detection and an effect will fly through the obstacles.

LimitMaxDistance
Limiting the flight of effect (at the end the effect will just disappear)

Follow settings like in the rigidbody physics
Mass
Speed
AirDrag
UseGravity
