/* ______                   ___  __            _       
  /_  __/_____ _____ ___   / _ \/ /  __ _____ (_)______
   / / / __/ // (_-<(_-<  / ___/ _ \/ // (_-</ / __(_-<
  /_/ /_/  \_,_/___/___/ /_/  /_//_/\_, /___/_/\__/___/
  Soft-Body Simulation for Unity3D /___/               
                                         Heartbroken :( */

using UnityEngine;

[ExecuteInEditMode]
public class TxWorld : MonoBehaviour
{
    #region Properties

    public int worldID
    {
        get
        {
            if (!TxNative.WorldExists(m_worldID))
            {
                m_worldID = TxNative.CreateWorld();
                TxNative.WorldSetSimulationStep(m_worldID, settings.simulationStep);
                TxNative.WorldSetSimulationSubstepPower(m_worldID, settings.substepPower);
                TxNative.WorldSetSolverIterations(m_worldID, settings.solverIterations);
                TxNative.WorldSetGlobalGravity(m_worldID, settings.globalGravity);
                TxNative.WorldSetGlobalPressure(m_worldID, settings.globalPressure);

                CreateAfterPhysXTrigger();
                SetupCollisionMatrix();
            }
            return m_worldID;
        }
    }

    public TxSceneSettings settings
    {
        get
        {
            if (m_settings == null)
            {
                m_settings = UnityEngine.Object.FindObjectOfType<TxSceneSettings>();
                if (m_settings == null) m_settings = gameObject.AddComponent<TxSceneSettings>();
            }
            return m_settings;
        }
    }

    #endregion

    #region Events

    public delegate void OnBeforePhysXFn();
    public event OnBeforePhysXFn onBeforePhysX;

    public delegate void OnBeforeStepFn();
    public event OnBeforeStepFn onBeforeStep;

    public delegate void OnAfterStepFn();
    public event OnAfterStepFn onAfterStep;

    public delegate void OnAfterUpdateFn();
    public event OnAfterUpdateFn onAfterUpdate;

    #endregion

    #region Methods

    public void AddComponent()
    {
        m_worldID = worldID;
        m_enabledComponents++;
    }

    public void RemoveComponent()
    {
        m_enabledComponents--;
        if (m_enabledComponents <= 0)
        {
            TxNative.ThreadsStopWorkers();
            TxNative.DestroyWorld(m_worldID);
            DestroyImmediate(gameObject);
        }
    }
    public bool RayCast(Vector3 _origin, Vector3 _direction, float _distance, TxBody _skip, out TxBody _body, out Vector3 _point, out Vector3 _normal, out int _face)
    {
        _body = null; int hitObjectID = -1; _point = Vector3.zero; _normal = Vector3.zero; _face = -1;
        if (TxNative.WorldRayCast(m_worldID, _origin, _direction, _distance, _skip ?_skip.objectID : -1, ref hitObjectID, ref _point, ref _normal, ref _face))
        {
            _body = TxBody.Find(hitObjectID);
            return true;
        }
        return false;
    }

    #endregion

    #region Unity

    void OnEnable()
    {
        if (Application.isPlaying)
        {
            int workerThreads = settings.workerThreads < 0 ? SystemInfo.processorCount + settings.workerThreads : Mathf.Min(SystemInfo.processorCount, settings.workerThreads);
            if (workerThreads > 0) TxNative.ThreadsStartWorkers(workerThreads);
        }
    }

    void FixedUpdate()
    {
        if (onBeforePhysX != null) onBeforePhysX();
        m_advanceSimulation = true;
    }

    void OnTriggerStay(Collider _collider)
    {
        if (!Application.isPlaying)
        {
            m_advanceSimulation = false;
        }
        if (m_advanceSimulation)
        {
            m_advanceSimulation = false;
            TxNative.DebugClear();
            if (onBeforeStep != null) onBeforeStep();
            m_simulationTime = Time.realtimeSinceStartup;
            TxNative.WorldAdvance(m_worldID);
            m_simulationTime = Time.realtimeSinceStartup - m_simulationTime;
            if (onAfterStep != null) onAfterStep();
        }
    }

    void Update()
    {
        if (Application.isPlaying)
        {
            m_interpolationTime = Time.realtimeSinceStartup;
            TxNative.WorldInterpolate(m_worldID, (Time.time - Time.fixedTime) / Time.fixedDeltaTime);
            m_interpolationTime = Time.realtimeSinceStartup - m_interpolationTime;
            if (onAfterUpdate != null) onAfterUpdate();
            TxNative.DebugDraw();
        }
        else
        {
            if (TxNative.WorldExists(m_worldID))
            {
                TxNative.WorldInterpolate(m_worldID, 0);
            }
        }
    }

#if TX_SHOWTIMING
    void OnGUI()
    {
        GUILayout.Label(string.Format("Sim {0:F1} ms | Lerp {1:F1} ms", m_simulationTime * 1000.0f, m_interpolationTime * 1000.0f));
    }
#endif

    #endregion

    #region Private

    void CreateAfterPhysXTrigger()
    {
        SphereCollider trigger = gameObject.AddComponent<SphereCollider>();
        trigger.radius = 1.0f;
        trigger.center = Vector3.up * 1000.0f;
        trigger.isTrigger = true;
        gameObject.layer = 31;
        SphereCollider hitter = new GameObject("__Hidden_Trigger_Hitter__").AddComponent<Rigidbody>().gameObject.AddComponent<SphereCollider>();
        hitter.radius = 0.5f;
        hitter.center = Vector3.up * 1000.0f;
        hitter.gameObject.transform.parent = transform;
        hitter.gameObject.hideFlags = HideFlags.HideAndDontSave;
        hitter.attachedRigidbody.constraints = RigidbodyConstraints.FreezePosition;
        hitter.gameObject.layer = 31;
    }

    void SetupCollisionMatrix()
    {
        for (int i = 0; i < 32; ++i)
        {
            for (int j = i; j < 32; ++j)
            {
                bool colliding = !Physics.GetIgnoreLayerCollision(i, j);
                TxNative.WorldSetColliding(m_worldID, i, j, colliding);
            }
        }
    }

    [System.NonSerialized]
    TxSceneSettings m_settings = null;

    [System.NonSerialized]
    int m_enabledComponents = 0;

    int m_worldID = -1;

    float m_simulationTime = 0;
    float m_interpolationTime = 0;

    bool m_advanceSimulation = false;

    #endregion
}