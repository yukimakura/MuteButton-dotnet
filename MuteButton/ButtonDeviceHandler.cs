using System;
using System.Threading.Tasks;

namespace MuteButton.Core
{
    public class ButtonDeviceHandler
    {
        private ushort vendorId;
        private ushort productId;

        private IDetectApplication detectApplication;
        private IHidComm hidComm;
        private IMuteApplication muteApplication;

        public ButtonDeviceHandler(ushort vendorId, ushort productId, IDetectApplication detectApplication, IHidComm hidComm, IMuteApplication muteApplication)
        {
            this.vendorId = vendorId;
            this.productId = productId;
            this.detectApplication = detectApplication;
            this.hidComm = hidComm;
            this.muteApplication = muteApplication;
        }

        public void StartMuteButton()
        {
            Task.Run(() =>
            {
                detectApplication.StartListenAppRegistKey(() =>
                {
                    var handleName = detectApplication.RegistApp();
                    detectApplication.NoticeRegist($"{handleName}をミュート対象として登録");
                });
            });
            Task.Run(() =>
            {
                hidComm.StartListen(vendorId, productId, msg =>
                {
                    if (msg == "S")
                    {
                        var result = detectApplication.ActiveApp();
                        if (!result.isSuccess)
                        {
                            detectApplication.NoticeRegist($"アクティブ化に失敗 {result.failedMsg}");
                            return;
                        }

                        muteApplication.Mute();
                    }
                    else
                        muteApplication.UnMute();

                }, conStatus =>
                {
                    detectApplication.NoticeConnectionStatus(conStatus, conStatus == ConnectionStatusType.Connected ? "デバイスが接続されました" : "デバイスが切断されました");
                });
            });

        }
    }
}

