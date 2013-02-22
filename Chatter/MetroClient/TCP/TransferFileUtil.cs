using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using Chatter.MetroClient.UI;
using MetroClient.ChatterService;

namespace Chatter.MetroClient.TCP
{
    public abstract class TransferFileUtil
    {


        protected FileMessage fm;
        protected TcpClient client;
        protected int BufferSize = 1024 * 1024;
        protected long progress;
        protected UI.FileTransferGrid fileTransferGrid;
        protected TransferState transferState = TransferState.Wating;


        public TransferFileUtil(FileMessage fm, FileTransferGrid fileTransferGrid)
        {
            this.fm = fm;
            this.fileTransferGrid = fileTransferGrid;
        }


        public void Cancle()
        {
            if(transferState==TransferState.Running)
                transferState = TransferState.CanceledByMyself;
            else if (transferState == TransferState.Wating)
            {
                transferState = TransferState.CanceledByMyself;
                Completed();
            }
        }

        public abstract void Transfer();

        public abstract void Completed();

    }



    public enum TransferState
    {
        Wating,
        Running,
        CanceledByMyself,
        CanceledByTheOther,
        InternetError,
        NoResponse,
        RefusedByTheOther

    }

}