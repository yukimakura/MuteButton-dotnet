using System;
using System.Collections.Generic;
using MuteButton.Core;
using HidSharp.Experimental;
using HidSharp.Reports;
using HidSharp.Reports.Encodings;
using HidSharp.Utility;
using HidSharp;
using System.Linq;
using NLog;
using System.Threading;

namespace MuteButton.WindowsHandler
{
    public class HidComm : IHidComm
    {
        private Logger logger = LogManager.GetLogger("HidCommLogger");
        public HidComm()
        {
        }

        public void StartListen(ushort vendorId, ushort productId, Action<string> receiveCallback, Action<ConnectionStatusType> connectionStateChange)
        {
            var isConnectedLatest = false;
            while (true)
            { 
                try
                {

                    var list = DeviceList.Local;
                    var device = list.GetHidDevices().Where(x => x.ProductID == productId).FirstOrDefault(x => x.VendorID == vendorId);
                    if (device == null)
                    {
                        if (isConnectedLatest)
                            connectionStateChange(ConnectionStatusType.Disconnected);

                        isConnectedLatest = false;
                        continue;
                    }
                    else
                    {
                        if (!isConnectedLatest)
                            connectionStateChange(ConnectionStatusType.Connected);

                        isConnectedLatest = true;
                    }


                    if(device.TryOpen(out var hidStream))
                    {
                        using (hidStream)
                        {
                            var inputReportBuffer = new byte[device.GetMaxInputReportLength()];
                            var inputReceiver = device.GetReportDescriptor().CreateHidDeviceInputReceiver();
                            inputReceiver.Received += (sender, e) =>
                            {
                                Report report;
                                while (inputReceiver.TryRead(inputReportBuffer, 0, out report))
                                {
                                    receiveCallback(System.Text.Encoding.UTF8.GetString(inputReportBuffer));
                                }
                            };
                            inputReceiver.Start(hidStream);

                            while (true)
                                Thread.Sleep(100);
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                }
                finally
                {
                    Thread.Sleep(100);
                }
            }
        }
    }
}

