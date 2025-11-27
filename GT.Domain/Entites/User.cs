namespace GT.Domain.Entites
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<Tarefa> Tarefas { get; set; } = new List<Tarefa>();
        
        public User() { }

        public User(string username, string email, string password)
        {
            Username = username;
            Email = email;
            Password = password;
            Tarefas = new List<Tarefa>();
        }
    }
}