namespace TodoApp.Core.Entities
{
    public class User : BaseEntity
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string PasswordHash { get; set; }  // Şifreyi hash'lemek için
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }  // Yalnızca bir kez kullanılıyor
        public string Gender { get; set; }
        //public string Password { get; set; }
    }
}