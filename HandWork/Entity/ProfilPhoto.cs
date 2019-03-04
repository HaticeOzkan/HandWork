using Entity.Abstract;

namespace Entity
{
    public class ProfilPhoto:IEntity
    {
        public int ID { get; set; }
        public string ImageURL { get; set; }
        public virtual Member Member { get; set; }
    }
}