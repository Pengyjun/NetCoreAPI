namespace HNKC.OperationLogsAPI.Dto.ResponseDto
{
    public class ProjectStatusChangResponse
    {

        public Guid Id { get; set; }
        public string BeforeStautsId { get; set; }
        public string AfterStautsId { get; set; }

        public DateTime Time { get; set; }
    }
}
