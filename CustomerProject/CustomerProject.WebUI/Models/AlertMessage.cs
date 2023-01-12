namespace CustomerProject.WebUI.Models
{
    //Yaplan işlemlerin hakkında bilgilendirme mesajı vermek için, TempDataExtension da kullanılmak üzere oluşturulmuş model.
    public class AlertMessage
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public string AlertType { get; set; }
    }
}
