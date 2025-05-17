namespace KoperasiTenterApp.Dtos
{
    public class SuccessOutputDto
    {
        public Guid? UserId { get; set; }
        public bool IsSuccess {  get; set; }
        public string? Message { get; set; }
    }
}
