using Ecard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecard.Services
{
    public interface IOrangeKeyAndopenIDService
    {
        int Insert(OrangeKeyAndopenID item);

        int Update(OrangeKeyAndopenID item);

        int Delete(OrangeKeyAndopenID item);

        OrangeKeyAndopenID GetByopenID(string openID);
        OrangeKeyAndopenID GetByorangeKey(string orangeKey);

        OrangeKeyAndopenID GetBymessageId(string messageId);
        OrangeKeyAndopenID GetById(int id);

    }
}
