using Game.GUI.Windows;

namespace ClashKeys.UI
{
internal class WindowDirector
{
    private readonly IWindowsManager _windowsManager;

    public WindowDirector(IWindowsManager windowsManager) => _windowsManager = windowsManager;

    public GameWindowMediatorUI OpenGameWindow() => _windowsManager.OpenWindow<GameWindowMediatorUI>();
    public VictoryWindowMediatorUI OpenVictoryWindow() => _windowsManager.OpenWindow<VictoryWindowMediatorUI>();
    public DefeatWindowMediatorUI OpenDefeatWindow() => _windowsManager.OpenWindow<DefeatWindowMediatorUI>();

    public EnemyFightingWindowMediatorUI OpenEnemyFightingWindow() =>
        _windowsManager.OpenWindow<EnemyFightingWindowMediatorUI>();

    public ChestWindowMediatorUI OpenChestWindow() => _windowsManager.OpenWindow<ChestWindowMediatorUI>();

    public T GetWindow<T>() where T : class, IMediator
    {
        _windowsManager.TryGetFirstWindow<T>(out var mediator);

        return mediator;
    }

    public void CloseWindow(IMediator mediator) => _windowsManager.CloseWindow(mediator);

    public void CloseWindow<T>() where T : class, IMediator
    {
        _windowsManager.CloseWindow<T>();
    }
}
}