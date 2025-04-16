namespace Movie.RequestDTO
{
    public class RequestLoginDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }

    }
    public class RequestSignUpDTO
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

    }
}
