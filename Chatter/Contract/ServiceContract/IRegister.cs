﻿
using Chatter.Contract.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;


namespace Chatter.Contract.ServiceContract
{
    [ServiceContract]
    public interface IRegister
    {

        [OperationContract(IsOneWay = false)]
        Member Register(Member member);
    }
}
