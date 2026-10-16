using System.Collections;
using System.Collections.Generic;

public enum DeathType
{
    Abyss,       // 深渊
    Lava,        // 岩浆
    Spikes,      // 尖刺
    Freeze,      // 冰冻
    Crush,       // 挤压
    Explosion    // 爆炸
}

public enum RespawnMode
{
    FallFromSky,  // 从天空下落
    AtCheckpoint  // 直接在检查点复活
}
