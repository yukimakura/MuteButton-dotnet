using System;
namespace MuteButton.Core
{
	public interface IDetectApplication
	{
		/// <summary>
		/// 特定のキーが入力されたら呼ばれるコールバック
		/// </summary>
		/// <param name="registAction"></param>
		void StartListenAppRegistKey(Action registAction);

		/// <summary>
		/// 登録されたことを通知する関数
		/// </summary>
		/// <param name="NoticeMessage"></param>
		void NoticeRegist(string NoticeMessage);

		/// <summary>
		/// 接続状況を通知する
		/// </summary>
		/// <param name="NoticeMessage"></param>
		void NoticeConnectionStatus(ConnectionStatusType status,string NoticeMessage);

		/// <summary>
		/// ボタンが押されたら登録されているアプリケーションをアクティブにする
		/// </summary>
		/// <returns>アクティブかどうか</returns>
		(bool isSuccess, string failedMsg) ActiveApp();

		/// <summary>
		/// ミュートするアプリケーションとして登録
		/// </summary>
		/// <returns>登録されたハンドル名</returns>
		string RegistApp();
	}
}

