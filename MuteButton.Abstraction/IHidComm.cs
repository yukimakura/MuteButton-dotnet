using System;
namespace MuteButton.Core
{
	public interface IHidComm
	{
		/// <summary>
		/// HIDデバイスを購読開始する
		/// </summary>
		/// <param name="vendorId"></param>
		/// <param name="productId"></param>
		/// <param name="receiveCallback">デバイスからデータ受信したときにコールバックされる関数</param>
		/// <param name="connectionStateChange">接続状況が変わったらコールバックされる関数</param>
		void StartListen(ushort vendorId,ushort productId,Action<string> receiveCallback,Action<ConnectionStatusType> connectionStateChange);

	}
}

