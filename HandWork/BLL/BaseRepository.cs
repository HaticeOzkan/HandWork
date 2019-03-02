using DAL;
using Entity.Abstract;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BaseRepository<T> where T : class, IEntity
    {
        HandWorkContext _Db;
        public BaseRepository(HandWorkContext Db)
        {
            _Db = Db;
        }
        public BaseRepository()
        {

        }
        public List<T> GetAll()
        {
            return _Db.Set<T>().ToList();
        }
        public T GetOne(int Id)
        {
            return _Db.Set<T>().Where(x => x.ID == Id).FirstOrDefault();
        }
        public bool Add(T NewItem)
        {
            try
            {
                _Db.Set<T>().Add(NewItem);
                return true;
            }
            catch (DbEntityValidationException ex)
            {
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool Delete(int Id)
        {
            try
            {
                T Item = _Db.Set<T>().Find(Id);
                _Db.Set<T>().Remove(Item);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        public bool Edit(T NewItem)
        {
            try
            {
                T OldItem = _Db.Set<T>().Find(NewItem.ID);
                _Db.Entry(OldItem).CurrentValues.SetValues(NewItem);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
