﻿/* ______                   ___  __            _       
  /_  __/_____ _____ ___   / _ \/ /  __ _____ (_)______
   / / / __/ // (_-<(_-<  / ___/ _ \/ // (_-</ / __(_-<
  /_/ /_/  \_,_/___/___/ /_/  /_//_/\_, /___/_/\__/___/
  Soft-Body Simulation for Unity3D /___/               
                                         Heartbroken :( */

using System.Collections.Generic;
using UnityEngine;

public class TxComponent : MonoBehaviour
{
    #region Classes

    public class PropertyArrayRO<ValueType>
    {
        int m_count;
        public delegate ValueType ReadCallbackFn(int _index);
        ReadCallbackFn m_readCallback;
        public PropertyArrayRO(int _count, ReadCallbackFn _readCallback)
        {
            m_count = _count;
            m_readCallback = _readCallback;
        }
        public ValueType this[int _index]
        {
            get { return m_readCallback(_index); }
        }
        public int Count
        {
            get { return m_count; }
        }
    }
    public class PropertyArrayRW<ValueType>
    {
        int m_count;
        public delegate ValueType ReadCallbackFn(int _index);
        ReadCallbackFn m_readCallback;
        public delegate void WriteCallbackFn(int _index, ValueType _value);
        WriteCallbackFn m_writeCallback;
        public PropertyArrayRW(int _count, ReadCallbackFn _readCallback, WriteCallbackFn _writeCallback)
        {
            m_count = _count;
            m_readCallback = _readCallback;
            m_writeCallback = _writeCallback;
        }
        public ValueType this[int _index]
        {
            get { return m_readCallback(_index); }
            set { m_writeCallback(_index, value); }
        }
        public int Count
        {
            get { return m_count; }
        }
    }

    #endregion

    #region Properties

    public bool isValid
    {
        get { return m_valid; }
    }

    #endregion

    #region Events

    public delegate void OnAfterCreateFn(TxComponent _component);
    public event OnAfterCreateFn onAfterCreate;

    public delegate void OnBeforeStepFn();
    public event OnBeforeStepFn onBeforeStep;

    public delegate void OnAfterStepFn();
    public event OnAfterStepFn onAfterStep;

    public delegate void OnAfterUpdateFn();
    public event OnAfterUpdateFn onAfterUpdate;

    public delegate void OnBeforeDestroyFn(TxComponent _component);
    public event OnBeforeDestroyFn onBeforeDestroy;

    #endregion

    #region Unity

    void OnEnable()
    {
        TxPhysics.world.AddComponent();
        RegisterDependencies();
        CreateIfReady();
    }

    void OnDisable()
    {
        Destroy();
        ClearDependencies();
        TxPhysics.world.RemoveComponent();
    }

    #endregion

    #region Methods

    public virtual void Create()
    {
        TxBody parentBody = (this is TxBody) ? (this as TxBody).FindParentBody() : (this is TxConstraint) ? (this as TxConstraint).attachedBody : null;
        if (parentBody != null)
        {
            parentBody.onBeforeStep += OnBeforeStep;
            parentBody.onAfterStep += OnAfterStep;
            parentBody.onAfterUpdate += OnAfterUpdate;
        }
        else
        {
            TxPhysics.onBeforeStep += OnBeforeStep;
            TxPhysics.onAfterStep += OnAfterStep;
            TxPhysics.onAfterUpdate += OnAfterUpdate;
        }

        m_valid = true;

        if (onAfterCreate != null) onAfterCreate(this);
    }

    public virtual void Destroy()
    {
        if (onBeforeDestroy != null) onBeforeDestroy(this);

        TxBody parentBody = (this is TxBody) ? (this as TxBody).FindParentBody() : (this is TxConstraint) ? (this as TxConstraint).attachedBody : null;
        if (parentBody != null)
        {
            parentBody.onBeforeStep -= OnBeforeStep;
            parentBody.onAfterStep -= OnAfterStep;
            parentBody.onAfterUpdate -= OnAfterUpdate;
        }
        else
        {
            TxPhysics.onBeforeStep -= OnBeforeStep;
            TxPhysics.onAfterStep -= OnAfterStep;
            TxPhysics.onAfterUpdate -= OnAfterUpdate;
        }

        m_valid = false;
    }

    #endregion

    #region Protected

    protected virtual void OnBeforeStep()
    {
        if (onBeforeStep != null) onBeforeStep();
    }
    protected virtual void OnAfterStep()
    {
        if (onAfterStep != null) onAfterStep();
    }
    protected virtual void OnAfterUpdate()
    {
        if (onAfterUpdate != null) onAfterUpdate();
    }

    protected virtual void RegisterDependencies()
    {
    }
    protected void ClearDependencies()
    {
        foreach (var d in m_dependencies)
        {
            d.onAfterCreate -= OnDependencyCreate;
            d.onBeforeDestroy -= OnDependencyDestroy;
        }
        m_dependencies.Clear();
    }

    protected void AddDependency(TxComponent _component)
    {
        m_dependencies.Add(_component);
        _component.onAfterCreate += OnDependencyCreate;
        _component.onBeforeDestroy += OnDependencyDestroy;
    }
    protected bool DependenciesAreValid()
    {
        foreach (var d in m_dependencies) if (!d.isValid) return false;
        return true;
    }
    protected void OnDependencyCreate(TxComponent _component)
    {
        CreateIfReady();
    }
    protected void OnDependencyDestroy(TxComponent _component)
    {
    }

    #endregion

    #region Private

    void CreateIfReady()
    {
        if (DependenciesAreValid()) Create();
    }

    bool m_valid = false;
    List<TxComponent> m_dependencies = new List<TxComponent>();

    #endregion
}
