using System;
using System.IO.Ports;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class CountUpCube : UdonSharpBehaviour
{
    [SerializeField, Header("カウントアップのテキストオブジェクト")]
    private Text _countUpText;
    [SerializeField, Header("オーナーかどうか判別するテキストオブジェクト")]
    private Text _OwnerText;
    
    // 変数同期を行う場合、戦闘に[UdonSynced]を付ける必要がある(権限Ownerしかいじれない)
    [UdonSynced(UdonSyncMode.None)] private int _countNum = 0;

    private void Start()
    {
        // 今自分がオーナーかどうかを判別
        SetOwnerText(Networking.LocalPlayer);
    }

    private void Update()
    {
        // アップデートで常に表示を行う
        _countUpText.text = _countNum.ToString();
    }

    public override void Interact()
    {
        // インタラクトされた時にインタラクトしたプレイヤーの情報をもってくる
        var player = Networking.LocalPlayer;
        // オーナー権限を渡す
        Networking.SetOwner(player, this.gameObject);
        
        // プレイヤーがオーナーかどうかを判別をおこなう
        if (player.IsOwner(this.gameObject))
        // カウントを増やす
        CountUp();
    }

    /// <summary>
    /// SetOwner()をよんだら通る関数、オーナーを譲渡するかどうか true = 許可 false = 不許可
    /// </summary>
    public override bool OnOwnershipRequest(VRCPlayerApi requestingPlayer, VRCPlayerApi requestedOwner)
    {
        // 譲渡を許可
        return true;
    }

    /// <summary>
    /// Ownerが移行した際の処理
    /// </summary>
    /// <param name="player">新しいオーナー</param>
    public override void OnOwnershipTransferred(VRCPlayerApi player)
    { 
        // オーナーが移行したときにテキストに表示する処理
        SetOwnerText(Networking.LocalPlayer);
    }

    /// <summary>
    /// カウントアップ処理
    /// </summary>
    public void CountUp()
    {
        // カウントを増やす
        _countNum++;

        // 20以上になったら初期化
        if (_countNum >= 20)
            _countNum = 0;
        
        // オーナー譲渡直後に動かすとずれる可能性があるので待機する
        SendCustomEventDelayedSeconds(nameof(SerializeData), 0.4f);

        // 表示
        _countUpText.text = _countNum.ToString();
    }

    public void SetOwnerText(VRCPlayerApi player)
    {
        // オーナーだったら
        if (player.IsOwner(this.gameObject))
            _OwnerText.text = $"{player.displayName} は オーナーです";
        // それ以外
        else
            _OwnerText.text = $"{player.displayName} は オーナーではありません";
    }

    /// <summary>
    /// 同期処理関数
    /// </summary>
    public void SerializeData()
    {
        RequestSerialization();
    }
}
