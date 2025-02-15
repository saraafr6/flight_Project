
namespace Fly.Model.Entity
{
    public abstract class EntityBase
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }

        public EntityBase() 
        { 

        }

        public EntityBase(bool initialize)
        {
            if (initialize)
            {
                Initialize();
            }

        }

        public void Initialize()
        {
            Id = Guid.NewGuid();
            OnInitialize();
        }
        public virtual void OnInitialize()
        {

        }
    }
}