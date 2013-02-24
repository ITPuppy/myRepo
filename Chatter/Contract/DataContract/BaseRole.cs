using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.Serialization;

namespace Chatter.Contract.DataContract
{
    /// <summary>
    /// Member 和 Group的基类
    /// </summary>
    [DataContract]
    [KnownType(typeof(Member))]
    [KnownType(typeof(Group))]
    [KnownType(typeof(UserGroup))]

   public class BaseRole
    {

    }
}
