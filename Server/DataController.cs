using Server.DataBase;
using UpdServer;

namespace Server
{
    class DataController
    {
        public int AddStudent(School school)
        {
            using (SchoolContext db = new SchoolContext())
            {
                db.Students.Add(school);
                db.SaveChanges();
                int id = db.Students.OrderBy(s => s.ID).First().ID;
                return id;
            }
        }

        public void DeleteStudent(int comand)
        {            
            using (SchoolContext db = new SchoolContext())
            {
                
                var students = db.Students;
                School school = db.Students.Find(comand);
                if (school == null)
                {
                    throw new ArgumentException();
                }
                else
                {
                    db.Students.Remove(school);
                    db.SaveChanges();
                }
            }
        }

        public School GetSchool(int comand)
        {
            using (SchoolContext db = new SchoolContext())
            {
                return db.Students.FirstOrDefault(p => p.ID == comand);
            }
        }

        public List<School> GetStudents()
        {
            using (SchoolContext db = new SchoolContext())
            {
                var entities = db.Set<School>();
                List<School> SchoolList = new List<School>();
                foreach (var ent in entities.OrderBy(x => x.ID).ToList())
                {
                    SchoolList.Add(ent);
                }
                return SchoolList;
                //return db.Students.ToList();
            }
        }

        public void DeleteAll()
        {
            using (SchoolContext db = new SchoolContext())
            {
                db.Students.RemoveRange(db.Students);
                db.SaveChanges();
            }
        }
    }
}
