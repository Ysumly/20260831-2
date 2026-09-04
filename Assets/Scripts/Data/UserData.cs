using UnityEngine;


/// <summary>
/// 这是一个数据类, 用于存储用户数据.
/// </summary>
//序列化
[System.Serializable]
public class UserData 
{
    public float MusicVolume; // 音量
    public float SFXVolume; // 音效音量
    public int MoneyNumber; // 金钱数量
    public short unlockedRoleNum = 0x1; // 解锁角色序号, 每一位代表一个角色是否解锁, 0表示未解锁, 1表示已解锁, 目前只有6个角色


    public override string ToString() => $"[UserData]MusicVolume: {MusicVolume}, SFXVolume: {SFXVolume}, MoneyNumber: {MoneyNumber}, UnlockedRoleNum: {unlockedRoleNum}";
}

