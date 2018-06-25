/* ______                   ___  __            _       
  /_  __/_____ _____ ___   / _ \/ /  __ _____ (_)______
   / / / __/ // (_-<(_-<  / ___/ _ \/ // (_-</ / __(_-<
  /_/ /_/  \_,_/___/___/ /_/  /_//_/\_, /___/_/\__/___/
  Soft-Body Simulation for Unity3D /___/               
                                         Heartbroken :( */

using UnityEngine;

public static class TxPhysics
{
    #region Properties

    static public TxWorld world
    {
        get
        {
            if (sm_world == null)
            {
                sm_world = new GameObject("__Hidden_Truss_Physics_World__").AddComponent<TxWorld>();
                sm_world.gameObject.hideFlags = HideFlags.HideAndDontSave;
                sm_world.onBeforePhysX += OnBeforePhysX;
                sm_world.onBeforeStep += OnBeforeStep;
                sm_world.onAfterStep += OnAfterStep;
                sm_world.onAfterUpdate += OnAfterUpdate;
            }
            return sm_world;
        }
    }

    #endregion

    #region Events

    public delegate void OnBeforePhysXFn();
    public static event OnBeforePhysXFn onBeforePhysX;

    public delegate void OnBeforeStepFn();
    public static event OnBeforeStepFn onBeforeStep;

    public delegate void OnAfterStepFn();
    public static event OnAfterStepFn onAfterStep;

    public delegate void OnAfterUpdateFn();
    public static event OnAfterUpdateFn onAfterUpdate;

    #endregion

    #region Methods

    public static bool RayCast(Vector3 _origin, Vector3 _direction, float _distance, TxBody _skip, out TxBody _body, out Vector3 _point, out Vector3 _normal, out int _face)
    {
        if (sm_world != null)
        {
            return sm_world.RayCast(_origin, _direction, _distance, _skip, out _body, out _point, out _normal, out _face);
        }
        _body = null; _point = Vector3.zero; _normal = Vector3.zero; _face = -1;
        return false;
    }

    #endregion

    #region Private

    static void OnBeforePhysX()
    {
        if (onBeforePhysX != null) onBeforePhysX();
    }
    static void OnBeforeStep()
    {
        if (onBeforeStep != null) onBeforeStep();
    }
    static void OnAfterStep()
    {
        if (onAfterStep != null) onAfterStep();
    }
    static void OnAfterUpdate()
    {
        if (onAfterUpdate != null) onAfterUpdate();
    }

    static TxWorld sm_world = null;

    #endregion
}
