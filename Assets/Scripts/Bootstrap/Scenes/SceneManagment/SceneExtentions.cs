using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneExtentions
{
    public static T GetRoot<T>(this Scene scene)
    {
        bool instanceFound = false;
        T result = default;

        foreach (GameObject gameObject in scene.GetRootGameObjects())
        {
            if (gameObject.TryGetComponent(out T component))
            {
                if (instanceFound)
                {
                    throw new Exception($"Found multiple instances of type {typeof(T)}");
                }
                result = component;
                instanceFound = true;
            }
        }

        if (!instanceFound)
        {
            throw new Exception($"Can't find instance of type {typeof(T)}");
        }

        return result;
    }
}
