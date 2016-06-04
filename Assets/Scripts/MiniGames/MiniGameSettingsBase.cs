using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MiniGameSettingsBase {

    // Game Options List:
    public List<GameOptionChannel> gameOptionsList;
    //= new List<GameOptionChannel>();
        
    public virtual void InitGameOptionsList() {

    }

    public virtual void CopySettingsToSave(MiniGameSettingsSaves miniGameSettingsSaves) {

    }

    public virtual void CopySettingsFromLoad(MiniGameSettingsSaves miniGameSettingsSaves) {

    }
}
