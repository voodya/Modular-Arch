public interface IRuntimeDataHolder
{
    public bool TryGetData<T>(out T data) where T : IModuleRuntimeData;

    public void SetData<T>(T Data) where T : IModuleRuntimeData;
}
