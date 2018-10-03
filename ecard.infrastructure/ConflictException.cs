using System;

namespace Ecard
{
    [Serializable]
    public class ConflictException : Exception
    { 
        public ConflictException( ) : base("数据冲突, 请确认是否有多人正在同时编辑当前帐户")
        {
        } 
    } 
    [Serializable]
    public class EntityNoFoundException : Exception
    {
        public EntityNoFoundException()
        {
        } 
    } 
}