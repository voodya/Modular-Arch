using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class SceneHolder : ISceneHolder
{
    private readonly Dictionary<Type, AssetReference> _scenes = new();
    private readonly Dictionary<Type, (IScene, AsyncOperationHandle<SceneInstance>)> _loadedScenes = new();

    public async UniTask<T> GetScene<T>() where T : class, IScene
    {
        if (_loadedScenes.ContainsKey(typeof(T)))
            return _loadedScenes[typeof(T)].Item1 as T;

        return await LoadScene<T>();
    }

    private async UniTask<T> LoadScene<T>() where T : class, IScene
    {
        Type screenType = typeof(T);

        AsyncOperationHandle<SceneInstance> loadingOperation = Addressables.LoadSceneAsync(_scenes[screenType], LoadSceneMode.Additive);

        T scene = (await loadingOperation).Scene.GetRoot<T>();
        _loadedScenes[screenType] = (scene, loadingOperation);
        return scene;
    }

    public void RegisterScene<T>(AssetReference reference) where T : IScene
    {
        if (_scenes.ContainsKey(typeof(T)))
        {
            Debug.LogError($"Scene {typeof(T)} alredy added");
            return;
        }

        _scenes[typeof(T)] = reference;
    }


}
