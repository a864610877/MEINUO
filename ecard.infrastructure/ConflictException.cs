using System;

namespace Ecard
{
    [Serializable]
    public class ConflictException : Exception
    { 
        public ConflictException( ) : base("���ݳ�ͻ, ��ȷ���Ƿ��ж�������ͬʱ�༭��ǰ�ʻ�")
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