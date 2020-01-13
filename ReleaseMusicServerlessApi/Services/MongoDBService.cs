using MongoDB.Driver;
using ReleaseMusicServerlessApi.Helpers;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ReleaseMusicServerlessApi.Services
{
    public class MongoDBService<TEntity>
    {
        IMongoClient client;
        IMongoDatabase database;
        IMongoCollection<TEntity> collection;

        public MongoDBService(string collection)
        {
            this.client = new MongoClient(Credentials.connectionString);
            this.database = this.client.GetDatabase(Credentials.dataBaseName);
            this.collection = database.GetCollection<TEntity>(collection);
        }

        public void InsertOne(TEntity entity)
        {
            try
            {
                this.collection.InsertOneAsync(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IList<TEntity> ListByFilter(Expression<Func<TEntity, bool>> filter)
        {
            try
            {
                IList<TEntity> items = this.collection.FindAsync(filter).Result.ToList();

                return items;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }
    }
}
