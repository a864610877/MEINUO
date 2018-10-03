using System;
using System.Data.Common;
using Moonlit.Data;

namespace Ecard
{
    public class TransactionHelper
    {
        private readonly DatabaseInstance _databaseInstance;

        public TransactionHelper(DatabaseInstance databaseInstance)
        {
            _databaseInstance = databaseInstance;
        }
        public Transaction BeginTransaction()
        {
            return new Transaction(_databaseInstance.BeginTransaction());
        }
        public void Commit()
        {
            _databaseInstance.Commit();
        }
        public T CommitAndReturn<T>(T r)
        {
            _databaseInstance.Commit();
            return r;
        }
    }
    public class Transaction : IDisposable
    {
        private DbTransaction _transaction;

        public Transaction(DbTransaction transaction)
        {
            _transaction = transaction;
        }
        public void Commit()
        {
            _transaction.Commit();
        }

        public void Dispose()
        {
            if (_transaction == null)
                return;
            try
            {
                _transaction.Dispose();
            }
            finally
            {
                _transaction = null;
            }
        }
    }
}