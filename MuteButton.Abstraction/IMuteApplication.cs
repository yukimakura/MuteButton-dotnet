using System;
namespace MuteButton.Core
{
	public interface IMuteApplication
	{
		/// <summary>
		/// ミュートする
		/// </summary>
		void Mute();

		/// <summary>
		/// ミュートを解除する
		/// </summary>
		void UnMute();
	}
}

