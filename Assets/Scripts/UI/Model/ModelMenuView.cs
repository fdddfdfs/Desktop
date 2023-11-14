public class ModelMenuView : SubmenuView<ModelData>
{
    private const int AlwaysUnlocked = 1;
    
    public override void Init(Submenu<ModelData> submenu, GameConfig gameConfig)
    {
        base.Init(submenu, gameConfig);
        
        for (var i = 0; i < AlwaysUnlocked; i++)
        {
            _lockers[i].enabled = false;
        }
        
        for (int i = AlwaysUnlocked; i < _lockers.Count; i++)
        {
            _lockers[i].enabled = gameConfig.IsDemo;
        }
    }
}